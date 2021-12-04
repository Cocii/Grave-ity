using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debugBuild : MonoBehaviour
{
    public Text text;
    public PlayerManager manager;

    private void Start() {
        manager = PlayerManager.instance;
        text = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        //text.text = manager.characterController.groundAngle.ToString();
        //text.text = manager.characterController.maxSlopeAngle.ToString();
    }
}
