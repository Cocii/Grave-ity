using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    PlayerManager manager;

    [Header("Grab info")]
    public GameObject grabbedObj;
    public float grabPositioningDistance;
    public float grabCheckDistance;
    public Vector2 grabbedObjPlayerSide;
    public float grabbedHeight;

    [Header("Grab settings")]
    public LayerMask propsLayer;
    public LayerMask propsGround;
    public float grabMassMult = 0.1f;
    public float grabReleaseDistanceOffset = 0.001f;
    public float grabReleaseReactionForce = Mathf.Infinity;
    public float defaultBreakForce = Mathf.Infinity;
    public float minimalBreakForce = 1000f;
    public float grabDistanceMult = 1f;

    [Header("Dash info")]
    public float lastDashTime = 0f;

    [Header("Dash settings")]
    public float dashDistanceMult = 1.75f;

    [Header("General info")]
    public float propCheckDistance = 2f;

    private void Start() {
        manager = PlayerManager.instance;

        grabCheckDistance = manager.bodyCollider.size.x * transform.localScale.x * grabDistanceMult;
        propCheckDistance = manager.bodyCollider.size.x * transform.localScale.x * dashDistanceMult;
    }

    private void Update() {
        CheckGrabbedObj();
        CheckDash();
    }

    //CHECKS-----------------------------

    private void CheckGrabbedObj() {
        if (grabbedObj == null) {

            Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
            float distance = grabCheckDistance;

            if (manager.characterController.groundAngle != 0f) {
                direction = new Vector2(1, 1) * -manager.characterController.groundNormalPerpendicular * direction.x * Mathf.Sign(transform.right.x);
                //direction = (new Vector2(direction.x, 1) * (manager.characterController.groundNormalPerpendicular) * Mathf.Sign(-manager.currentGravity.y)).normalized;
                distance *= 1.75f;
            }

            Debug.DrawRay(transform.position, direction.normalized * distance, Color.red);




            return;
        }
            
        Debug.DrawLine(transform.position, grabbedObj.transform.position, Color.red);
        DistanceJoint2D grabbedObjJoint = grabbedObj.GetComponent<DistanceJoint2D>();

        if (grabbedObjJoint == null) {
            ReleaseGrab();
            return;
        }


        //float distance = Vector3.Distance(transform.position, grabbedObj.transform.position);
        //float distanceOffset = Mathf.Abs(distance - grabPositioningDistance);
        //if (distanceOffset < 0.001) {
        //    distanceOffset = 0f;
        //}
        //print(distanceOffset);


        Vector2 directionToPlayer = (transform.position - grabbedObj.transform.position);
        Vector2 scale = grabbedObj.transform.lossyScale * 1.5f;
        //print("scale:" + scale);
        Debug.DrawRay(grabbedObj.transform.position, manager.currentGravity.normalized * scale.y, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(grabbedObj.transform.position, manager.currentGravity.normalized, scale.y, propsGround);
        if (hit) {
            
            return;
        }
        else {
            print("Center not grounded, keep checking");
            Vector3 pointTowardsPlayer = grabbedObj.transform.position + new Vector3(grabbedObjJoint.distance * 0.25f, 0f, 0f)*Mathf.Sign(directionToPlayer.x);

            hit = Physics2D.Raycast(pointTowardsPlayer, manager.currentGravity.normalized, scale.y, propsGround);
            if (hit) {
                if (grabbedObjJoint.breakForce < defaultBreakForce)
                    grabbedObjJoint.breakForce = defaultBreakForce;

                return;
            }
            else {
                print("Grabbed not grounded on player side");
                //TODO: mettere break force al minimo che regga la vent
                grabbedObjJoint.breakForce = minimalBreakForce;
                //ReleaseGrab();
            }
        }

        //--------------------------------------

        

        //float grabReactionForce = grabbedObjJoint.reactionForce.magnitude;

        //print("offset: " + distanceOffset);

        //if (distanceOffset > grabReleaseDistanceOffset) {
        //    print("RELEASE FORZATO | offset: " + distanceOffset);
        //    ReleaseGrab();
        //}




    }

    private void CheckDash() {
        if (!manager.isDashing)
            return;

        if (CheckPropInFront())
            StopDash(0.05f);
    }

    private bool CheckPropInFront() {
        RaycastHit2D hit;
        Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
        hit = Physics2D.Raycast(transform.position, direction, propCheckDistance, propsLayer);
        if (hit) {
            return true;
        }
        return false;
    }

    //GRAB-----------------------------

    public void Grab() {
        RaycastHit2D hit;
        Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
        float distance = grabCheckDistance;

        if(manager.characterController.groundAngle != 0f) {
            direction = new Vector2(1, 1) * -manager.characterController.groundNormalPerpendicular * direction.x * Mathf.Sign(transform.right.x);
            distance *= 1.75f;
        }

        hit = Physics2D.Raycast(transform.position, direction.normalized, distance, propsLayer);
        if (hit) {
            //print("Grab hit: " + hit.collider);

            //grabPositioningDistance = (float)(manager.bodyCollider.size.x * 0.75 * transform.localScale.x) + (hit.transform.localScale.x * 0.5f);
            grabPositioningDistance = Vector3.Distance(transform.position, hit.transform.position);
            

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
        }

        grabbedObjJoint.breakForce = defaultBreakForce;
        grabbedObjJoint.connectedBody = manager.body;
        grabbedObjJoint.distance = grabPositioningDistance;

        grabbedObj.GetComponent<Rigidbody2D>().mass = grabbedObj.GetComponent<Rigidbody2D>().mass * grabMassMult;
        manager.isGrabbing = true;
        grabbedObjPlayerSide = grabbedObj.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
        //print("lossy scale: " + grabbedObj.transform.lossyScale);
        grabbedHeight = grabbedObj.transform.lossyScale.y;
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
        grabbedHeight = 0f;
    }

    //DASH-----------------------------

    private bool CanDash() {
        if (manager.isDashing || (Time.time - lastDashTime) <= manager.dashCooldown || CheckPropInFront() || manager.input.GetMoveInput.magnitude==0f)
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
