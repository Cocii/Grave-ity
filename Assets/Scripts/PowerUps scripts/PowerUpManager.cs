using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpManager : MonoBehaviour
{
    public PowerUpScriptable[] powers;
    public PowerUpScriptable currentPower;

    private void Start() {
        currentPower = powers[0];
        currentPower.ActivationFunction();
    }

    public void SwitchPower() {
        currentPower.DeactivationFunction();
        currentPower = powers[(System.Array.IndexOf(powers, currentPower) + 1) % powers.Length];
        currentPower.ActivationFunction();
    }
}
