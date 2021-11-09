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
    public List<string> inputsToRegister;
    public List<string> inputsToRegisterD;
    private Dictionary<string, bool> inputDict;
    private Dictionary<string, bool> inputDictD;

    private bool inputShiftD; 
    private bool inputShiftU; 
    private bool inputClickLeft; 
    private bool inputClickLeftD; 
    private bool inputClickRight;
    private bool inputSpace;
    private bool inputSpaceD;


    private bool inputTabD;

    private bool inputQD;
    private bool inputED;
    private bool inputRD;
    private bool inputFD;
    
    


    [Header("Booleans")]
    public bool inputBlocked = false; 
    public bool inputEnabled = true;

    void Start() {
        inputBlocked = false;
        inputEnabled = true;

        inputDict = new Dictionary<string, bool>();
        inputDictD = new Dictionary<string, bool>();
    }

    void Update() {
        inputBlocked = !inputEnabled;
        ReadInputValues(); 
        ExcecuteInputActions(); 
    }

    void ReadInputValues() {
        inputMovement.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(!inputBlocked && mainFreeCam)
            inputCamera.Set(mainFreeCam.m_XAxis.Value, mainFreeCam.m_YAxis.Value);

        inputClickLeft = Input.GetKey(KeyCode.Mouse0);
        inputClickLeftD = Input.GetKeyDown(KeyCode.Mouse0);
        inputClickRight = Input.GetKey(KeyCode.Mouse1);
        inputSpace = Input.GetKey(KeyCode.Space);
        inputSpaceD = Input.GetKeyDown(KeyCode.Space);

        inputQD = Input.GetKeyDown(KeyCode.Q);
        inputED = Input.GetKeyDown(KeyCode.E);
        inputRD = Input.GetKeyDown(KeyCode.R);
        inputFD = Input.GetKeyDown(KeyCode.F);

        inputShiftD = Input.GetKeyDown(KeyCode.LeftShift);
        inputShiftU = Input.GetKeyUp(KeyCode.LeftShift);
    }

    void ExcecuteInputActions() {
        if (inputED) {
            GravityManager.instance.RotateGravityUpsideDown();
        }
        if (inputRD) {
            GravityManager.instance.WeakerGravity();
        }
        if (inputFD) {
            GravityManager.instance.StrongerGravity();
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
            if (inputBlocked)
                return Vector2.zero;
            return inputMovement;
        }
    }

    public Vector2 GetCameraInput {
        get {
            return inputCamera;
        }
    }

    public bool GetClickLeftInput {
        get { return inputClickLeft && !inputBlocked; }
    }

    public bool GetClickLeftDownInput {
        get { return inputClickLeftD && !inputBlocked; }
    }

    public bool GetClickRightInput {
        get { return inputClickRight && !inputBlocked; }
    }

    public bool GetSpaceInput {
        get { return inputSpace && !inputBlocked; }
    }

    public bool GetSpaceDInput {
        get { return inputSpaceD && !inputBlocked; }
    }

    
}
