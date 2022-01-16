using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    PlayerManager manager;
    public float speed = 0.15f;

    private void Start() {
        manager = PlayerManager.instance;
    }

    void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation() {
        float velocityX = Mathf.Round((manager.body.velocity.x / manager.defaultMaxMoveSpeed) * 10f) * 0.1f;
        float smoothedVelocityX = Mathf.MoveTowards(manager.animator.GetFloat("velocityX"), Mathf.Abs(velocityX), speed);
        manager.animator.SetFloat("velocityX", smoothedVelocityX);

        float velocityY = (Mathf.Round(manager.body.velocity.y * 10f) / 10f) * -Mathf.Sign(manager.currentGravity.y);
        manager.animator.SetFloat("velocityY", velocityY);

        manager.animator.SetBool("grounded", manager.isGrounded);
        manager.animator.SetBool("dashing", manager.isDashing);
        manager.animator.SetBool("grabbing", manager.isGrabbing);
        manager.animator.SetBool("crouching", manager.isCrouching);

        bool wallback = manager.isBackOnWall && !manager.isGrounded;
        manager.animator.SetBool("wallback", wallback);

        float grabVelocityX = velocityX * manager.actions.grabbedObjPlayerSide.x;
        manager.animator.SetFloat("grabVelocityX", grabVelocityX);

        float grabHeight = manager.actions.grabbedHeight;
        manager.animator.SetFloat("grabHeight", grabHeight);
    }
}
