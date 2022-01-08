using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    UIManager uiManager;

    [SerializeField]
    string text;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.FindGameObjectWithTag("GameCanvas").GetComponent<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            uiManager.SetUpperCenterTextAndDelay(text, 7f);
            Destroy(gameObject);
        }
    }
}
