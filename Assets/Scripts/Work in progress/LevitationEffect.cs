using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationEffect : MonoBehaviour
{
    public enum LevitationModeEnum {
        overTime,
        groundDistance,
        centerOfGravity
    }

    [Header("Settings")]
    public LevitationModeEnum mode = LevitationModeEnum.groundDistance;
    public Rigidbody2D body;

    [Header("Ground distance settings")]
    public float distanceFromGround;
    public LayerMask groundLayer;
    public float groundForceMagnitude;
    public float groundForcePositiveMult;

    [Header("Over time settings")]
    [Range(0,2)]
    public float antiGravityMult;
    //[Range(0, 5)]
    //public float multSpeed;
    //public float multMaxAmplitude = 1f;

    [Header("Over time info")]
    public Vector2 antiGravityForce;
    [Range(0, 1)]
    public float sinTimeMult;

    [Header("Center settings")]
    public float centerHeight;
    public float centerForceMagnitude;
    public float maxDistanceFromCenter;



    void Update()
    {
        Vector2 gravity = Physics2D.gravity;
        Vector2 forceToApply = Vector2.zero;

        switch (mode) {
            case LevitationModeEnum.groundDistance:
                RaycastHit2D hit = Physics2D.Raycast(transform.position, gravity.normalized, distanceFromGround, groundLayer);
                //Debug.DrawRay(transform.position, gravity.normalized * distanceFromGround, Color.yellow);
                if (hit) {
                    forceToApply = -gravity.normalized * groundForceMagnitude;
                    
                    if((body.velocity.y > 0.1f && gravity.normalized==Vector2.down) || (body.velocity.y < -0.1f && gravity.normalized == Vector2.up)) {
                        forceToApply *= groundForcePositiveMult;
                    }

                }

                break;

            case LevitationModeEnum.overTime:
                sinTimeMult = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup));
                //sinTimeMult = 1f - ((Mathf.Sin(Time.realtimeSinceStartup * multSpeed) + 1) * multMaxAmplitude / 2f);
                antiGravityForce = sinTimeMult * -(gravity * antiGravityMult);
                forceToApply = antiGravityForce;
                break;

            case LevitationModeEnum.centerOfGravity:
                if(body.drag != 0) {
                    print("Body drag of " + gameObject.name + " changed to 0");
                    body.drag = 0;
                }
                

                Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);

                

                RaycastHit2D hitCenter = Physics2D.Raycast(transform.position, gravity.normalized, 30f, groundLayer);
                //Debug.DrawRay(transform.position, gravity.normalized * distanceFromGround, Color.yellow);
                if (hitCenter) {
                    float currentHeight = Vector2.Distance(currentPosition, hitCenter.point);

                    
                    float distanceFromCenter = centerHeight - currentHeight;

                    Vector2 targetPos = currentPosition + new Vector2(0f, distanceFromCenter);

                    

                    float distanceMult = Mathf.Pow(distanceFromCenter / maxDistanceFromCenter, 2);
                    //print(mult);
                    //forceToApply = -gravity.normalized * centerForceMagnitude * mult;

                    //if (currentPosition.y > centerHeight)
                    //    forceToApply *= -1f;

                    forceToApply = (targetPos - currentPosition).normalized * centerForceMagnitude * distanceMult;

                }


                break;

            default:
                print("Mode not implemented");
                break;
        }

        body.AddForce(forceToApply);
    }
}
