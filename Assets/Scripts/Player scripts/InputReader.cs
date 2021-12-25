using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class InputReader : MonoBehaviour
{
    [HideInInspector]
    public GameObject mainCamera; 
    [HideInInspector]
    public CinemachineFreeLook mainFreeCam;
    [HideInInspector]
    public CinemachineVirtualCamera mainVirtualCamera;
    
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

    //private float lastTimeInput;
    //public float doubleClickThreshold;

    private bool inputGrabD;
    private bool inputGrabU;

    [Header("Booleans")]
    public bool inputBlocked = false; 
    public bool inputEnabled = true;
    public bool resetMoveInputIfBlocked = true;
    public bool gravityChangeEnabled = true;

    void Start() {
        inputBlocked = false;
        inputEnabled = true;
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
            
        if(!inputBlocked && mainFreeCam)
            inputCamera.Set(mainFreeCam.m_XAxis.Value, mainFreeCam.m_YAxis.Value);

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
    }

    void ExcecuteInputActions() {
        if (inputGravityUpsideD && !inputBlocked && gravityChangeEnabled) {
            GravityManager.instance.RotateGravityUpsideDown();
            DisableGravityChange();
        }
        if (inputGravityStrongerD && !inputBlocked && gravityChangeEnabled) {
            GravityManager.instance.StrongerGravity();
            PlayerManager.instance.sound.PlayGravvityStronger();
        }
        if (inputGravityWeakerD && !inputBlocked && gravityChangeEnabled) {
            GravityManager.instance.WeakerGravity();
            PlayerManager.instance.sound.PlayGravvityWeaker();
        }

        //if (inputDashD && !inputBlocked) {
        //    if((Time.time - lastTimeInput) < doubleClickThreshold) {
        //        PlayerManager.instance.actions.Dash();
        //        lastTimeInput = 0f;
        //    }
        //    else {
        //        lastTimeInput = Time.time;
        //    }
        //}

        if (inputDashD && !inputBlocked) {
            PlayerManager.instance.actions.Dash();
        }

        if (inputGrabD && !inputBlocked) {
            PlayerManager.instance.actions.Grab();
        }
        else if (inputGrabU && !inputBlocked) {
            PlayerManager.instance.actions.ReleaseGrab();
        }

        if(inputCrouchD && !inputBlocked) {
            PlayerManager.instance.actions.Crouch();
        }else if (inputCrouchU && !inputBlocked) {
            PlayerManager.instance.actions.CrouchStop();
        }

        if (inputMenuD) {
            GameMenuManager.instance.SwitchState();
        }
    }

    public void DisableInput() {
        inputEnabled = false;
    }

    public void EnableInput() {
        inputEnabled = true;
    }

    public void DisableGravityChange() {
        gravityChangeEnabled = false;
    }

    public void EnableGravityChange() {
        gravityChangeEnabled = true;
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
