using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    public PowerUpScriptable[] powers;
    public PowerUpScriptable currentPower;

    public SpriteRenderer body;

    public bool activated = false;

    private void Start() {
        currentPower = powers[0];
        ActivateCurrentPower();
    }

    public void TakeSuit()
    {
        activated = true;
        body.color = Color.black;
    }

    public void SwitchPower() {
        if (!activated)
            return;

        currentPower.DeactivationFunction();
        currentPower = powers[(System.Array.IndexOf(powers, currentPower) + 1) % powers.Length];
        ActivateCurrentPower();
    }

    private void ActivateCurrentPower() {
        currentPower.ActivationFunction();

        UIManager uiManager = (UIManager)GameObject.FindObjectOfType(typeof(UIManager));
        if (uiManager)
            uiManager.powerNameText.text = currentPower.powerName;
    }
}
