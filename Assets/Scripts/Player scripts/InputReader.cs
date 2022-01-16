using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputReader : MonoBehaviour
{
    PlayerManager pManager;
    GravityManager gManager;

    [Header("Movement inputs")]
    public Vector2 inputMovement; 
    public Vector2 inputCamera;

    [Header("Keys")]
    public KeyCode jumpKey;
    public KeyCode gravityStrongerKey;
    public KeyCode gravityWeakerKey;
    public KeyCode gravityUpsideKey;
    public KeyCode dashKey;
    public KeyCode grabKey;
    public KeyCode switchPowerKey;
    public KeyCode crouchKey;
    public KeyCode menuKey;

    private bool inputJump;
    private bool inputJumpD;
    private bool inputGravityUpsideD;
    private bool inputGravityStrongerD;
    private bool inputGravityWeakerD;
    private bool inputDashD;
    private bool inputCrouchD;
    private bool inputCrouchU;
    private bool inputMenuD;
    private bool inputSwitchPowerD;
    private bool inputGrabD;
    private bool inputGrabU;

    [Header("Booleans")]
    public bool inputBlocked = false; 
    public bool inputEnabled = true;
    public bool resetMoveInputIfBlocked = true;
    public bool gravityInversionEnabled = true;

    void Start() {
        inputBlocked = false;
        inputEnabled = true;

        pManager = PlayerManager.instance;
        gManager = GravityManager.instance;
    }

    void Update() {
        inputBlocked = !inputEnabled;
        ReadInputValues(); 
        ExcecuteInputActions(); 
    }

    void ReadInputValues() {
        if (!inputBlocked)
            inputMovement.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else if (resetMoveInputIfBlocked)
            inputMovement = Vector2.zero;
            
        //if(!inputBlocked && mainFreeCam)
        //    inputCamera.Set(mainFreeCam.m_XAxis.Value, mainFreeCam.m_YAxis.Value);

        inputJump = Input.GetKey(jumpKey);
        inputJumpD = Input.GetKeyDown(jumpKey);

        inputGravityUpsideD = Input.GetKeyDown(gravityUpsideKey);
        inputGravityStrongerD = Input.GetKeyDown(gravityStrongerKey);
        inputGravityWeakerD = Input.GetKeyDown(gravityWeakerKey);

        inputGrabD = Input.GetKeyDown(grabKey);
        inputGrabU = Input.GetKeyUp(grabKey);
        inputDashD = Input.GetKeyDown(dashKey);
        inputCrouchD = Input.GetKeyDown(crouchKey);
        inputCrouchU = Input.GetKeyUp(crouchKey);

        inputMenuD = Input.GetKeyDown(menuKey);

        inputSwitchPowerD = Input.GetKeyDown(switchPowerKey);
    }

    void ExcecuteInputActions() {
        if (inputGravityUpsideD && !inputBlocked && gravityInversionEnabled) {
            pManager.events.gravityChangesChannel.RaiseEvent(GravityChangesEnum.Inverted);
            pManager.events.gravityChangesChannel.RaiseEventVoid();
        }
        if (inputGravityStrongerD && !inputBlocked) {
            pManager.events.gravityChangesChannel.RaiseEvent(GravityChangesEnum.Stronger);
            pManager.events.gravityChangesChannel.RaiseEventVoid();
        }
        if (inputGravityWeakerD && !inputBlocked) {
            pManager.events.gravityChangesChannel.RaiseEvent(GravityChangesEnum.Weaker);
            pManager.events.gravityChangesChannel.RaiseEventVoid();
        }

        if (inputDashD && !inputBlocked) {
            pManager.actions.Dash();
        }

        if (inputGrabD && !inputBlocked) {
            pManager.actions.Grab();
        }
        else if (inputGrabU && !inputBlocked) {
            pManager.actions.ReleaseGrab();
        }

        if(inputCrouchD && !inputBlocked) {
            pManager.actions.Crouch();
        }else if (inputCrouchU && !inputBlocked) {
            pManager.actions.CrouchStop();
        }

        if (inputMenuD) {
            //GameMenuManager.instance.SwitchState();
            pManager.events.gameMenuChannel.RaiseEvent();
        }

        if(inputSwitchPowerD && !inputBlocked) {
            //PlayerManager.instance.events.powerSwitched.Invoke();
            pManager.events.powerSwitchedChannel.RaiseEvent();
        }
    }

    public void DisableInput() {
        inputEnabled = false;
    }

    public void EnableInput() {
        inputEnabled = true;
    }

    public void DisableGravityInversion() {
        gravityInversionEnabled = false;
    }

    public void DisableGravityInversion(GravityChangesEnum changeRequested) {
        if(changeRequested==GravityChangesEnum.Inverted)
            gravityInversionEnabled = false;
    }

    public void EnableGravityInversion() {
        gravityInversionEnabled = true;
    }

    public Vector2 GetMoveInput {
        get {
            return inputMovement;
        }
    }

    public Vector2 GetCameraInput {
        get {
            return inputCamera;
        }
    }

    public bool GetJumpInput {
        get { return inputJump && !inputBlocked; }
    }

    public bool GetJumpDInput {
        get { return inputJumpD && !inputBlocked; }
    }

    
}
