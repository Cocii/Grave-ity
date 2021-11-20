using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager manager;

    private void Start() {
        manager = PlayerManager.instance;
    }

    void Update() {
        MovementCheck();
    }

    private void MovementCheck() {
        Vector2 force = Vector2.zero;

        Vector2 moveInput = manager.input.GetMoveInput;
        bool jumpInput = manager.input.GetJumpDInput;

        Vector2 gravityForce = manager.currentGravity;
        Vector2 gravityForceNormal = manager.currentGravityNormal;
        float gravityRatio = manager.currentGravityRatio;

        //WASD
        if (moveInput.magnitude > 0.1f) {
            if (manager.isGrounded) {
                force = gravityForceNormal * moveInput * manager.moveForceMagnitude;
            }
            else {
                force = gravityForceNormal * moveInput * manager.moveAirForceMagnitude;
            }

            if (gravityForce.normalized == Vector2.down) {
                force *= -1f;
            }

            manager.characterController.SetMoveForce(force);
        }

        //JUMP
        if (jumpInput && CanJump()) {
            force = -gravityForce.normalized * manager.jumpForceMagnitude;
            manager.characterController.SetJumpForce(force);
        }
        //

        //WALLJUMP
        if(jumpInput && !manager.isGrounded && manager.isBackOnWall && manager.canWalljump && moveInput.x!=0f) {
            force.Set(manager.wallJumpDirection.x * moveInput.x, manager.wallJumpDirection.y * -gravityForce.normalized.y);
            force *= manager.jumpForceMagnitude * manager.walljumpMult;
            manager.characterController.SetJumpForce(force);
        }
        //

        //JUMP BOOSTER
        jumpInput = manager.input.GetJumpInput;
        if (jumpInput && manager.body.velocity.y > 0.5f) {
            force = -gravityForce.normalized * manager.jumpForceMagnitude * manager.jumpBoostMult * Time.deltaTime;
            manager.characterController.AddBoostForce(force);
        }
        //

        //GRAVITY BOOSTER
        if(!manager.isGrounded && gravityRatio < 1) {
            //print("gravity booster added");
            force = gravityForce * manager.gravityBoostMult;
            manager.characterController.AddBoostForce(force);
        }
        //
    }

    public bool CanJump() {
        return manager.isGrounded && !manager.isOnHighSlope && !manager.isGrabbing;
    }
}
