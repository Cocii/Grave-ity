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

    private bool inputJump;
    private bool inputJumpD;
    private bool inputGravityUpsideD;
    private bool inputGravityStrongerD;
    private bool inputGravityWeakerD;

    private bool inputDashD;
    private float lastTimeInputDashD;
    private bool inputDashAltD;
    public float doubleClickThreshold;

    private bool inputGrabD;
    private bool inputGrabU;

    [Header("Booleans")]
    public bool inputBlocked = false; 
    public bool inputEnabled = true;
    public bool resetMoveInputIfBlocked = true;

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

        inputDashAltD = Input.GetKeyDown(dashKey);
        inputDashD = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D);

        inputGrabD = Input.GetKeyDown(grabKey);
        inputGrabU = Input.GetKeyUp(grabKey);
    }

    void ExcecuteInputActions() {
        if (inputGravityUpsideD && !inputBlocked) {
            GravityManager.instance.RotateGravityUpsideDown();
        }
        if (inputGravityStrongerD && !inputBlocked) {
            GravityManager.instance.StrongerGravity();
        }
        if (inputGravityWeakerD && !inputBlocked) {
            GravityManager.instance.WeakerGravity();
        }

        if (inputDashD && !inputBlocked) {
            if((Time.time - lastTimeInputDashD) < doubleClickThreshold) {
                PlayerManager.instance.actions.Dash();
                lastTimeInputDashD = 0f;
            }
            else {
                lastTimeInputDashD = Time.time;
            }
        }

        if (inputDashAltD && !inputBlocked) {
            PlayerManager.instance.actions.Dash();
        }

        if (inputGrabD && !inputBlocked) {
            PlayerManager.instance.actions.Grab();
        }
        else if (inputGrabU && !inputBlocked) {
            PlayerManager.instance.actions.ReleaseGrab();
        }
    }

    public void DisableInput() {
        inputEnabled = false;
    }

    public void EnableInput() {
        inputEnabled = true;
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
