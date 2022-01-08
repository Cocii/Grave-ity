using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryBall : MonoBehaviour
{
    [SerializeField]
    string memory;

    public GameObject key_press;
    public GameObject msgTrigger;
    private MessageTrigger msg;

    private bool canCollect = false;

    // Start is called before the first frame update
    void Start()
    {
        key_press.SetActive(false);

        msg = msgTrigger.GetComponent<MessageTrigger>();
        msgTrigger.SetActive(false);

        int isCollected = PlayerPrefs.GetInt(memory);
        if(isCollected == 1)
        {
            //Destroy(gameObject);
        }

    }

    private void Update()
    {
        if(canCollect)
        {
            if(memory != null)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    msgTrigger.SetActive(true);
                    msg.ShowMessage();
                    PlayerPrefs.SetInt(memory, 1);
                    PlayerPrefs.Save();
                    Destroy(gameObject);
                }
            }
        }
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
