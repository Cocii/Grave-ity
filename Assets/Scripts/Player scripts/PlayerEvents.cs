using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    PlayerManager pManager;
    GravityManager gManager;

    public GravityChangeEventChannelSO gravityChangesChannel;
    public VoidEventChannelSO powerSwitchedChannel;
    public VoidEventChannelSO gameMenuChannel;

    private void Start() {
        pManager = PlayerManager.instance;
        gManager = GravityManager.instance;

        RegisterGravityChangesActions();
        RegisterPowerSwitchedActions();
        RegisterGameMenuActions();
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

    private void RegisterGameMenuActions() {
        gameMenuChannel.OnEventRaised += GameMenuManager.instance.SwitchState;
    }
}
