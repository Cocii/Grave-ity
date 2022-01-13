using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAI : MonoBehaviour
{
    public ManagerAI manager;
    public float speed = 0.15f;

    void Start()
    {
        if (manager == null)
            manager = GetComponent<ManagerAI>();
    }

    void Update()
    {
        float velocityX = Mathf.Round((manager.body.velocity.x / manager.defaultMaxMoveSpeed) * 10f) * 0.1f;
        float velocityXAbs = Mathf.Abs(velocityX);
        float smoothedVelocityX = Mathf.MoveTowards(manager.animator.GetFloat("velocityX"), velocityXAbs, speed);
        manager.animator.SetFloat("velocityX", smoothedVelocityX);

        float velocityY = (Mathf.Round(manager.body.velocity.y * 10f) / 10f) * -Mathf.Sign(manager.currentGravity.y);
        manager.animator.SetFloat("velocityY", velocityY);

        manager.animator.SetBool("grounded", manager.isGrounded);
        manager.animator.SetBool("dashing", manager.isDashing);
        manager.animator.SetBool("grabbing", manager.isGrabbing);
        manager.animator.SetBool("crouching", manager.isCrouching);

        //bool wallback = manager.isBackOnWall && !manager.isGrounded;
        //anim.SetBool("wallback", wallback);

        //float grabVelocityX = velocityX * manager.actions.grabbedObjPlayerSide.x;
        //anim.SetFloat("grabVelocityX", grabVelocityX);

        //float grabHeight = manager.actions.grabbedHeight;
        //anim.SetFloat("grabHeight", grabHeight);
    }
}
