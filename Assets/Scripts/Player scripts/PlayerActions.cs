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
    public Rigidbody2D grabbedBody;
    public Vector2 moveForce;

    [Header("Grab settings")]
    public LayerMask propsLayer;
    public LayerMask propsGround;
    public float grabMassMult = 0.2f;
    public float defaultBreakForce = Mathf.Infinity;
    public float minimalBreakForce = 500f;
    public float grabDistanceMult = 1.35f;
    public float grabDistanceMultOnSlope = 1.75f;

    [Header("Dash info")]
    public float lastDashTime = 0f;

    [Header("Dash settings")]
    public float dashDistanceMult = 1.85f;

    [Header("Prop check info")]
    public float propCheckDistance = 0f;
    public float proximityPropDistanceThreshold = 1f;

    [Header("Crouch check settings")]
    public float gettingUpCheckDistance = 2.85f;
    public bool triedToGetUp = false;

    private void Start() {
        manager = PlayerManager.instance;

        float sizeX = manager.bodyCollider.size.x * transform.localScale.x;
        grabCheckDistance = sizeX * grabDistanceMult;
        propCheckDistance = sizeX * dashDistanceMult;

        propCheckDistance = sizeX * 2f;
        proximityPropDistanceThreshold = sizeX * 0.525f;
}

    private void Update() {
        CheckGrabbedObj();
        CheckDash();

        CheckCrouchGetUp();
        
    }

    private void FixedUpdate() {
        if (grabbedBody && moveForce.magnitude != 0f) {
            print("adding force to grabbed obj");
            grabbedBody.AddForce(moveForce);
            moveForce = Vector2.zero;
        }
    }

    //CHECKS-----------------------------

    private void CheckGrabbedObj() {
        if (!manager.isGrabbing) {
            return;
        }
            
        //Debug.DrawLine(transform.position, grabbedObj.transform.position, Color.red);
        DistanceJoint2D grabbedObjJoint = grabbedObj.GetComponent<DistanceJoint2D>();

        if (grabbedObjJoint == null) {
            ReleaseGrab();
            return;
        }

        Vector2 directionToPlayer = (transform.position - grabbedObj.transform.position);
        Vector2 scale = grabbedObj.transform.lossyScale * 1.5f;
        Debug.DrawRay(grabbedObj.transform.position, manager.currentGravity.normalized * scale.y, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(grabbedObj.transform.position, manager.currentGravity.normalized, scale.y, propsGround);
        if (hit) {
            return;
        }
        else {
            print("Center not grounded, keep checking");
            Vector3 pointTowardsPlayer = grabbedObj.transform.position + new Vector3(grabbedObjJoint.distance * 0.35f, 0f, 0f)*Mathf.Sign(directionToPlayer.x);
            Debug.DrawRay(pointTowardsPlayer, manager.currentGravity.normalized * scale.y, Color.green);
            hit = Physics2D.Raycast(pointTowardsPlayer, manager.currentGravity.normalized, scale.y, propsGround);
            if (hit) {
                if (grabbedObjJoint.breakForce < defaultBreakForce)
                    grabbedObjJoint.breakForce = defaultBreakForce;
                return;
            }
            else {
                print("Grabbed not grounded on player side");
                grabbedObjJoint.breakForce = minimalBreakForce;
            }
        }

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
            distance *= grabDistanceMultOnSlope;
        }

        hit = Physics2D.Raycast(transform.position, direction.normalized, distance, propsLayer);
        if (hit) {
            //print("Grab hit: " + hit.collider);

            //grabPositioningDistance = (float)(manager.bodyCollider.size.x * 0.75 * transform.localScale.x) + (hit.transform.localScale.x * 0.5f);
            grabPositioningDistance = Vector3.Distance(transform.position, hit.transform.position);
            

            AttachToDistanceJoint(hit.transform.gameObject);
        }
    }

    private void AttachToDistanceJoint(GameObject toAttach) {
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

        //grabbedObj.GetComponent<Rigidbody2D>().mass = grabbedObj.GetComponent<Rigidbody2D>().mass * grabMassMult;
        
        manager.isGrabbing = true;
        grabbedObjPlayerSide = grabbedObj.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
        grabbedHeight = grabbedObj.transform.lossyScale.y;
        grabbedBody = grabbedObj.GetComponent<Rigidbody2D>();
    }

    public void ReleaseGrab() {
        if (grabbedObj == null)
            return;

        manager.isGrabbing = false;

        //grabbedObj.GetComponent<Rigidbody2D>().mass = grabbedObj.GetComponent<Rigidbody2D>().mass * (1/grabMassMult);

        DetachToDistanceJoint();

        grabbedBody = null;
        grabbedObj = null;
        grabbedHeight = 0f;
    }

    private void DetachToDistanceJoint() {
        DistanceJoint2D grabbedObjJoint = grabbedObj.GetComponent<DistanceJoint2D>();
        if (grabbedObjJoint != null) {
            grabbedObjJoint.connectedBody = grabbedObj.GetComponent<Rigidbody2D>();
        }
    }

    public void AddMoveForce(Vector2 force) {
        moveForce += force;
    }

    //DASH-----------------------------

    private bool CanDash() {
        if (manager.isDashing || (Time.time - lastDashTime) <= manager.dashCooldown || CheckPropInFront())
            return false;

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
        float orientation = manager.isFacingRight ? 1f : -1f;
        Vector2 force = manager.dashForceMult * manager.moveForceMagnitude * transform.right * orientation;
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
            manager.moveForceMagnitude *= manager.crouchMoveSpeedMult;
            //manager.bodyCollider.size *= new Vector2(1f, manager.crouchColliderHeightMult);
            //manager.bodyCollider.offset = new Vector2(manager.bodyCollider.offset.x, manager.bodyCollider.offset.y - (manager.bodyCollider.size.y * manager.crouchColliderHeightMult));
            manager.bodyCollider.size = manager.crouchColliderSize;
            manager.bodyCollider.offset = manager.crouchColliderOffset;
        
        }
        
        manager.isCrouching = true;

    }

    public void CrouchStop() {
        if (manager.isCrouching) {
            if (CheckCeilingCrouched()) {
                triedToGetUp = true;
                return;
            }


            manager.currentMaxMoveSpeed *= (1f/manager.crouchMoveSpeedMult);
            manager.moveForceMagnitude *= (1f / manager.crouchMoveSpeedMult);
            //manager.bodyCollider.offset = new Vector2(manager.bodyCollider.offset.x, manager.bodyCollider.offset.y + (manager.bodyCollider.size.y * manager.crouchColliderHeightMult));
            //manager.bodyCollider.size *= new Vector2(1f, 1f/manager.crouchColliderHeightMult);
            manager.bodyCollider.size = manager.defaultColliderSize;
            manager.bodyCollider.offset = manager.defaultColliderOffset;

        }

        manager.isCrouching = false;
    }

    private bool CheckCeilingCrouched() {
        if (Physics2D.Raycast(transform.position, transform.up, gettingUpCheckDistance, manager.obstaclesLayer)) {
            return true;
        }

        return false;
    }

    private void CheckCrouchGetUp() {
        //Debug.DrawRay(transform.position, transform.up * gettingUpCheckDistance, Color.cyan);

        if (!triedToGetUp)
            return;

        if(!CheckCeilingCrouched()) {
            triedToGetUp = false;
            CrouchStop();
        }
        
    }
}
