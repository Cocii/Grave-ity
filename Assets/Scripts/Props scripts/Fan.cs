using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    public enum VentStates {
        NoTarget,
        TargetAcquired,
        TargetPushing
    }

    public enum TargetTypeEnum {
        Prop,
        Player
    }

    GravityManager gManager;
    public VentStates state = VentStates.NoTarget;
    public Vector2 colliderDefaultSize = new Vector2(1f, 1f);
    public float raiseHeight;
    public float pushHeightThreshold;
    public float defaultForceMagnitude;
    public float forceMagnitude;
    public LayerMask layerToAffect;
    public Rigidbody2D targetBody;
    public BoxCollider2D underPropCollider;
    public bool applyForce = false;
    //public Vector2 forceDirection = Vector2.up;
    public float targetDistance;
    public float rayHitDistance;
    private Vector2 hitPoint;
    public TargetTypeEnum targetType;
    

    void Start()
    {
        gManager = GravityManager.instance;
    }

    private void FixedUpdate() {
        if (!applyForce)
            return;

        //print("applying force");
        Vector2 force = transform.up * forceMagnitude;
        targetBody.AddForce(force);
        

    }

    void Update() {
        forceMagnitude = (2 - gManager.gravityRatio) * defaultForceMagnitude;
        GameObject target; 

        switch (state) {
            case VentStates.NoTarget:
                target = GetObjectOnTopBoxCast(raiseHeight * 1.25f);

                if (target != null) {
                    targetBody=target.GetComponent<Rigidbody2D>();
                    state = VentStates.TargetAcquired;
                }
                break;

            case VentStates.TargetAcquired:
                target = GetObjectOnTopBoxCast(raiseHeight * 1.25f);

                if (target == null) {
                    //applyForce = false;
                    //targetBody = null;
                    //ResetPropCollider();
                    //state = VentStates.NoTarget;
                    ResetState();
                    break;
                }

                if (targetDistance <= pushHeightThreshold) {
                    state = VentStates.TargetPushing;
                }
                else {
                    ResetPropCollider();
                }

                break;

            case VentStates.TargetPushing:
                target = GetObjectOnTopBoxCast(raiseHeight * 2f);

                if (target==null) {
                    //applyForce = false;
                    //targetBody = null;
                    //ResetPropCollider();
                    //state = VentStates.NoTarget;
                    ResetState();
                    break;
                }

                //print("hisdistance: " + hitDistance + " - raiseHeight: " + raiseHeight);
                if(targetDistance < raiseHeight) {
                    applyForce = true;
                }
                else {
                    applyForce = false;
                }

                ApplyBehaviourTargetTypeBased();

                break;

            default:
                break;
        }

    }

    private void ResetState() {
        applyForce = false;
        targetBody = null;
        ResetPropCollider();
        state = VentStates.NoTarget;
    }

    private void ApplyBehaviourTargetTypeBased() {
        switch (targetType) {
            case TargetTypeEnum.Prop:
                //print("moving collider");
                MovePropCollider(GetObjectOnTopDistanceRayCast());

                break;


            case TargetTypeEnum.Player:

                if (gManager.gravityRatio <= 1f) {
                    print("locking player movement");
                    PlayerManager.instance.body.velocity *= Vector2.up;
                }

                break;

            default:
                break;
        }
    }

    private GameObject GetObjectOnTopBoxCast(float checkDistance) {
        Vector2 size = transform.lossyScale;
        size.x *= 0.9f;
        size.y = checkDistance * 2f;
        Vector2 origin = transform.position + new Vector3(0f, -0.1f * Mathf.Sign(transform.up.y), 0f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, size, 0f, transform.up, 0f, layerToAffect);
        if (hit) {
            targetDistance = (hit.transform.position - transform.position).y;
            hitPoint = hit.point;
            return hit.transform.gameObject;
        }

        targetDistance = -1f;
        return null;
    }

    private GameObject GetObjectOnTopRayCast(float checkDistance) {
        Vector3 origin = new Vector3(targetBody.transform.position.x, transform.position.y, transform.position.z);

        Debug.DrawRay(origin, transform.up * checkDistance, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.up, checkDistance, layerToAffect);
        if (hit) {
            rayHitDistance = hit.distance;
            return hit.transform.gameObject;
        }

        rayHitDistance = -1f;
        return null;
    }

    private float GetObjectOnTopDistanceRayCast() {
        Vector2 origin = new Vector3(targetBody.transform.position.x, transform.position.y, transform.position.z);

        Debug.DrawRay(origin, transform.up * raiseHeight * 2f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.up, raiseHeight*2f, layerToAffect);
        if (hit) {
            
            return hit.distance;
        }

        return -1;
    }

    private void OnDrawGizmos() {
        //Gizmos.color = Color.magenta;
        //Vector2 size = transform.lossyScale;
        //size.y = 19.5f * 1.25f;
        //Gizmos.DrawCube(transform.position + new Vector3(0f, size.y * 0.5f, 0f), size);

        Gizmos.color = Color.green;
        if (hitPoint != Vector2.zero)
            Gizmos.DrawSphere(hitPoint, 0.25f);

        Gizmos.color = Color.white;
    }

    private void MovePropCollider(float distance) {
        //print("Fan: moving coll at distance " + distance);

        if (distance == 0 || gManager.gravityRatio >= 1f) {
            ResetPropCollider();
            return;
        }

        float sizeOffset = transform.lossyScale.y * underPropCollider.size.y;
        float distanceOffset = (distance - sizeOffset) * (1 / transform.lossyScale.y) * 0.999f;
        underPropCollider.offset = new Vector2(0f, distanceOffset);

        underPropCollider.size = colliderDefaultSize;
    }

    private void ResetPropCollider() {
        underPropCollider.size = colliderDefaultSize;
        underPropCollider.offset = Vector2.zero;
    }
}
