using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsAI : MonoBehaviour
{
    GravityManager gManager;
    ManagerAI aiManager;

    public GravityChangeEventChannelSO gravityChangesChannel;

    private void Awake() {
        aiManager = GetComponent<ManagerAI>();
        gManager = GravityManager.instance;
    }

    private void OnEnable() {   
        RegisterGravityChangesActions();
    }

    private void OnDisable() {
        UnregisterGravityChangesActions();
    }

    private void RegisterGravityChangesActions() {
        gravityChangesChannel.OnEventRaisedVoid += aiManager.UpdateGravity;
        gravityChangesChannel.OnEventRaisedVoid += aiManager.characterController.StartRotation;
    }

    private void UnregisterGravityChangesActions() {
        gravityChangesChannel.OnEventRaisedVoid -= aiManager.UpdateGravity;
        gravityChangesChannel.OnEventRaisedVoid -= aiManager.characterController.StartRotation;
    }
}
