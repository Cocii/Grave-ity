using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public float speed = 0.15f;

    void Update()
    {
        PlayerManager manager = PlayerManager.instance;
        Rigidbody2D body = manager.body;
        Animator anim = manager.animator;

        float velocityX = Mathf.Round((body.velocity.x / manager.defaultMaxMoveSpeed) * 10f) / 10f;
        float velocityXAbs = Mathf.Abs(velocityX);
        float velocityY = (Mathf.Round(body.velocity.y * 10f) / 10f) * -Mathf.Sign(manager.currentGravity.y);


        anim.SetFloat("velocityX", Mathf.MoveTowards(anim.GetFloat("velocityX"), velocityXAbs, speed));
        //anim.SetFloat("velocityX", velocityXAbs);
        anim.SetFloat("velocityY", velocityY);

        anim.SetBool("grounded", manager.isGrounded);
        anim.SetBool("dashing", manager.isDashing);
        anim.SetBool("grabbing", manager.isGrabbing);
        anim.SetBool("crouching", manager.isCrouching);

        bool wallback = manager.isBackOnWall && !manager.isGrounded;
        anim.SetBool("wallback", wallback);

        float grabVelocityX = velocityX * manager.actions.grabbedObjPlayerSide.x;
        anim.SetFloat("grabVelocityX", grabVelocityX);

        float grabHeight = manager.actions.grabbedHeight;
        anim.SetFloat("grabHeight", grabHeight);


    }
}
