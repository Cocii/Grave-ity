using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    PlayerManager pManager;
    GravityManager gManager;
    PlayerInputSystemActions actions;

    [Header("Movement inputs")]
    public Vector2 inputMovement; 
    
    [Header("Booleans")]
    public bool inputBlocked = false; 
    public bool inputEnabled = true;
    public bool resetMoveInputIfBlocked = true;
    public bool gravityInversionEnabled = true;

    void Start() {
        actions = new PlayerInputSystemActions();
        actions.Gameplay.Enable();
        RegisterActions();

        inputBlocked = false;
        inputEnabled = true;

        pManager = PlayerManager.instance;
        gManager = GravityManager.instance;
    }

    private void RegisterActions() {
        actions.Gameplay.Jump.started += JumpInputPhases;
        actions.Gameplay.Jump.performed += JumpInputPhases;
        actions.Gameplay.Jump.canceled += JumpInputPhases;

        actions.Gameplay.Gravityinverted.performed += InvertGravityInputPerformed;
        actions.Gameplay.Gravitystronger.performed += StrongGravityInputPerformed;
        actions.Gameplay.Gravityweaker.performed += WeakGravityInputPerformed;

        actions.Gameplay.Pause.performed += PauseGameInputPerformed;

        actions.Gameplay.SwitchPower.performed += SwitchPowerInputPerformed;

        actions.Gameplay.Dash.performed += DashInputPerformed;

        actions.Gameplay.Grab.started += GrabInputPhases;
        actions.Gameplay.Grab.canceled += GrabInputPhases;

        actions.Gameplay.Crouch.started += CrouchInputPhases;
        actions.Gameplay.Crouch.canceled += CrouchInputPhases;

        actions.Gameplay.Movement.started += MoveInputPhases;
        actions.Gameplay.Movement.canceled += MoveInputPhases;
    }

    private void OnDestroy() {
        UnregisterActions();
    }

    private void UnregisterActions() {
        if (actions == null || this==null)
            return;

        actions.Gameplay.Jump.started -= JumpInputPhases;
        actions.Gameplay.Jump.performed -= JumpInputPhases;
        actions.Gameplay.Jump.canceled -= JumpInputPhases;

        actions.Gameplay.Gravityinverted.performed -= InvertGravityInputPerformed;
        actions.Gameplay.Gravitystronger.performed -= StrongGravityInputPerformed;
        actions.Gameplay.Gravityweaker.performed -= WeakGravityInputPerformed;

        actions.Gameplay.Pause.performed -= PauseGameInputPerformed;

        actions.Gameplay.SwitchPower.performed -= SwitchPowerInputPerformed;

        actions.Gameplay.Dash.performed -= DashInputPerformed;

        actions.Gameplay.Grab.started -= GrabInputPhases;
        actions.Gameplay.Grab.canceled -= GrabInputPhases;

        actions.Gameplay.Crouch.started -= CrouchInputPhases;
        actions.Gameplay.Crouch.canceled -= CrouchInputPhases;

        actions.Gameplay.Movement.started -= MoveInputPhases;
        actions.Gameplay.Movement.canceled -= MoveInputPhases;
    }

    public void MoveInputPhases(InputAction.CallbackContext context) {
        if (!inputBlocked)
            switch (context.phase) {
                case InputActionPhase.Started:
                    inputMovement = actions.Gameplay.Movement.ReadValue<Vector2>();
                    break;

                case InputActionPhase.Canceled:
                    inputMovement = Vector2.zero;
                    break;
            }
        else 
            if (resetMoveInputIfBlocked)
                inputMovement = Vector2.zero;
    }

    public void JumpInputPhases(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        switch (context.phase) {
            case InputActionPhase.Started:
                if (pManager.isGrounded) {
                    pManager.movement.Jump();
                    pManager.movement.JumpBoostActivation();
                }
                else {
                    pManager.movement.Walljump();
                }
                    

                break;

            case InputActionPhase.Performed:
                //pManager.movement.JumpBoostDeactivation();
                break;

            case InputActionPhase.Canceled:
                pManager.movement.JumpBoostDeactivation();
                break;
        }
    }

    public void InvertGravityInputPerformed(InputAction.CallbackContext context) {
        if (inputBlocked || !gravityInversionEnabled)
            return;

        pManager.events.gravityChangesChannel.RaiseEvent(GravityChangesEnum.Inverted);
        pManager.events.gravityChangesChannel.RaiseEventVoid();
    }

    public void WeakGravityInputPerformed(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        pManager.events.gravityChangesChannel.RaiseEvent(GravityChangesEnum.Weaker);
        pManager.events.gravityChangesChannel.RaiseEventVoid();
    }

    public void StrongGravityInputPerformed(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        pManager.events.gravityChangesChannel.RaiseEvent(GravityChangesEnum.Stronger);
        pManager.events.gravityChangesChannel.RaiseEventVoid();
    }

    public void PauseGameInputPerformed(InputAction.CallbackContext context) {
        //print("GAME PAUSE PRESSED");
        pManager.events.gameMenuChannel.RaiseEvent();
    }

    public void SwitchPowerInputPerformed(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        pManager.events.powerSwitchedChannel.RaiseEvent();
    }

    public void DashInputPerformed(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        pManager.actions.Dash();
    }

    public void GrabInputPhases(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        switch (context.phase) {
            case InputActionPhase.Started:
                pManager.actions.Grab();
                break;

            case InputActionPhase.Performed:
                
                break;

            case InputActionPhase.Canceled:
                pManager.actions.ReleaseGrab();
                break;
        }
    }

    public void CrouchInputPhases(InputAction.CallbackContext context) {
        if (inputBlocked)
            return;

        switch (context.phase) {
            case InputActionPhase.Started:
                pManager.actions.Crouch();
                break;

            case InputActionPhase.Performed:
                
                break;

            case InputActionPhase.Canceled:
                pManager.actions.CrouchStop();
                break;
        }
    }

    
    public void DisableInput() {
        inputEnabled = false;
        inputBlocked = !inputEnabled;
    }

    public void EnableInput() {
        inputEnabled = true;
        inputBlocked = !inputEnabled;
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

    //public bool GetJumpInput {
    //    get { return inputJump && !inputBlocked; }
    //}

}
