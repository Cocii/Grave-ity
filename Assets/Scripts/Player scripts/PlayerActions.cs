using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    PlayerManager manager;

    [Header("Grab info")]
    public GameObject grabbedObj;
    public float grabPositioningDistance;

    [Header("Grab settings")]
    public float grabCheckDistance;
    public LayerMask propsLayer;
    public float grabMassMult = 0.1f;
    public float grabReleaseDistanceOffset = 0.001f;

    [Header("Dash info")]
    public float lastDashTime = 0f;

    private void Start() {
        manager = PlayerManager.instance;
    }

    private void Update() {
        CheckGrabbedObj();
        CheckDash();
    }

    //CHECKS-----------------------------

    private void CheckGrabbedObj() {
        if (grabbedObj == null)
            return;

        float distance = Vector3.Distance(transform.position, grabbedObj.transform.position);
        print(distance);
        if (distance > grabPositioningDistance + grabReleaseDistanceOffset) {
            print(distance + " eccede la distanza di grab " + (grabPositioningDistance+grabReleaseDistanceOffset));
            ReleaseGrab();
        }

        //if (grabbedObj == null)
        //    return;

        //RaycastHit2D hit = Physics2D.Raycast(grabbedObj.transform.position, manager.currentGravity.normalized, grabbedObj.GetComponent<BoxCollider2D>().size.y * 0.75f);
        //if (!hit) {
        //    print("no terrain under grabbed obj");
        //    if (grabbedObj.GetComponent<DistanceJoint2D>())
        //        grabbedObj.GetComponent<DistanceJoint2D>().breakForce = 50f;
        //}
    }

    private void CheckDash() {
        if (!manager.isDashing)
            return;

        if (CheckPropInFront())
            StopDash(0.05f);
    }

    //GRAB-----------------------------

    public void Grab() {
        RaycastHit2D hit;
        Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(transform.position, direction, grabCheckDistance, propsLayer);
        if (hit) {
            grabPositioningDistance = (float)(manager.bodyCollider.size.x * 0.75) + (hit.transform.localScale.x * 0.5f);
            AttachGrabbedObject(hit.transform.gameObject);
        }
    }

    private void AttachGrabbedObject(GameObject toAttach) {
        grabbedObj = toAttach;
        
        DistanceJoint2D grabbedObjJoint = grabbedObj.GetComponent<DistanceJoint2D>();
        if (grabbedObjJoint == null) {
            grabbedObj.AddComponent<DistanceJoint2D>();
            grabbedObjJoint = grabbedObj.GetComponent<DistanceJoint2D>();

            grabbedObjJoint.enableCollision = true;
            grabbedObjJoint.autoConfigureDistance=false;
            //grabbedObjJoint.distance = grabPositioningDistance;
            grabbedObjJoint.breakForce = 1000f;
        }

        grabbedObjJoint.connectedBody = manager.body;
        grabbedObjJoint.distance = grabPositioningDistance;
        grabbedObj.GetComponent<Rigidbody2D>().mass = grabbedObj.GetComponent<Rigidbody2D>().mass * grabMassMult;
        manager.isGrabbing = true;
    }

    public void ReleaseGrab() {
        if (grabbedObj == null)
            return;

        print("Release grab");
        manager.isGrabbing = false;
        grabbedObj.GetComponent<Rigidbody2D>().mass = grabbedObj.GetComponent<Rigidbody2D>().mass * (1/grabMassMult);

        DistanceJoint2D grabbedObjJoint = grabbedObj.GetComponent<DistanceJoint2D>();
        if (grabbedObjJoint != null) {
            grabbedObjJoint.connectedBody = grabbedObj.GetComponent<Rigidbody2D>();  
        }

        grabbedObj = null;
    }

    //DASH-----------------------------

    private bool CheckPropInFront() {
        RaycastHit2D hit;
        Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(transform.position, direction, manager.dashCheckDistance, propsLayer);
        if (hit) {
            return true;
        }
        return false;
    }

    private bool CanDash() {
        if (manager.isDashing || (Time.time - lastDashTime) <= manager.dashCooldown || CheckPropInFront())
            return false;

        //RaycastHit2D hit;
        //Vector2 direction = manager.input.GetMoveInput;
        //hit = Physics2D.Raycast(transform.position, direction, manager.dashCheckDistance, propsLayer);

        //return hit.transform == null;

        return true;
    }

    public void Dash() {
        //print("Try to dash");

        if (CanDash()) {
            StartCoroutine(DashCo());
        }
    }

    private IEnumerator DashCo() {
        manager.isDashing = true;
        Vector2 force = manager.dashForceMult * manager.moveForceMagnitude * manager.input.GetMoveInput;
        manager.characterController.SetDashForce(force);
        lastDashTime = Time.time;

        yield return new WaitForSeconds(manager.dashTime);

        StopDash(0.25f);
        yield break;
    }

    private void StopDash(float stopSpeedMult) {
        manager.isDashing = false;
        manager.body.velocity *= new Vector2(stopSpeedMult, 1f);
    }

    //CROUCH-----------------------------

    public void Crouch() {
        if (!manager.isCrouching) {
            manager.currentMaxMoveSpeed *= manager.crouchMoveSpeedMult;
            manager.bodyCollider.size *= new Vector2(1f, manager.crouchColliderHeightMult);
            manager.bodyCollider.offset = new Vector2(manager.bodyCollider.offset.x, manager.bodyCollider.offset.y - (manager.bodyCollider.size.y * manager.crouchColliderHeightMult));
        }
        
        manager.isCrouching = true;

    }

    public void CrouchStop() {
        if (manager.isCrouching) {
            manager.currentMaxMoveSpeed *= (1f/manager.crouchMoveSpeedMult);
            manager.bodyCollider.offset = new Vector2(manager.bodyCollider.offset.x, manager.bodyCollider.offset.y + (manager.bodyCollider.size.y * manager.crouchColliderHeightMult));
            manager.bodyCollider.size *= new Vector2(1f, 1f/manager.crouchColliderHeightMult);
        }

        manager.isCrouching = false;
    }
}
