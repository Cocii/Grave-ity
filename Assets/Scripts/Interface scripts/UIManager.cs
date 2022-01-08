using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject upperCenterPanel;

    [Header("Texts")]
    public Text powerNameText;
    public Text upperCentertext;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.J)) {
            SetUpperCenterTextAndDelay("test attivazione 2 secondi", 2f);
        }
    }

    public void SetUpperCenterTextAndDelay(string text, float delay) {
        upperCentertext.text = text;
        ActivateUpperCenterPanel();
        DeactivateUpperCenterPanelDelay(delay);
    }

    public void ActivateUpperCenterPanel() {
        upperCenterPanel.SetActive(true);
    }

    public void DeactivateUpperCenterPanelDelay(float delay) {
        StartCoroutine(DelayerDeactivation(delay, upperCenterPanel));
    }

    IEnumerator DelayerDeactivation(float time, GameObject obj) {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        yield break;
    }
}
