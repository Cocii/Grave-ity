using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsAI : MonoBehaviour
{
    GravityManager gManager;
    ManagerAI aiManager;

    public GravityChangeEventChannelSO gravityChangesChannel;

    private void Start() {
        aiManager = GetComponent<ManagerAI>();
        gManager = GravityManager.instance;

        RegisterGravityChangesActions();
    }

    private void RegisterGravityChangesActions() {
        gravityChangesChannel.OnEventRaisedVoid += aiManager.UpdateGravity;
        gravityChangesChannel.OnEventRaisedVoid += aiManager.characterController.StartRotation;
    }
}
