using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBall : MonoBehaviour
{
    [SerializeField]
    int memory;

    public GameObject key_press;

    private Collider2D trigger;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<Collider2D>();
        key_press.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Collect
            Debug.Log("YOU CAN COLLECT");
            key_press.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        key_press.SetActive(false);
    }
}
