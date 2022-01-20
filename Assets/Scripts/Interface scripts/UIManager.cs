using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    public static UIManager instance;

    [Header("Panels")]
    public GameObject upperCenterPanel;
    public GameObject comandsPanel;

    [Header("Texts")]
    public Text powerNameText;
    public Text upperCentertext;

    [Header("Animators")]
    public Animator comandsPanelAnimator;

    [Header("Channels")]
    public VoidEventChannelSO menuSwitchEventChannel;

    [Header("Bools")]
    public bool comandsPanelActivationState = false;
    public bool comandsPanelLastActivationState = false;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        menuSwitchEventChannel.OnEventRaised += UpdateAnimators;
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

    public void ActivateComandsPanel(bool activation) {
        //comandsPanel.SetActive(activation);
        comandsPanelActivationState = activation;
    }
    
    public void UpdateAnimators() {
        if(comandsPanelLastActivationState != comandsPanelActivationState) {
            comandsPanelLastActivationState = comandsPanelActivationState;
            comandsPanelAnimator.SetBool("Visible", comandsPanelActivationState);
            comandsPanelAnimator.SetTrigger("Transition");
        }
        
        //if(comandsPanelAnimator.GetBool("Visibile") != comandsPanelActivationState) {
        //    comandsPanelAnimator.SetBool("Visible", comandsPanelActivationState);
        //    comandsPanelAnimator.SetTrigger("Transition");
        //}
        
    }
}
