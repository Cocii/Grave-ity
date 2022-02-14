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
    public AudioSource audioEffectSource;
    [Space]

    public VentStates state = VentStates.NoTarget;
    public float maxPushDistance;
    public float startPushThresholdDistance;
    public float defaultForceMagnitude;
    public LayerMask layerToAffect;
    public float maxCheckDistance = 25f;

    [Space]

    public bool usePropFollow = true;
    public BoxCollider2D propFollowCollider;
    public Vector2 propFollowColliderDefaultSize = new Vector2(1f, 1f);

    [Space]

    public float forceMagnitude;
    public Rigidbody2D targetBody;
    public bool applyForce = false;
    public float targetDistance;
    public float rayHitDistance;
    private Vector2 hitPoint;
    public TargetTypeEnum targetType;
    public Vector2 boxCastOrigin;
    public Vector2 boxCastSize;
    

    void Start()
    {
        gManager = GravityManager.instance;

        Vector2 up = transform.up;
        Vector2 perp = Vector2.Perpendicular(up);

        Vector2 sizeUp = up * maxCheckDistance * 2f;
        Vector2 sizePerp = perp * transform.lossyScale.x * 0.9f;
        boxCastSize = new Vector2(sizePerp.x + sizeUp.x, sizePerp.y + sizeUp.y);
        boxCastSize.Set(Mathf.Abs(boxCastSize.x), Mathf.Abs(boxCastSize.y));

        Vector3 offset = up * boxCastSize * 0.5f;

        boxCastOrigin = transform.position + offset;
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
                target = GetObjectOnTopBoxCast(maxPushDistance * 1.25f);

                if (target != null) {
                    targetBody=target.GetComponent<Rigidbody2D>();
                    state = VentStates.TargetAcquired;
                }
                break;

            case VentStates.TargetAcquired:
                target = GetObjectOnTopBoxCast(maxPushDistance * 1.25f);

                if (target == null) {
                    ResetState();
                    break;
                }

                if (targetDistance <= startPushThresholdDistance) {
                    state = VentStates.TargetPushing;
                }
                else {
                    ResetPropCollider();
                }

                break;

            case VentStates.TargetPushing:
                target = GetObjectOnTopBoxCast(maxPushDistance * 2f);

                if (target==null) {
                    ResetState();
                    break;
                }

                if(targetDistance < maxPushDistance) {
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
        state = VentStates.NoTarget;
        hitPoint = Vector3.zero;

        if (usePropFollow)
            ResetPropCollider();
    }

    private void ApplyBehaviourTargetTypeBased() {
        switch (targetType) {
            case TargetTypeEnum.Prop:
                if(usePropFollow)
                    MovePropCollider(Vector3.Distance(hitPoint, transform.position));

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
        //Vector2 size = transform.lossyScale;
        //size.x *= 0.9f;
        //size.y = checkDistance * 2f;
        
        //Vector2 origin = transform.position + new Vector3(0f, -0.1f * Mathf.Sign(transform.up.y), 0f);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, transform.up, 0f, layerToAffect);
        if (hit) {
            targetDistance = (hit.transform.position - transform.position).y;
            //hitPoint = hit.point;

            //hitPoint = Physics2D.ClosestPoint(new Vector2((hit.transform.position - transform.position).x, 0f)*0.975f, hit.collider);
            hitPoint = hit.collider.ClosestPoint(hit.transform.position - new Vector3(0f, targetDistance));
            
            //hitPoint = Physics2D.ClosestPoint(transform.position, hit.collider);
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

        Debug.DrawRay(origin, transform.up * maxPushDistance * 2f, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(origin, transform.up, maxPushDistance * 2f, layerToAffect);
        if (hit) {
            return hit.distance;
        }

        return -1;
    }

    private void OnDrawGizmos() {
        if (!enabled)
            return;

        Gizmos.color = Color.magenta;

        Vector2 up = transform.up;
        Vector2 perp = Vector2.Perpendicular(up);

        Vector2 sizeUp = up * maxCheckDistance * 2f;
        Vector2 sizePerp = perp * transform.lossyScale.x * 0.9f;
        Vector2 size = new Vector2(sizePerp.x + sizeUp.x, sizePerp.y + sizeUp.y);
        size.Set(Mathf.Abs(size.x), Mathf.Abs(size.y));

        Vector3 offset = up * size * 0.5f;

        Vector2 origin = transform.position + offset;
        
        Gizmos.DrawWireCube(origin, size);

        Gizmos.color = Color.green;
        if (hitPoint != Vector2.zero)
            Gizmos.DrawSphere(hitPoint, 0.25f);

        Gizmos.color = Color.white;
    }

    private void MovePropCollider(float distance) {
        //print("Fan: moving coll at distance " + distance);

        //ONLY PUSH WITH WEAK GRAVITY
        if (distance == 0 || gManager.gravityRatio >= 1f) {
            ResetPropCollider();
            return;
        }

        float sizeOffset = transform.lossyScale.y * propFollowCollider.size.y;
        float distanceOffset = (distance - sizeOffset) * (1 / transform.lossyScale.y) * 0.999f;
        propFollowCollider.offset = new Vector2(0f, distanceOffset);

        propFollowCollider.size = propFollowColliderDefaultSize;
    }

    private void ResetPropCollider() {
        propFollowCollider.size = propFollowColliderDefaultSize;
        propFollowCollider.offset = Vector2.zero;
    }

    public void Activation() {
        if(audioEffectSource)
            audioEffectSource.Play();
    }

    public void Deactivation() {
        if(audioEffectSource)
            audioEffectSource.Pause();
    }
}
