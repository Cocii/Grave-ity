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
        if (jumpInput && manager.isGrounded && !manager.isOnHighSlope) {
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
        if (jumpInput && manager.body.velocity.y > 0f) {
            force = -gravityForce.normalized * manager.jumpForceMagnitude * manager.jumpBoostMult * Time.deltaTime;
            manager.characterController.AddJumpForce(force);
        }
        //

    }

    public void Dash() {
        Vector2 force = manager.dashForceMult * manager.moveForceMagnitude * manager.input.GetMoveInput;
        manager.characterController.SetDashForce(force);
    }
}
