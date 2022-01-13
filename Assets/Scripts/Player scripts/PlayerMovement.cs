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
                force = gravityForceNormal * moveInput * manager.moveForceMagnitude * manager.moveAirForceMult;
            }

            if (gravityForce.normalized == Vector2.down) {
                force *= -1f;
            }

            
            float timeMult = Time.smoothDeltaTime;
            force = force * 100f * timeMult;

            manager.characterController.AddMoveForce(force);
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

            if (manager.isCrouching) {
                manager.actions.CrouchStop();
            }
        }
        //

        //WALLJUMP
        if (jumpInput && CanWalljump(moveInput.x)) {
            force.Set(manager.wallJumpDirection.x * moveInput.x, manager.wallJumpDirection.y * -gravityForce.normalized.y);
            force *= manager.jumpForceMagnitude * manager.walljumpForceMult;
            manager.characterController.SetJumpForce(force);
        }
        //

        //JUMP BOOSTER
        jumpInput = manager.input.GetJumpInput;
        if (jumpInput && CanBoostJump(gravityForce)) {
            //print("Boost jump");
            force = -gravityForce.normalized * manager.jumpForceMagnitude * manager.currentJumpBoostMult;

            float timeMult = Time.smoothDeltaTime;

            force *= timeMult;
            manager.characterController.AddBoostForce(force);
        }
        //

        //GRAVITY BOOSTER
        if (CanBoostgravity(gravityRatio, gravityForce)) {
            //print("Boost gravity");
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
            can = manager.body.velocity.y > manager.jumpBoostStopVelocityThreshold;
        }
        else {
            can = manager.body.velocity.y < -manager.jumpBoostStopVelocityThreshold;
        }

        return can;
    }

    private bool CanBoostgravity(float gravityRatio, Vector2 gravityForce) {
        bool can = !manager.isGrounded;
        can &= gravityRatio <= 1;
        can &= Mathf.Sign(manager.body.velocity.y) == Mathf.Sign(gravityForce.y);

        return can;
    }
}
