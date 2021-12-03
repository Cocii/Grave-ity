using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class WeightedPlatform : MonoBehaviour
{
    public Transform targetPosition, door;

    public AudioSource audio;
    
    public Light2D _light;
    public float movingSpeed;
   
    // Start is called before the first frame update
    void Start()
    {
        targetPosition = GameObject.FindGameObjectWithTag("DoorTarget").transform;
        door = GameObject.FindGameObjectWithTag("Door").transform;
        _light = GetComponentInChildren<Light2D>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Move()
    {
        while (door.transform.position != targetPosition.position)
        {
            float step = movingSpeed * Time.deltaTime;
            door.transform.position = Vector3.MoveTowards(door.transform.position, targetPosition.position, step);

            // Important! Tells Unity to pause here, render this frame
            // and continue from here in the next frame
            yield return null;
        }

        // When done set the position fixed once
        // since == has a precision of 0.00001 so it never reaches an exact position
        door.transform.position = targetPosition.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BreakingBox"))
        {
            //change light to green
            _light.color = Color.green;

            //move door up
            StartCoroutine(Move());
            
            audio.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BreakingBox"))
        {
            //change light to green
            _light.color = Color.red;

        }
    }
}
