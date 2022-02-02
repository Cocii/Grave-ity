using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptables/Input Reader")]
public class InputReaderSO : DescriptionBaseSO //, PlayerInputSystemActions.IGameplayActions
{
	/*
	private PlayerInputSystemActions inputSystem;

	//Phases events
	public event UnityAction JumpStartedEvent = delegate { };
	public event UnityAction JumpPerformedEvent = delegate { };
	public event UnityAction JumpCanceledEvent = delegate { };
	public event UnityAction GrabStartedEvent = delegate { };
	public event UnityAction GrabCanceledEvent = delegate { };
	public event UnityAction CrouchStartedEvent = delegate { };
	public event UnityAction CrouchCanceledEvent = delegate { };

	//Only performed events
	public event UnityAction InteractEventPerformed = delegate { };
	public event UnityAction MenuPauseEventPerformed = delegate { };
	public event UnityAction GravityInvertedEventPerformed = delegate { };
	public event UnityAction GravityStrongerEventPerformed = delegate { };
	public event UnityAction GravityWeakerEventPerformed = delegate { };
	public event UnityAction SwitchPowerEventPerformed = delegate { };
	public event UnityAction DashEventPerformed = delegate { };

	//Update events
	public event UnityAction<Vector2> MoveEvent = delegate { };

	private void OnEnable() {
		if (inputSystem == null) {
			inputSystem = new PlayerInputSystemActions();
			inputSystem.Gameplay.SetCallbacks(this);
		}

		Debug.Log("Input reader object enabled");
	}

	private void OnDisable() {
		DisableAllInput();
	}

	public void EnableGameplayInput() {
		inputSystem.Gameplay.Enable();
	}

	public void DisableAllInput() {
		inputSystem.Gameplay.Disable();
	}

	//Gameplay interface callbacks
	public void OnCrouch(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:

				break;
			case InputActionPhase.Performed:

				break;
			case InputActionPhase.Canceled:

				break;
		}
	}

    public void OnDash(InputAction.CallbackContext context) {
		if (context.performed) {

		}
	}

    public void OnGrab(InputAction.CallbackContext context) {
		switch (context.phase) {
			case InputActionPhase.Started:

				break;
			case InputActionPhase.Performed:

				break;
			case InputActionPhase.Canceled:

				break;
		}
	}

    public void OnGravityinverted(InputAction.CallbackContext context) {
		if (context.performed) {

		}
	}

    public void OnGravitystronger(InputAction.CallbackContext context) {
		if (context.performed) {

		}
	}

    public void OnGravityweaker(InputAction.CallbackContext context) {
		if (context.performed) {

		}
	}

    public void OnJump(InputAction.CallbackContext context) {
		Debug.Log("Input reader jump context: " + context.phase);

		switch (context.phase) {
			case InputActionPhase.Started:

				break;
			case InputActionPhase.Performed:

				break;
			case InputActionPhase.Canceled:

				break;
		}
	}

    public void OnMovement(InputAction.CallbackContext context) {
		Debug.Log("Input reader move context: " + context.phase);

		if (context.performed) {
			Debug.Log(inputSystem.Gameplay.Movement.ReadValue<Vector2>());
		}
        if (context.canceled) {
			Debug.Log(inputSystem.Gameplay.Movement.ReadValue<Vector2>());
		}
	}

    public void OnPause(InputAction.CallbackContext context) {
		if (context.performed) {

		}
	}

    public void OnSwitchPower(InputAction.CallbackContext context) {
        if (context.performed) {

        }
	}

	
	*/
}

