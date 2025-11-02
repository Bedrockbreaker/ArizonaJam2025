using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Pawn : MonoBehaviour
{
	public delegate void OnControllerChangedDelegate(PlayerController oldController, PlayerController newController);
	public event OnControllerChangedDelegate OnControllerChanged;

	public PlayerController Controller { get; private set; }

	public virtual void Move(InputAction.CallbackContext context) { }
	public virtual void StopMove(InputAction.CallbackContext context) { }
	public virtual void Look(InputAction.CallbackContext context) { }
	public virtual void StopLook(InputAction.CallbackContext context) { }
	public virtual void Interact(InputAction.CallbackContext context) { }

	public virtual void AttachController(PlayerController controllerIn)
	{
		PlayerController oldController = Controller;
		Controller = controllerIn;

		if (Controller != null)
		{
			Controller.actionMove.action.performed += Move;
			Controller.actionMove.action.canceled += StopMove;
			Controller.actionLook.action.performed += Look;
			Controller.actionLook.action.canceled += StopLook;
			Controller.actionInteract.action.performed += Interact;
		}

		if (oldController != controllerIn) OnControllerChanged?.Invoke(oldController, controllerIn);
	}

	public virtual void DetachController()
	{
		if (Controller == null) return;

		Controller.actionMove.action.performed -= Move;
		Controller.actionMove.action.canceled -= StopMove;
		Controller.actionLook.action.performed -= Look;
		Controller.actionLook.action.canceled -= StopLook;
		Controller.actionInteract.action.performed -= Interact;

		PlayerController oldController = Controller;

		OnControllerChanged?.Invoke(oldController, null);

		Controller = null;
	}

}
