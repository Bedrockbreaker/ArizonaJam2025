using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public delegate void OnPawnChangedDelegate(Pawn oldPawn, Pawn newPawn);
	public event OnPawnChangedDelegate OnPawnChanged;

	[field: SerializeField]
	public Pawn Pawn { get; private set; }

	public InputActionReference actionMove;
	public InputActionReference actionLook;
	public InputActionReference actionInteract;

	public void Possess(Pawn pawnIn)
	{
		Pawn oldPawn = Pawn;

		if (Pawn != null) Pawn.DetachController();
		Pawn = pawnIn;
		if (Pawn != null) Pawn.AttachController(this);

		if (oldPawn != pawnIn) OnPawnChanged?.Invoke(oldPawn, Pawn);
	}

	protected void Awake()
	{
		GameManager.Instance.RegisterController(this);
	}

	protected void Start()
	{
		if (actionMove == null) actionMove = InputActionReference.Create(InputSystem.actions.FindAction("Move"));
		if (actionMove == null) actionMove = InputActionReference.Create(InputSystem.actions.FindAction("Look"));
		if (actionInteract == null) actionInteract =
			InputActionReference.Create(InputSystem.actions.FindAction("Interact"));

		// Initialize pawn in inspector
		if (Pawn != null)
		{
			Possess(Pawn);
			OnPawnChanged?.Invoke(null, Pawn);
		}
	}

	protected void OnDestroy()
	{
		if (Pawn != null) Pawn.DetachController();
		GameManager.Instance.UnregisterController();
	}
}
