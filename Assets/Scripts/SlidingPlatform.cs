using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPlatform : MonoBehaviour
{
    public Transform slidingPlatform;
    public float movingSpeed;

    public Transform targetPosition;
    public BoxCollider2D triggerCollider;

    private void Start()
    {        
        
     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {

            //move platform
            //triggerCollider.enabled = false;
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        while (slidingPlatform.position != targetPosition.position)
        {
            float step = movingSpeed * Time.deltaTime;
            slidingPlatform.position = Vector3.MoveTowards(slidingPlatform.position, targetPosition.position, step);

            // Important! Tells Unity to pause here, render this frame
            // and continue from here in the next frame
            yield return null;
        }

        // When done set the position fixed once
        // since == has a precision of 0.00001 so it never reaches an exact position
        slidingPlatform.position = targetPosition.position;
    }
}
