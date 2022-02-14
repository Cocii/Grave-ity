using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiFanController : MonoBehaviour
{
    public List<Fan> fans;
    public VoidEventChannelSO changeFanChannel;
    public float timeBeforeChange = 1f;
    
    [Space]

    public int funsLenght = 0;
    public int currentActive = 0;

    private void Start() {
        currentActive = 0;
        funsLenght = fans.Count;    
        foreach(Fan f in fans) {
            DeactivateFan(f);
        }
        ActivateFan(fans[currentActive]);
        StartCoroutine(FanActivationCo());
    }

    private void OnEnable() {
        changeFanChannel.OnEventRaised += ActiveNext;
    }

    private void OnDisable() {
        changeFanChannel.OnEventRaised -= ActiveNext;
    }

    public void ActiveNext() {
        DeactivateFan(fans[currentActive]);
        currentActive = (currentActive + 1) % funsLenght;
        ActivateFan(fans[currentActive]);
        StartCoroutine(FanActivationCo());
    }

    IEnumerator FanActivationCo() {
        yield return new WaitForSeconds(timeBeforeChange);
        changeFanChannel.RaiseEvent();
        yield break;
    }

    private void ActivateFan(Fan fan) {
        fan.enabled = true;
        fan.Activation();
    }

    private void DeactivateFan(Fan fan) {
        fan.enabled = false;
        fan.Deactivation();
    }
}
