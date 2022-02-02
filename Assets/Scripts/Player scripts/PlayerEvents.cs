using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    PlayerManager pManager;
    GravityManager gManager;

    public GravityChangeEventChannelSO gravityChangesChannel;
    public VoidEventChannelSO powerSwitchedChannel;
    public VoidEventChannelSO gameMenuChannel;
    //public VoidEventChannelSO 

    private void Awake() {
        
    }

    private void Start() {
        pManager = PlayerManager.instance;
        gManager = GravityManager.instance;
        RegisterGravityChangesActions();
        RegisterPowerSwitchedActions();
    }

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    private void OnDestroy() {
        UnregisterGravityChangesActions();
        UnregisterPowerSwitchedActions();
    }

    private void RegisterGravityChangesActions() {
        gravityChangesChannel.OnEventRaised += gManager.GravityChange;
        gravityChangesChannel.OnEventRaised += pManager.sound.PlayGravityChange;
        gravityChangesChannel.OnEventRaised += pManager.input.DisableGravityInversion;
        
        gravityChangesChannel.OnEventRaisedVoid += pManager.UpdateGravity;
        gravityChangesChannel.OnEventRaisedVoid += pManager.movement.UpdateGravityInfo;
        gravityChangesChannel.OnEventRaisedVoid += pManager.characterController.StartRotation;
    }

    private void RegisterPowerSwitchedActions() {
        powerSwitchedChannel.OnEventRaised += pManager.powersManager.SwitchPower;
    }

    private void UnregisterGravityChangesActions() {
        if (gManager == null || pManager == null)
            return;

        gravityChangesChannel.OnEventRaised -= gManager.GravityChange;
        gravityChangesChannel.OnEventRaised -= pManager.sound.PlayGravityChange;
        gravityChangesChannel.OnEventRaised -= pManager.input.DisableGravityInversion;

        gravityChangesChannel.OnEventRaisedVoid -= pManager.UpdateGravity;
        gravityChangesChannel.OnEventRaisedVoid -= pManager.movement.UpdateGravityInfo;
        gravityChangesChannel.OnEventRaisedVoid -= pManager.characterController.StartRotation;
    }

    private void UnregisterPowerSwitchedActions() {
        if (gManager == null || pManager == null)
            return;

        powerSwitchedChannel.OnEventRaised -= pManager.powersManager.SwitchPower;
    }
}
