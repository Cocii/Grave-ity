using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutdownEngine : MonoBehaviour
{
    public GameObject key_press;
    public GameObject glitch;
    private bool canPress = false;

    public List<GameObject> toDeactivate;
    public Collider2D confineCollider;
    public SpriteRenderer confineRenderer;



    // Start is called before the first frame update
    void Start()
    {
        key_press.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(canPress)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                foreach(GameObject go in toDeactivate)
                {
                    GetComponentInChildren<AudioSource>().Play();

                    glitch.SetActive(false);
                    go.SetActive(false);
                    confineCollider.enabled = false;
                    confineRenderer.color = Color.white;
                    
                }
                    gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPress = true;
            key_press.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canPress = false;
            key_press.SetActive(false);
        }
    }
}
