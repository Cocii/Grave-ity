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

        float velocityX = Mathf.Abs(Mathf.Round(body.velocity.x * 100f) / 100f);
        float velocityY = Mathf.Abs(Mathf.Round(body.velocity.y * 100f) / 100f);

        anim.SetFloat("velocityX", velocityX);
        anim.SetFloat("velocityY", velocityY);
        anim.SetBool("grounded", PlayerManager.instance.isGrounded);

    }
}
