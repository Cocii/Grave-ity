using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAI : MonoBehaviour
{
    public float detectionMaxRange = 8f;
    public float detectionMaxDot = 0.5f;
    public float detectionMinRange = 0.5f;
    public AIManager manager;
    public Vector2 moveInput;

    public float moveChangeSpeed;
    public float moveForceToApply;

    public float gettingUpCheckDistance = 3f;

    void Start()
    {
        if (manager == null)
            manager = GetComponent<AIManager>();

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
        moveInput = Vector2.zero;

        Vector3 playerPosition = PlayerManager.instance.transform.position;
        float distance = Vector3.Distance(transform.position, playerPosition);
        print(distance);
        if (distance <= detectionMaxRange && distance>detectionMinRange) {
            Vector3 playerDirection = (playerPosition - transform.position);
            float dot = Vector3.Dot(transform.up, playerDirection.normalized);
            if (Mathf.Abs(dot) < detectionMaxDot) {
                //print("player in range: " + (playerPosition - transform.position));
                moveInput = new Vector2(Mathf.Sign(playerDirection.x), 0f);
                moveForceToApply = Mathf.MoveTowards(moveForceToApply, manager.moveForceMagnitude, moveChangeSpeed);

            }
        }
        else {
            moveForceToApply = 0f;
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
            //print(timeMult);
            force = force * 100f * timeMult;

            //manager.characterController.SetMoveForce(force);
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
}
