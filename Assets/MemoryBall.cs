using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBall : MonoBehaviour
{
    [SerializeField]
    int memory;

    public GameObject key_press;

    private bool canCollect = false;

    // Start is called before the first frame update
    void Start()
    {
        key_press.SetActive(false);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Collect
            canCollect = true;
            key_press.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canCollect = false;
            key_press.SetActive(false);
        }
    }
}
