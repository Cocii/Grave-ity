using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vent : MonoBehaviour
{
    public enum VentStates {
        NoTarget,
        TargetAcquired,
        TargetPushed
    }

    GravityManager gManager;
    public VentStates state = VentStates.NoTarget;
    public Vector2 colliderDefaultSize = new Vector2(1f, 1f);
    public float raiseHeight;
    public float defaultForceMagnitude;
    public float forceMagnitude;
    public LayerMask layerToAffect;
    public Rigidbody2D targetBody;
    public BoxCollider2D underPropCollider;
    public bool applyForce = false;
    public Vector2 forceDirection = Vector2.up;
    public float hitDistance;

    void Start()
    {
        gManager = GravityManager.instance;
    }

    private void FixedUpdate() {
        if (!applyForce)
            return;

        Vector2 force = forceDirection * forceMagnitude;
        targetBody.AddForce(force);
        

    }

    void Update() {
        forceMagnitude = (2 - gManager.gravityRatio) * defaultForceMagnitude;
        GameObject target = CheckObject(raiseHeight * 1.25f);

        switch (state) {
            case VentStates.NoTarget:
                if (target != null) {
                    targetBody=target.GetComponent<Rigidbody2D>();
                    state = VentStates.TargetAcquired;
                }
                break;

            case VentStates.TargetAcquired:
                if (target == null) {
                    applyForce = false;
                    targetBody = null;
                    ResetPropCollider();
                    state = VentStates.NoTarget;
                    break;
                }

                if (hitDistance <= raiseHeight) {
                    state = VentStates.TargetPushed;
                }

                break;

            case VentStates.TargetPushed:
                if (target==null) {
                    applyForce = false;
                    targetBody = null;
                    ResetPropCollider();
                    state = VentStates.NoTarget;
                    break;
                }

                applyForce = (hitDistance < raiseHeight);
                MovePropCollider(hitDistance);
                break;

            default:
                break;
        }



        //Debug.DrawRay(transform.position, direction * raiseHeight, Color.cyan);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raiseHeight, layerToAffect);
        //if (hit) {
        //    targetBody = hit.transform.gameObject.GetComponent<Rigidbody2D>();
        //}
        //else {
        //    if (targetBody)
        //        targetBody = null;

        //    hit = Physics2D.Raycast(transform.position, direction, raiseHeight*1.2f, layerToAffect);
        //    if (hit && gManager.gravityRatio < 1f) {
        //        MovePropCollider(hit.distance);
        //    }
        //    else {
        //        MovePropCollider(0f);
        //    }
        //}
    }

    private GameObject CheckObject(float checkDistance) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, forceDirection, checkDistance, layerToAffect);
        if (hit) {
            hitDistance = hit.distance;
            return hit.transform.gameObject;
        }

        hitDistance = -1f;
        return null;
    }

    private void MovePropCollider(float distance) {
        if (distance == 0 || gManager.gravityRatio >= 1f) {
            ResetPropCollider();
            return;
        }

        float sizeOffset = transform.lossyScale.y * underPropCollider.size.y;

        underPropCollider.offset = new Vector2(0f, (distance - sizeOffset) * (1 / transform.lossyScale.y));
        underPropCollider.size = colliderDefaultSize;
    }

    private void ResetPropCollider() {
        underPropCollider.size = colliderDefaultSize;
        underPropCollider.offset = Vector2.zero;
    }
}
