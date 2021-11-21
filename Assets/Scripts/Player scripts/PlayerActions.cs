using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    PlayerManager manager;

    public GameObject grabbedObj;
    public float grabCheckDistance;
    public LayerMask propsLayer;
    public float grabPositioningDistance = 0.6f;
    public float grabMassMult = 0.1f;

    public float lastDashTime = 0f;

    private void Start() {
        manager = PlayerManager.instance;
    }

    private void Update() {
        checkGrabbedObj();
    }

    public void Grab() {
        RaycastHit2D hit;
        Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(transform.position, direction, grabCheckDistance, propsLayer);
        if (hit) {
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

    private void checkGrabbedObj() {
        if (grabbedObj == null)
            return;

        float distance = Vector3.Distance(transform.position, grabbedObj.transform.position);
        if (distance > grabPositioningDistance) {
            print(distance);
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

    private bool CanDash() {
        if (manager.isDashing || (Time.time - lastDashTime) <= manager.dashCooldown)
            return false;

        RaycastHit2D hit;
        Vector2 direction = manager.input.GetMoveInput;
        hit = Physics2D.Raycast(transform.position, direction, manager.dashCheckDistance, propsLayer);

        return hit.transform == null;
    }

    public void Dash() {
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

        manager.isDashing = false;
        manager.body.velocity *= new Vector2(0.1f, 1f);
        yield break;
    }
}
