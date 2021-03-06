using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerAI : MonoBehaviour
{
    ManagerAI manager;

    [Header("Forces info")]
    public Vector2 gravityForce;
    public Vector2 moveForce;
    public Vector2 jumpForce;
    public Vector2 boostForce;
    public Vector2 dashForce;

    [Header("Ground types")]
    public const string grassSurfaceTag = "GrassSurface";
    public const string glassSurfaceTag = "GlassSurface";
    public const string metalSurfaceTag = "MetalSurface";

    [Header("Wallback check info")]
    public Vector2 wallbackOrigin;
    public float wallbackRaycastDistance;
    public Vector2 wallbackBoxcastSize;

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
        if (manager == null)
            manager = GetComponent<ManagerAI>();

        rayCastOrigin = new Vector2(0f, (-(manager.bodyCollider.size.y / 2) + manager.bodyCollider.offset.y) * transform.localScale.y);
        capsuleCastOrigin.y = rayCastOrigin.y + (-(manager.groundCapsuleCastSize.y / 2) * transform.localScale.y);

    }

    private void FixedUpdate() {
        ApplyMoveForce();
        ApplyJumpForce();
        ApplyBoostForce();
        ApplyDashForce();
    }

    void Update() {
        //gravityForce = manager.currentGravity;

        if (manager.isRotating)
            RotateTowardsGravity();
        
        GroundCheck();

        if(manager.isGrounded)
            SlopeCheck();

        ObstaclesCheck();

        CheckSpriteFlip();

        VelocityLimit();
    }

    public void UpdateGravityForce() {
        gravityForce = manager.currentGravity;
    }

    public void StartRotation() {
        if (manager == null)
            return;

        manager.isRotating = true;
        manager.currentRotationSpeed = CalculateRotationSpeed();
        RotateTowardsGravity();
    }


    private void ObstaclesCheck() {
        manager.isFacingObstacle = false;
        Vector2 direction = manager.isFacingRight ? Vector2.right : Vector2.left;
        Vector3 sizeAdjs = !manager.isCrouching ? manager.scaledColliderSize : manager.scaledCrouchedColliderSize;
        Vector3 offset = !manager.isCrouching ? manager.obstacleCheckOffset : manager.crouchedObstacleCheckOffset;

        if (!manager.isGrounded) {
            offset.y *= 0;
            sizeAdjs.y *= 0.975f;
        }
        else {
            offset.y *= Mathf.Sign(transform.up.y);
            sizeAdjs.y *= 0.7f;
        }
        offset.x *= direction.x;
        sizeAdjs.x *= 0.1f;

        Vector3 posAdjs = transform.position + offset;

        //Debug.DrawRay(posAdjs, direction * 20f, Color.yellow);

        RaycastHit2D hit;
        //hit = Physics2D.Raycast(transform.position, direction, manager.obstacleCheckDistance, manager.obstaclesLayer);
        hit = Physics2D.CapsuleCast(posAdjs, sizeAdjs, CapsuleDirection2D.Vertical, 0f, Vector2.right, 0f, manager.obstaclesLayer);
        if (hit) {
            manager.isFacingObstacle = true;
            //print("obstacle hit");
        }
    }

    private void VelocityLimit() {
        if (Mathf.Abs(manager.body.velocity.x) < 0.01f)
            manager.body.velocity *= new Vector2(0f, 1f);

        if (!manager.isDashing)
            manager.body.velocity = new Vector2(Mathf.Clamp(manager.body.velocity.x, -manager.currentMaxMoveSpeed, manager.currentMaxMoveSpeed), manager.body.velocity.y);
    }


    private void GroundCheck() {
        manager.wasGrounded = manager.isGrounded;
        manager.isGrounded = false;


        float offsetAdjst = manager.isFacingRight ? manager.lateralOffset : -manager.lateralOffset;
        capsuleCastOrigin.x = offsetAdjst;
        rayCastOrigin.x = offsetAdjst;

        Vector2 rayOriginAdjst = transform.position + new Vector3(rayCastOrigin.x, rayCastOrigin.y) * transform.up.y;
        Vector2 capsuleOriginAdjst = transform.position + new Vector3(capsuleCastOrigin.x, capsuleCastOrigin.y) * transform.up.y;

        RaycastHit2D hit;
        hit = Physics2D.Raycast(rayOriginAdjst, -transform.up.normalized, manager.groundRayCastDistance, manager.groundLayer);
        //Debug.DrawRay(rayOriginAdjst, -transform.up.normalized * groundRayCastDistance, Color.green);
        if (hit) {
            GroundHit(hit);
            Debug.DrawRay(hitPoint, groundNormal * 0.75f, Color.red);
            Debug.DrawRay(hitPoint, groundNormalPerpendicular * 0.75f, Color.red);
        }
        else {
            hit = Physics2D.CapsuleCast(capsuleOriginAdjst, manager.groundCapsuleCastSize, CapsuleDirection2D.Horizontal, 0f, Vector2.right, 0, manager.groundLayer);
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
            Land();
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
                //print("Ground tag not recognized");
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

    private void Land() {
        
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
        Vector2 checkVector = manager.movement.moveInput;
        if (checkVector == Vector2.zero)
            checkVector = manager.body.velocity;

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

        if (!manager.isRotating) {
            manager.isRotating = true;
            manager.currentRotationSpeed = CalculateRotationSpeed();
        }

        Rotate(targetAngle);

        AdjustFlipToGravity();
    }

    private void Rotate(float targetAngle) {
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, manager.currentRotationSpeed * Time.deltaTime);

        float absDiff = Mathf.Abs(transform.eulerAngles.z - targetAngle);
        if (absDiff < 10f || absDiff > 350f) {
            transform.eulerAngles = new Vector3(0f, 0f, targetAngle);
            return;
        }
    }

    private float CalculateRotationSpeed() {

        RaycastHit2D hit;
        float distance = manager.distanceRotationMin;
        float speed = manager.minRotationSpeed;
        hit = Physics2D.Raycast(transform.position, gravityForce.normalized, manager.distanceRotationMin, manager.groundLayer);

        if (hit) {
            distance = hit.distance;
            speed = Utilities.Map(distance, manager.distanceRotationMin, manager.distanceRotationMax, manager.minRotationSpeed, manager.maxRotationSpeed);
        }

        //print("Speed is " + speed + " for distance " + distance);
        return speed;
    }

    private void SlopeCheck() {
        manager.wasOnHighSlope = manager.isOnHighSlope;
        manager.isOnHighSlope = false;

        if (groundAngle == 0 || !manager.isGrounded) {
            return;
        }

        if (groundAngle >= manager.maxSlopeAngle) {
            //print("Check max slope");
            manager.isOnHighSlope = true;
        }

    }

    private void SlopeMoveAdjustement() {
        if (groundAngle == 0f) {
            return;
        }

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

            float mult = manager.moveForceMultOnSlopes;

            if (Vector2.Dot(Vector2.up, moveForce.normalized) > 0 && groundAngle > 20) {
                mult += 0.3f;
            }

            moveForce *= mult;
        }

    }

    //----------------------------------------------------------

    private void OnDrawGizmos() {
        if (!manager)
            return;


        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, manager.movement.detectionMaxRange);
        


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

        if (manager.isFacingObstacle && !manager.isCrouching) {
            print("Stopping cause obstacle");
            moveForce *= new Vector2(0, 1f);
            manager.body.velocity *= new Vector2(0f, 1f);
            //print("Movement blocked cause obstacle");
        }

        manager.body.AddForce(moveForce);

        moveForce = Vector2.zero;
    }

    private void ApplyJumpForce() {
        if (jumpForce.magnitude == 0)
            return;

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
