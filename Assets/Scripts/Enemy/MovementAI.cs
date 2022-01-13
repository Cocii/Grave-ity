using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAI : MonoBehaviour
{
    public float detectionMaxRange = 8f;
    public float detectionMaxDot = 0.5f;
    public float detectionMinRange = 0.5f;
    public ManagerAI manager;
    public Vector2 moveInput;

    public float moveChangeSpeed = 5f;
    public float moveForceToApply;

    public float gettingUpCheckDistance = 3f;

    public float angleLimit = 40f;
    

    void Start()
    {
        if (manager == null)
            manager = GetComponent<ManagerAI>();

        switch (manager.initialPose) {
            case InitialPoseEnum.crouched:
                Crouch();
                break;
            default:
                break;
        }
        
    }

    void Update()
    {
        DetectPlayer();

        if (!manager.playerDetected)
            return;

        HorizontalMovementUpdate();

        StatusCheck();
    }

    private void StatusCheck() {
        if (manager.isCrouching) {
            if (Physics2D.Raycast(transform.position, transform.up, gettingUpCheckDistance, manager.obstaclesLayer)) {
                //c'è un ostacolo sopra
            }
            else {
                CrouchStop();
            }
        }
    }

    private void DetectPlayer() {
        if (!PlayerManager.instance)
            return;

        manager.playerDetected = false;
        moveInput = Vector2.zero;

        Vector3 playerPosition = PlayerManager.instance.transform.position;
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= detectionMaxRange) {
            if (distance > detectionMinRange) {
                Vector3 playerDirection = (playerPosition - transform.position);
                float dot = Vector3.Dot(transform.up, playerDirection.normalized);
                if (Mathf.Abs(dot) < detectionMaxDot) {
                    moveInput = new Vector2(Mathf.Sign(playerDirection.x), 0f);
                    moveForceToApply = Mathf.MoveTowards(moveForceToApply, manager.moveForceMagnitude, moveChangeSpeed);
                    manager.playerDetected = true;
                }
            }
            else {
                moveForceToApply = 0f;
            }

            HeadRotation();

        }
        else {
            moveForceToApply = 0f;
            ResetHeadRotation();
        }

    }

    private void HorizontalMovementUpdate() {
        Vector2 force = Vector2.zero;

        Vector2 gravityForce = manager.currentGravity;
        Vector2 gravityForceNormal = manager.currentGravityNormal;

        //WASD
        if (moveInput.magnitude > 0.1f) {
            if (manager.isGrounded) {
                force = gravityForceNormal * moveInput * moveForceToApply;
            }
            else {
                force = gravityForceNormal * moveInput * moveForceToApply * manager.moveAirForceMult;
            }

            if (gravityForce.normalized == Vector2.down) {
                force *= -1f;
            }


            float timeMult = Time.smoothDeltaTime;
            force = force * 100f * timeMult;

            manager.characterController.AddMoveForce(force);
        }
    }

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
            
            manager.currentMaxMoveSpeed *= (1f / manager.crouchMoveSpeedMult);
            manager.moveForceMagnitude *= (1f / manager.crouchMoveSpeedMult);
            //manager.bodyCollider.offset = new Vector2(manager.bodyCollider.offset.x, manager.bodyCollider.offset.y + (manager.bodyCollider.size.y * manager.crouchColliderHeightMult));
            //manager.bodyCollider.size *= new Vector2(1f, 1f/manager.crouchColliderHeightMult);
            manager.bodyCollider.size = manager.defaultColliderSize;
            manager.bodyCollider.offset = manager.defaultColliderOffset;

        }

        manager.isCrouching = false;
    }

    private void HeadRotation() {
        float angle = 0f;

        Vector3 targetPosVector = PlayerManager.instance.transform.position - transform.position;
        angle = Utilities.AngleOfVectorOnZAxis(targetPosVector);
        angle = Mathf.Round(angle);
        if (!manager.isFacingRight) {
            if (angle >= 0)
                angle -= 180f;
            else
                angle += 180f;

            angle *= -1f;
        }
            
        angle = Mathf.Clamp(angle, -angleLimit, angleLimit);

        if (manager.headBone) {
            
            manager.headBone.transform.localEulerAngles = new Vector3(0, 0, angle);
            
        }
    }

    private void ResetHeadRotation() {
        if (manager.headBone) {
            manager.headBone.transform.localEulerAngles = Vector3.zero;
            
        }
    }
}
