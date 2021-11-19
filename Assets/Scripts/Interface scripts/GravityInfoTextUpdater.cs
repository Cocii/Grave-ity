using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class GravityInfoTextUpdater : MonoBehaviour
{
    public Text gravityText;

    private void Awake() {
        if (gravityText == null)
            gravityText = GetComponent<Text>();
    }

    void Update()
    {
        GravityManager manager = GravityManager.instance;

        if (manager == null)
            return;

        gravityText.text = "Gravity: " + manager.physicsGravity.ToString() + ", ratio: " + manager.gravityRatio; 
    }
}
