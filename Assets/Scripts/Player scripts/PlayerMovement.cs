using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager pManager;
    private Vector2 moveInput;
    private bool jumpBoost;
    public Vector2 gravityForce;
    public Vector2 gravityForceNormal;
    public float gravityRatio;

    private void Start() {
        pManager = PlayerManager.instance;

        GravityManager gManager = GravityManager.instance;
        gravityForce = gManager.physicsGravity;
        gravityForceNormal = gManager.physicsGravityNormal;
        gravityRatio = gManager.gravityRatio;
    }

    void Update() {
        moveInput = pManager.input.GetMoveInput;

        if (moveInput.magnitude > 0.1f)
            HorizontalMovementUpdate();
        
        if(!pManager.isGrounded)
            VerticalMovementUpdate();
    }

    public void UpdateGravityInfo() {
        gravityForce = pManager.currentGravity;
        gravityForceNormal = pManager.currentGravityNormal;
        gravityRatio = pManager.currentGravityRatio;
    }

    private void HorizontalMovementUpdate() {
        Vector2 force = Vector2.zero;

        if (pManager.isGrounded) {
            force = gravityForceNormal * moveInput * pManager.moveForceMagnitude;
        }
        else {
            force = gravityForceNormal * moveInput * pManager.moveForceMagnitude * pManager.moveAirForceMult;
        }

        if (gravityForce.normalized == Vector2.down) {
            force *= -1f;
        }

        force = force * 100f * Time.smoothDeltaTime;

        pManager.characterController.AddMoveForce(force);

        if (pManager.isGrabbing) {
            pManager.actions.AddMoveForce(force);
        }
    }

    private void VerticalMovementUpdate() {
        Vector2 force = Vector2.zero;

        //JUMP BOOSTER
        if (jumpBoost && CanBoostJump(gravityForce)) {
            //print("Boost jump");
            force = -gravityForce.normalized * pManager.jumpForceMagnitude * pManager.currentJumpBoostMult;

            float timeMult = Time.smoothDeltaTime;

            force *= timeMult;
            pManager.characterController.AddBoostForce(force);
        }
        //

        //GRAVITY BOOSTER
        if (CanBoostgravity(gravityRatio, gravityForce)) {
            //print("Boost gravity");
            force = gravityForce * pManager.gravityBoostMult;
            //force *= (2f - gravityRatio);
            pManager.characterController.AddBoostForce(force);
        }
        //
    }

    public void Jump() {
        //print("Jump " + (jumpInput && CanJump()));
        Vector2 force = Vector2.zero;

        if (CanJump()) {
            force = -gravityForce.normalized * pManager.jumpForceMagnitude;
            pManager.characterController.SetJumpForce(force);

            if (pManager.isCrouching) {
                pManager.actions.CrouchStop();
            }
        }
    }

    public void Walljump() {
        Vector2 force = Vector2.zero;

        if (CanWalljump()) {
            float orientation = pManager.isFacingRight ? 1f : -1f;
            force.Set(pManager.wallJumpDirection.x * orientation, pManager.wallJumpDirection.y * -gravityForce.normalized.y);
            force *= pManager.jumpForceMagnitude * pManager.walljumpForceMult;
            pManager.characterController.SetJumpForce(force);
            pManager.canWalljump = false;
        }
        else {
            if (CanReverseWalljump()) {
                float reverseOrientation = pManager.isFacingRight ? -1f : 1f;
                force.Set(
                    pManager.wallJumpDirection.x * reverseOrientation, 
                    pManager.wallJumpDirection.y * -gravityForce.normalized.y
                    );
                
                force *= pManager.jumpForceMagnitude * pManager.walljumpForceMult;
                pManager.characterController.SetJumpForce(force);
                pManager.canWalljump = false;
            }
        }
    }

    public void JumpBoostActivation() {
        jumpBoost = true;
    }

    public void JumpBoostDeactivation() {
        jumpBoost = false;
    }

    private bool CanJump() {
        return pManager.isGrounded && !pManager.isOnHighSlope && !pManager.isGrabbing;
    }

    private bool CanWalljump() {
        return !pManager.isGrounded && pManager.isBackOnWall && pManager.canWalljump;
    }

    private bool CanReverseWalljump() {
        return !pManager.isGrounded && pManager.isFacingObstacle;
    }

    private bool CanBoostJump(Vector2 gravityForce) {
        bool can = false;

        if(Mathf.Sign(gravityForce.y) < 0f) {
            can = pManager.body.velocity.y > pManager.jumpBoostStopVelocityThreshold;
        }
        else {
            can = pManager.body.velocity.y < -pManager.jumpBoostStopVelocityThreshold;
        }

        return can;
    }

    private bool CanBoostgravity(float gravityRatio, Vector2 gravityForce) {
        bool can = !pManager.isGrounded;
        can &= gravityRatio <= 1;
        can &= Mathf.Sign(pManager.body.velocity.y) == Mathf.Sign(gravityForce.y);

        return can;
    }
}
