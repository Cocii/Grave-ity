using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager manager;

    private void Start() {
        manager = PlayerManager.instance;
    }

    void Update()
    {
        MovementCheck();
    }

    private void MovementCheck() {
        Vector2 moveInput = PlayerManager.instance.input.GetMoveInput;
        bool jumpInput = PlayerManager.instance.input.GetJumpDInput;
        Vector2 force = Vector2.zero;
        Vector2 gravityForce = PlayerManager.instance.currentGravity;
        
        
        if (moveInput.magnitude > 0.1f) {
            force = manager.currentGravityNormal * moveInput * manager.moveForceMagnitude;

            if (gravityForce.normalized == Vector2.down || gravityForce.normalized == Vector2.right) {
                force *= -1f;
            }

            manager.characterController.SetMoveForce(force);
        }

        if (jumpInput && manager.isGrounded) {
            force = -manager.currentGravity.normalized * manager.jumpForceMagnitude;
            PlayerManager.instance.characterController.SetJumpForce(force);
        }

        jumpInput = manager.input.GetJumpInput;
        if (jumpInput && manager.body.velocity.y > 0f) {
            force = -manager.currentGravity.normalized * manager.jumpForceMagnitude * manager.jumpBoostMult * Time.deltaTime;
            manager.characterController.AddJumpForce(force);
        }

    }
}
