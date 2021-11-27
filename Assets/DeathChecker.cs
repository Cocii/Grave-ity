using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathChecker : MonoBehaviour
{
    public Transform checkpoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Checkpoint")
        {
            if(collision.transform.position != checkpoint.transform.position)
            {
                checkpoint = collision.transform;
            }
        }

        if(collision.tag == "Death Trigger")
        {
            //Oscura schermo
            //...

            GravityManager.instance.ResetGravity();
            transform.position = checkpoint.position;
            
        }
    }
}
