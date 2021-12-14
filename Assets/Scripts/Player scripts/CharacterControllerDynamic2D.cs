using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDynamic2D : MonoBehaviour
{
    private PlayerManager manager;

    [Header("Forces info")]
    public Vector2 gravityForce;
    public Vector2 moveForce;
    public Vector2 jumpForce;
    public Vector2 boostForce;
    public Vector2 dashForce;

    [Header("Ground check settings")]
    public LayerMask groundLayer;
    public Vector2 groundCapsuleCastSize;
    public float maxSlopeAngle;
    public float lateralOffset = 0f;
    public float groundRayCastDistance;

    [Header("Ground type settings")]
    public const string grassSurfaceTag = "GrassSurface";
    public const string glassSurfaceTag = "GlassSurface";
    public const string metalSurfaceTag = "MetalSurface";

    [Header("Wallback check info")]
    public Vector2 wallbackRaycastOrigin;
    public float wallbackRaycastDistance;

    [Header("Ground check info")]
    public Vector2 rayCastOrigin;
    public Vector2 capsuleCastOrigin;
    public float currentDistanceFromGround;

    [Header("Ground info")]
    public Vector2 groundNormal;
    public Vector2 groundNormalPerpendicular;
    public float groundNormalDot;
    public float groundAngle;
    public Vector2 hitPoint;

    private void Start() {
        manager = PlayerManager.instance;

        rayCastOrigin = new Vector2(0f, (-(manager.bodyCollider.size.y / 2) + manager.bodyCollider.offset.y) * transform.localScale.y);
        capsuleCastOrigin.y = rayCastOrigin.y + (-(groundCapsuleCastSize.y  / 2) * transform.localScale.y);
        
        wallbackRaycastDistance = manager.bodyCollider.size.x * transform.localScale.y * 1.1f;
    }

    private void FixedUpdate() {
        ApplyMoveForce();
        ApplyJumpForce();
        ApplyBoostForce();
        ApplyDashForce();
    }

    void Update() {
        gravityForce = manager.currentGravity;

        RotateTowardsGravity();

        CheckSpriteFlip();
        CheckRotationLock();

        GroundCheck();
        WallBackCheck();
        SlopeCheck();

        VelocityFix();
    }

    private void VelocityFix() {
        if (Mathf.Abs(manager.body.velocity.x) < 0.01f)
            manager.body.velocity *= new Vector2(0f, 1f);

        if (!manager.isDashing)
            manager.body.velocity = new Vector2(Mathf.Clamp(manager.body.velocity.x, -manager.currentMaxMoveSpeed, manager.currentMaxMoveSpeed), manager.body.velocity.y);
    }

    private void WallBackCheck() {
        manager.wasBackOnWall = manager.isBackOnWall;
        manager.isBackOnWall = false;

        Vector2 wallbackRayOriginAdjst = transform.position + new Vector3(wallbackRaycastOrigin.x, wallbackRaycastOrigin.y) * transform.up.y;
        Vector2 direction = manager.isFacingRight ? Vector2.left : Vector2.right;

        RaycastHit2D hit;
        hit = Physics2D.Raycast(wallbackRayOriginAdjst, direction, wallbackRaycastDistance, groundLayer);
        if (hit) {
            manager.isBackOnWall = true;

            if (!manager.wasBackOnWall && !manager.isGrounded) {
                manager.canWalljump = true;
            }
        }
        else { //TESTARE SE QUESTO ELSE NON SERVE
            if (manager.wasBackOnWall && !manager.isGrounded) {
                manager.canWalljump = true;
            }
        }
    }

    private void GroundCheck() {
        manager.wasGrounded = manager.isGrounded;
        manager.isGrounded = false;


        float offsetAdjst = manager.isFacingRight ? lateralOffset : -lateralOffset;
        capsuleCastOrigin.x = offsetAdjst;
        rayCastOrigin.x = offsetAdjst;

        Vector2 rayOriginAdjst = transform.position + new Vector3(rayCastOrigin.x, rayCastOrigin.y) * transform.up.y;
        Vector2 capsuleOriginAdjst = transform.position + new Vector3(capsuleCastOrigin.x, capsuleCastOrigin.y) * transform.up.y;

        RaycastHit2D hit;
        hit = Physics2D.Raycast(rayOriginAdjst, -transform.up.normalized, groundRayCastDistance, groundLayer);
        //Debug.DrawRay(rayOriginAdjst, -transform.up.normalized * groundRayCastDistance, Color.green);
        if (hit) {
            GroundHit(hit);
            Debug.DrawRay(hitPoint, groundNormal * 0.75f, Color.red);
            Debug.DrawRay(hitPoint, groundNormalPerpendicular * 0.75f, Color.red);
        }
        else {
            hit = Physics2D.CapsuleCast(capsuleOriginAdjst, groundCapsuleCastSize, CapsuleDirection2D.Horizontal, 0f, Vector2.right, 0, groundLayer);
            if (hit) {
                GroundHit(hit);
                Debug.DrawRay(hitPoint, groundNormal * 0.75f, Color.green);
                Debug.DrawRay(hitPoint, groundNormalPerpendicular * 0.75f, Color.green);
            }
            else {
                GroundNotHit();
            }
        }

        
    }

    private void GroundHit(RaycastHit2D hit) {
        manager.isGrounded = true;

        currentDistanceFromGround = hit.distance;

        if (manager.wasGrounded == false) {
            PlayerLand();
        }

        hitPoint = hit.point;
        groundNormal = hit.normal;
        groundNormalPerpendicular = Vector2.Perpendicular(groundNormal).normalized;
        groundNormalDot = Vector2.Dot(transform.up.normalized, groundNormal);
        groundAngle = Vector2.Angle(groundNormal, transform.up.normalized);
        
        manager.canWalljump = false;

        manager.groundType = DetectGroundType(hit.transform.gameObject.tag);

        //print(hit.collider + " - angolo: " + groundAngle);
    }

    private GroundTypeEnum DetectGroundType(string tag) {
        GroundTypeEnum detected;

        //print("detecting with: " + tag);
        switch (tag) {
            case grassSurfaceTag:
                //print("grass detected");
                detected = GroundTypeEnum.grass;
                break;

            case glassSurfaceTag:
                //print("glass detected");
                detected = GroundTypeEnum.glass;
                break;

            case metalSurfaceTag:
                //print("metal detected");
                detected = GroundTypeEnum.metal;
                break;

            default:
                //print("Ground tag not recognized, putting it to grass");
                detected = GroundTypeEnum.grass;
                break;
        }


        return detected;
    }

    private void GroundNotHit() {
        groundNormal = -gravityForce.normalized;
        groundNormalPerpendicular = Vector2.Perpendicular(groundNormal).normalized;
        groundNormalDot = Vector2.Dot(transform.up.normalized, groundNormal);
        groundAngle = 0f;
        hitPoint = transform.position;
    }

    private void PlayerLand() {
            
    }

    private void PlayerWalljump() {
        //print("Walljump with force: " + jumpForce);
        manager.canWalljump = false;
    }

    private void FlipFacingAndSprite() {
        manager.isFacingRight = !manager.isFacingRight;
        FlipSprite();
    }

    private void FlipSprite() {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1f;
        transform.localScale = currentScale;
    }

    private void AdjustFlipToGravity() {
        bool upside = gravityForce.normalized == Vector2.up ? true : false;
        Vector3 currentScale = transform.localScale;

        if (!upside) {
            if ((manager.isFacingRight && currentScale.x < 0) || (!manager.isFacingRight && currentScale.x > 0)) {
                FlipSprite();
            }
        }
        else {
            if ((manager.isFacingRight && currentScale.x > 0) || (!manager.isFacingRight && currentScale.x < 0)) {
                FlipSprite();
            }
        }
    }

    private void CheckSpriteFlip() {
        Vector2 checkVector = manager.input.GetMoveInput;
        if(checkVector==Vector2.zero)
            checkVector = manager.body.velocity;

        if (manager.isGrabbing) {
            Vector2 grabDir = manager.actions.grabbedObjPlayerSide;
            if((manager.isFacingRight && grabDir==Vector2.right) || (!manager.isFacingRight && grabDir == Vector2.left)) {
                return;
            }
            else {
                FlipFacingAndSprite();
                return;
            }
        }

        if (gravityForce.normalized == Vector2.down) {
            if ((checkVector.x > 0.01f) && !manager.isFacingRight) {
                FlipFacingAndSprite();
            }
            else if ((checkVector.x < -0.01f) && manager.isFacingRight) {
                FlipFacingAndSprite();
            }
        }
        else if (gravityForce.normalized == Vector2.up) {
            if ((checkVector.x > 0.01f) && !manager.isFacingRight) {
                FlipFacingAndSprite();
            }
            else if ((checkVector.x < -0.01f) && manager.isFacingRight) {
                FlipFacingAndSprite();
            }
        }
    }

    private void RotateTowardsGravity() {
        float targetAngle = 0f;
        if (gravityForce.normalized == Vector2.up)
            targetAngle = 180f;

        if (transform.eulerAngles.z == targetAngle) {
            if (manager.isRotating) {
                manager.wasRotating = true;
                manager.isRotating = false;
            }
            return;
        }

        //if (manager.isGrounded)
        //    return;

        PlayerRotate(targetAngle);

        AdjustFlipToGravity();

        if (!manager.isRotating) {
            manager.isRotating = true;
            manager.input.DisableInput();
        }
    }

    private void PlayerRotate(float targetAngle) {
        float currentRotationSpeed = CalculateRotationSpeed();

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, currentRotationSpeed * Time.deltaTime);

        //transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, new Vector3(0f, 0f, targetAngle), manager.rotationSpeed * Time.deltaTime);

        float absDiff = Mathf.Abs(transform.eulerAngles.z - targetAngle);
        if (absDiff < 10f || absDiff > 350f) {
            transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
            return;
        }

        //if(manager.isRotating && manager.isGrounded) {
        //    transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
        //    print("Rotation stuck bug fix applied");
        //}
    }

    private float CalculateRotationSpeed() {
        RaycastHit2D hit;
        float distance = manager.distanceRotationMin;
        float speed = manager.minRotationSpeed;
        hit = Physics2D.Raycast(transform.position, gravityForce.normalized, manager.distanceRotationMin, groundLayer);

        if (hit) {
            distance = hit.distance;
            speed = Utilities.Map(distance, manager.distanceRotationMin, manager.distanceRotationMax, manager.minRotationSpeed, manager.maxRotationSpeed);
        }

        //print("Speed is " + speed + " for distance " + distance);
        return speed;
    }

    private void CheckRotationLock() {
        if (manager.wasRotating && manager.isGrounded) {
            manager.wasRotating = false;
            manager.input.EnableInput();
        }
    }

    private void SlopeCheck() {
        manager.wasOnHighSlope = manager.isOnHighSlope;
        manager.isOnHighSlope = false;

        if (groundAngle == 0) {
            return;
        }

        if (groundAngle >= maxSlopeAngle) {
            print("Check max slope");
            manager.isOnHighSlope = true;
        }
        else if (manager.input.GetMoveInput.magnitude > 0f && Mathf.Approximately(Mathf.Floor(manager.body.velocity.magnitude), 0f) && !manager.isGrounded) {
            print("Check slope with vel");
            manager.isOnHighSlope = true;
        }
    }

    private void SlopeMoveAdjustement() {
        if (groundAngle == 0f) {
            return;
        }

        //moveForce = new Vector2(moveForce.magnitude, moveForce.magnitude) * -groundNormalPerpendicular * moveForce.normalized.x * Mathf.Sign(transform.right.x);
        //if (groundAngle != 0f) {
        //    moveForce *= manager.moveForceMultOnSlopes;
        //}
        //if (groundAngle > maxSlopeAngle) {
        //    moveForce = Vector2.zero;
        //}

        if (manager.isOnHighSlope) {
            Vector2 position = transform.position;
            Vector2 hitPointDirection = hitPoint - position;
            float dotMoveHitPoint = Vector2.Dot(moveForce.normalized, hitPointDirection.normalized);

            if (dotMoveHitPoint >= 0) {
                //print("Slope too high to walk: " + groundAngle);
                moveForce = Vector2.zero;
            }

        }
        else {
            //print("Adjusting move force to slope");
            moveForce = new Vector2(moveForce.magnitude, moveForce.magnitude) * -groundNormalPerpendicular * moveForce.normalized.x * Mathf.Sign(transform.right.x);

            moveForce *= manager.moveForceMultOnSlopes;
            
        }
  
    }


    //----------------------------------------------------------

    private void OnDrawGizmos() {
        Vector2 rayOriginAdjst = transform.position + new Vector3(rayCastOrigin.x, rayCastOrigin.y) * transform.up.y;
        Vector2 capsuleOriginAdjst = transform.position + new Vector3(capsuleCastOrigin.x, capsuleCastOrigin.y) * transform.up.y;
        Vector2 wallbackRayOriginAdjst = transform.position + new Vector3(wallbackRaycastOrigin.x, wallbackRaycastOrigin.y) * transform.up.y;
        Vector2 direction = Vector2.right;
        if (manager!=null)
            direction = manager.isFacingRight ? Vector2.left : Vector2.right;

        Gizmos.color = Color.cyan;
        Gizmos.DrawCube(capsuleOriginAdjst, groundCapsuleCastSize);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(rayOriginAdjst, -transform.up.normalized * groundRayCastDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(hitPoint, .15f);


        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(wallbackRayOriginAdjst, direction * wallbackRaycastDistance);

        Gizmos.color = Color.white;
    }

    //----------------------------------------------------------

    private void ApplyMoveForce() {
        if (moveForce.magnitude == 0)
            return;

        if (Mathf.Sign(moveForce.x) != Mathf.Sign(manager.body.velocity.x) && manager.body.velocity.x != 0f && manager.isGrounded) {
            manager.body.velocity *= new Vector2(0f, 1f);
        }

        SlopeMoveAdjustement();

        //print("Moving force and magnitude: " + moveForce + " - " + moveForce.magnitude + " - angle: " + groundAngle);

        Debug.DrawRay(transform.position, moveForce * 0.005f, Color.green);

        manager.body.AddForce(moveForce);
        
        moveForce = Vector2.zero;
    }

    private void ApplyJumpForce() {
        if (jumpForce.magnitude == 0)
            return;

        if(jumpForce.x != 0f) {
            PlayerWalljump();
        }

        manager.body.AddForce(jumpForce);
        //print("Jump with force: " + jumpForce);
        jumpForce = Vector2.zero;
    }

    private void ApplyBoostForce() {
        if (boostForce.magnitude == 0)
            return;

        manager.body.AddForce(boostForce);
        //print("Boost with force: " + boostForce);
        boostForce = Vector2.zero;
    }

    private void ApplyDashForce() {
        if (dashForce.magnitude == 0)
            return;

        manager.body.AddForce(dashForce, ForceMode2D.Impulse);
        dashForce = Vector2.zero;
    }

    public void SetMoveForce(Vector2 force) {
        moveForce = force;
    }

    public void SetJumpForce(Vector2 force) {
        jumpForce = force;
    }

    public void AddMoveForce(Vector2 force) {
        moveForce += force;
    }

    public void AddJumpForce(Vector2 force) {
        jumpForce += force;
    }

    public void SetDashForce(Vector2 force) {
        dashForce = force;
    }

    public void AddBoostForce(Vector2 force) {
        boostForce += force;
    }
}
