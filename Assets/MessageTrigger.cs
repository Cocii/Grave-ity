using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    UIManager uiManager;

    [SerializeField]
    string text;

    [SerializeField]
    float time;


    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            uiManager.SetUpperCenterTextAndDelay(text, time);
            Destroy(gameObject);
        }
    }

    public void ShowMessage()
    {
        uiManager.SetUpperCenterTextAndDelay(text, time);
    }
}
