using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    public PowerUpScriptable[] powers;
    public PowerUpScriptable currentPower;

    private void Start() {
        currentPower = powers[0];
        ActivateCurrentPower();
    }

    public void SwitchPower() {
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
