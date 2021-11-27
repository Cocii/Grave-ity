using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    void Update()
    {
        PlayerManager manager = PlayerManager.instance;

        Rigidbody2D body = manager.body;
        Animator anim = manager.animator;

        float velocityX = Mathf.Abs(Mathf.Round((body.velocity.x / manager.defaultMaxMoveSpeed) * 10f) / 10f);
        float velocityY = (Mathf.Round(body.velocity.y * 10f) / 10f);

        anim.SetFloat("velocityX", velocityX);
        anim.SetFloat("velocityY", velocityY);
        anim.SetBool("grounded", manager.isGrounded);
        anim.SetBool("dashing", manager.isDashing);
    }
}
