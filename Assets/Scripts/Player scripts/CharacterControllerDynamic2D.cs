using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDynamic2D : MonoBehaviour
{
    private PlayerManager manager;

    [Header("Info")]
    public Vector2 gravityForce;
    public Vector2 moveForce;
    public Vector2 jumpForce;
    public Vector2 externalForce;
    
    [Header("Ground check settings")]
    public LayerMask groundLayer;
    public Vector2 rayCastOrigin;
    public float rayCastDistance;
    public Vector2 capsuleCastOrigin;
    public Vector2 capsuleCastSize;
    public float maxSlopeAngle;

    [Header("Ground info")]
    public Vector2 groundNormal;
    public Vector2 groundNormalPerpendicular;
    public float groundNormalDot;
    public float groundAngle;
    public Vector2 hitPoint;


    private void FixedUpdate() {
        if (manager == null)
            return;

        ApplyMoveForce();
        ApplyJumpForce();
    }

    void Update() {
        manager = PlayerManager.instance;

        if (manager == null)
            return;

        gravityForce = manager.currentGravity;
        RotateTowardsGravity();
        CheckSpriteFlip();
        CheckRotationLock();

        GroundCheck();
        SlopeAdjustement();
    }

    private void OnDrawGizmos() {
        Vector2 rayOriginAdjst = transform.position + new Vector3(rayCastOrigin.x, rayCastOrigin.y) * transform.up.y;
        Vector2 capsuleOriginAdjst = transform.position + new Vector3(capsuleCastOrigin.x, capsuleCastOrigin.y) * transform.up.y;

        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(capsuleOriginAdjst, capsuleCastSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(capsuleOriginAdjst, 0.015f);

        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(rayOriginAdjst, 0.015f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.01f);

        Gizmos.color = Color.white;
    }

    private void GroundCheck() {
        manager.wasGrounded = manager.isGrounded;
        PlayerManager.instance.isGrounded = false;

        //TO DO: mettere in start
        rayCastOrigin = new Vector2(0f, (-manager.bodyCollider.size.y / 2) + manager.bodyCollider.offset.y);
        capsuleCastOrigin.y = rayCastOrigin.y  + (-capsuleCastSize.y / 2);
        float lateralOffset = 0f;
        capsuleCastOrigin.x = manager.facingRight ? -lateralOffset : lateralOffset;
        // 

        Vector2 rayOriginAdjst = transform.position + new Vector3(rayCastOrigin.x, rayCastOrigin.y) * transform.up.y;
        Vector2 capsuleOriginAdjst = transform.position + new Vector3(capsuleCastOrigin.x, capsuleCastOrigin.y) * transform.up.y;

        RaycastHit2D hit;

        hit = Physics2D.Raycast(rayOriginAdjst, -transform.up.normalized, rayCastDistance, groundLayer);
        Debug.DrawRay(rayOriginAdjst, -transform.up.normalized * rayCastDistance, Color.green);
        if (hit) {
            //print("Ray hit: " + hit.collider);
            GroundHit(hit);
        }
        else {
            hit = Physics2D.CapsuleCast(capsuleOriginAdjst, capsuleCastSize, CapsuleDirection2D.Horizontal, 0f, Vector2.right, 0, groundLayer);
            if (hit) {
                //print("Capsule hit");
                GroundHit(hit);
            }
            else {
                GroundNotHit();
            }
        }
        
    }

    private void GroundHit(RaycastHit2D hit) {
        manager.isGrounded = true;

        if (manager.wasGrounded == false) {
            PlayerLand();
        }

        //print("Ground set");
        groundNormal = hit.normal.normalized;
        groundNormalPerpendicular = Vector2.Perpendicular(groundNormal).normalized;
        groundNormalDot = Vector2.Dot(transform.up.normalized, groundNormal);
        groundAngle = Vector2.Angle(groundNormal, transform.up.normalized);

        hitPoint = Physics2D.ClosestPoint(transform.position, hit.collider);
        Debug.DrawRay(hitPoint, groundNormal * 0.5f, Color.blue);
        Debug.DrawRay(hitPoint, groundNormalPerpendicular * 0.5f, Color.red);
        
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

    private void FlipFacingAndSprite() {
        manager.facingRight = !manager.facingRight;
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
            if ((manager.facingRight && currentScale.x < 0) || (!manager.facingRight && currentScale.x > 0)) {
                FlipSprite();
                //print("Adjusted flip");
            }
        }
        else {
            if ((manager.facingRight && currentScale.x > 0) || (!manager.facingRight && currentScale.x < 0)) {
                FlipSprite();
                //print("Adjusted flip");
            }
        }
    }

    private void CheckSpriteFlip() {
        //Vector2 checkVector = manager.body.velocity;
        Vector2 checkVector = manager.input.GetMoveInput;
        if(checkVector==Vector2.zero)
            checkVector = manager.body.velocity;

        if (gravityForce.normalized == Vector2.down) {
            if ((checkVector.x > 0.01f) && !manager.facingRight) {
                FlipFacingAndSprite();
            }
            else if ((checkVector.x < -0.01f) && manager.facingRight) {
                FlipFacingAndSprite();
            }
        }
        else if (gravityForce.normalized == Vector2.up) {
            if ((checkVector.x > 0.01f) && !manager.facingRight) {
                FlipFacingAndSprite();
            }
            else if ((checkVector.x < -0.01f) && manager.facingRight) {
                FlipFacingAndSprite();
            }


        }

    }

    private void RotateTowardsGravity() {
        float targetAngle = 0f;
        bool facingRight = PlayerManager.instance.facingRight;

        targetAngle = GravityManager.instance.physicsGravityTransform.eulerAngles.z - 180f;
        
        if(transform.eulerAngles.z == Mathf.Abs(targetAngle)) {
            if (manager.isRotating) {
                manager.wasRotating = true;
                manager.isRotating = false;
            }
            return;
        }
        else {
            //print("Rotating player");
        }

        PlayerRotate(targetAngle);
        AdjustFlipToGravity();

        if (!manager.isRotating) {
            //print("Disabling input");
            manager.isRotating = true;
            manager.input.DisableInput();
        }
    }

    private void PlayerRotate(float targetAngle) {
        float smoothedAngle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, PlayerManager.instance.rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(smoothedAngle - Mathf.RoundToInt(smoothedAngle)) < 0.05f) {
            smoothedAngle = Mathf.RoundToInt(smoothedAngle);
        }

        //if (Mathf.Abs(smoothedAngle - targetAngle) < 1f) {
        //    smoothedAngle = targetAngle;
        //}

        transform.eulerAngles = new Vector3(0f, 0f, smoothedAngle);

    }

    private void CheckRotationLock() {
        if (manager.wasRotating && manager.isGrounded) {
            //print("Enabling input" + " - " + manager.wasRotating);
            manager.wasRotating = false;
            manager.input.EnableInput();
        }

    }

    private void ApplyMoveForce() {
        if (moveForce.magnitude == 0)
            return;

        if (Mathf.Sign(moveForce.x) != Mathf.Sign(manager.body.velocity.x) && manager.body.velocity.x != 0f) {
            //print("Velocity turn");
            manager.body.velocity *= new Vector2(0f, 1f);
        }

        print("Moving force: " + moveForce);

        PlayerManager.instance.body.AddForce(moveForce);
        manager.body.velocity = Vector2.ClampMagnitude(manager.body.velocity, manager.maxMoveSpeed);
        //manager.body.velocity.Set(Mathf.Round(manager.body.velocity.x), Mathf.Round(manager.body.velocity.y));

        moveForce = Vector2.zero;

    }

    private void SlopeAdjustement() {
        if (groundAngle == 0f) {
            AdjustSlopeMaterial(true);
            return;
        }

        //high slope 
        if (manager.input.GetMoveInput.magnitude > 0f && Mathf.Approximately(Mathf.Floor(manager.body.velocity.magnitude), 0f)) {
            if(!manager.isGrounded || groundAngle >= maxSlopeAngle) {
                
                Vector2 position = transform.position;
                Vector2 hitPointDirection = hitPoint - position;
                float dotMoveHitPoint = Vector2.Dot(moveForce.normalized, hitPointDirection.normalized);

                if (dotMoveHitPoint >= 0) {
                    print("Slope too high, current anfle: " + groundAngle + " - " + dotMoveHitPoint);
                    moveForce = Vector2.zero;
                    AdjustSlopeMaterial(true);
                    return;
                }
            }           
        }

        if(groundAngle >= maxSlopeAngle) {
            AdjustSlopeMaterial(true);
        }
        else {
            AdjustSlopeMaterial(false);
        }

        moveForce = new Vector2(moveForce.magnitude, moveForce.magnitude) * groundNormalPerpendicular * -moveForce.normalized.x * Mathf.Sign(-gravityForce.y);
            
        

    }

    private void AdjustSlopeMaterial(bool forceDefault) {
        if (forceDefault) {
            if (manager.body.sharedMaterial == manager.fullFrictionMaterial) {
                manager.body.sharedMaterial = manager.defaultPhysicsMaterial;
            }
            return;
        }

        if (moveForce.magnitude == 0f && externalForce.magnitude == 0f) {
            if (manager.body.sharedMaterial == manager.defaultPhysicsMaterial) {
                manager.body.sharedMaterial = manager.fullFrictionMaterial;
            }
        }
        else {
            if (manager.body.sharedMaterial == manager.fullFrictionMaterial) {
                manager.body.sharedMaterial = manager.defaultPhysicsMaterial;
            }
        }
    }

    private void ApplyJumpForce() {
        PlayerManager.instance.body.AddForce(jumpForce);
        jumpForce = Vector2.zero;
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
}
