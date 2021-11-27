using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager manager;
    private Vector2 moveInput;
    private bool jumpInput;

    private void Start() {
        manager = PlayerManager.instance;
    }

    void Update() {
        moveInput = manager.input.GetMoveInput;
        jumpInput = manager.input.GetJumpDInput;

        HorizontalMovementUpdate();
        VerticalMovementUpdate();
    }

    private void HorizontalMovementUpdate() {
        Vector2 force = Vector2.zero;
        
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
    }

    private void VerticalMovementUpdate() {
        Vector2 force = Vector2.zero;

        Vector2 gravityForce = manager.currentGravity;
        float gravityRatio = manager.currentGravityRatio;

        //JUMP
        if (jumpInput && CanJump()) {
            force = -gravityForce.normalized * manager.jumpForceMagnitude;
            manager.characterController.SetJumpForce(force);
        }
        //

        //WALLJUMP
        if (jumpInput && CanWalljump(moveInput.x)) {
            force.Set(manager.wallJumpDirection.x * moveInput.x, manager.wallJumpDirection.y * -gravityForce.normalized.y);
            force *= manager.jumpForceMagnitude * manager.walljumpMult;
            manager.characterController.SetJumpForce(force);
        }
        //

        //JUMP BOOSTER
        jumpInput = manager.input.GetJumpInput;
        if (jumpInput && CanBoostJump(gravityForce)) {
            print("Boost jump");
            force = -gravityForce.normalized * manager.jumpForceMagnitude * manager.jumpBoostMult * Time.deltaTime;
            manager.characterController.AddBoostForce(force);
        }
        //

        //GRAVITY BOOSTER
        if (CanBoostgravity(gravityRatio, gravityForce)) {
            print("Boost gravity");
            force = gravityForce * manager.gravityBoostMult;
            //force *= (2f - gravityRatio);
            manager.characterController.AddBoostForce(force);
        }
        //
    }

    private bool CanJump() {
        return manager.isGrounded && !manager.isOnHighSlope && !manager.isGrabbing;
    }

    private bool CanWalljump(float xInput) {
        return !manager.isGrounded && manager.isBackOnWall && manager.canWalljump && xInput != 0f;
    }

    private bool CanBoostJump(Vector2 gravityForce) {
        bool can = false;
        if(Mathf.Sign(gravityForce.y) < 0f) {
            can = manager.body.velocity.y > 0.5f;
        }
        else {
            can = manager.body.velocity.y < -0.5f;
        }

        return can;
    }

    private bool CanBoostgravity(float gravityRatio, Vector2 gravityForce) {
        return !manager.isGrounded && gravityRatio <= 1 && Mathf.Sign(manager.body.velocity.y) == Mathf.Sign(gravityForce.y);
    }
}
