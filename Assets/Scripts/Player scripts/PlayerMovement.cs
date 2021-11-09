using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Update()
    {
        MovementCheck();
    }

    private void MovementCheck() {
        Vector2 moveInput = PlayerManager.instance.input.GetMoveInput;
        bool jumpInput = PlayerManager.instance.input.GetSpaceDInput;
        Vector2 force = Vector2.zero;
        Vector2 gravityForce = PlayerManager.instance.currentGravity;
        PlayerManager manager = PlayerManager.instance;
        
        if (moveInput.magnitude > 0.1f) {
            force = manager.currentGravityNormal * moveInput * manager.moveForceMagnitude;

            if (gravityForce.normalized == Vector2.down || gravityForce.normalized == Vector2.right) {
                force *= -1f;
            }

            manager.characterControllerDynamic.SetMoveForce(force);
        }

        if (jumpInput && manager.isGrounded) {
            force = -manager.currentGravity.normalized * manager.jumpForceMagnitude;
            PlayerManager.instance.characterControllerDynamic.SetJumpForce(force);
        }

        jumpInput = manager.input.GetSpaceInput;
        if (jumpInput && manager.body.velocity.y > 0f) {
            force = -manager.currentGravity.normalized * manager.jumpForceMagnitude * manager.jumoBoostMult * Time.deltaTime;
            manager.characterControllerDynamic.AddJumpForce(force);
        }

    }
}
