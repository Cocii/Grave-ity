using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSuit : MonoBehaviour
{
    public GameObject key_press;
    private bool canPress = false;

    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        key_press.SetActive(false);
        player = PlayerManager.instance;
        if(player.powersManager.activated)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canPress)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                player.powersManager.TakeSuit();
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
