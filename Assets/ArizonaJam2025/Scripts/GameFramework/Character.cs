using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
public class Character : Pawn
{
	[Header("Stats")]
	public float walkSpeed = 2.5f;
	public float pushSpeed = 1.5f;
	[Tooltip("Greater values are more responsive"), Min(float.Epsilon)]
	public float speedSmoothing = 0.5f;
	public float cameraLookAheadScale = 2.5f;
	[Tooltip("Greater values are more responsive"), Min(float.Epsilon)]
	public float carrySmoothing = 1.0f;

	[Header("Component References")]
	public Animator animator;
	public Rigidbody physicsBody;
	public GameObject mesh;
	public CapsuleCollider characterCollider;
	public Transform cameraLookPoint;
	public Transform carryPoint;
	public SeeingEye seeingEye;

	private bool bHiding = false;
	private Transform carryObject = null;
	private float currentSpeed = 0f;
	private float desiredSpeed = 0f;

	public override void Move(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();

		// Eat fast inputs (only polled once per physics tick below)
		// Good enough for a jam 
		desiredSpeed = input.x * currentSpeed;
		cameraLookPoint.transform.position = transform.position + cameraLookAheadScale * input.x * Vector3.forward;
	}

	public override void StopMove(InputAction.CallbackContext context)
	{
		desiredSpeed = 0f;
		cameraLookPoint.transform.position = transform.position;
	}

	public override void Look(InputAction.CallbackContext context)
	{
		Debug.Log("Character.Look");
		GameManager.Instance.GetSeeingEye().Look(context);
	}

	protected Collider[] interactables = new Collider[8];
	public override void Interact(InputAction.CallbackContext context)
	{
		if (bHiding)
		{
			Unhide();
			return;
		}
		else if (carryObject != null)
		{
			Carry(null);
			return;
		}

		Physics.OverlapCapsuleNonAlloc(
			characterCollider.bounds.center + 0.5f * characterCollider.height * Vector3.up,
			characterCollider.bounds.center - 0.5f * characterCollider.height * Vector3.up,
			characterCollider.radius,
			interactables
		);

		foreach (Collider collider in interactables)
		{
			if (collider == null) continue;
			if (collider.TryGetComponent<Interactable>(out var interactable))
			{
				interactable.Interact();
				break;
			}
		}
	}

	public void Carry(Transform transformIn)
	{
		if (carryObject != null) carryObject.GetComponent<Rigidbody>().useGravity = true;
		carryObject = transformIn;
		if (carryObject == null)
		{
			SetSpeed(walkSpeed);
		}
		else
		{
			carryObject.GetComponent<Rigidbody>().useGravity = false;
			SetSpeed(pushSpeed);
		}
	}

	public void Hide()
	{
		if (carryObject != null) Carry(null);

		bHiding = true;
		mesh.SetActive(false);
		physicsBody.useGravity = false;
		characterCollider.enabled = false;
	}

	public void Unhide()
	{
		mesh.SetActive(true);
		physicsBody.useGravity = true;
		characterCollider.enabled = true;
		bHiding = false;
	}

	public void SetSpeed(float speedIn)
	{
		currentSpeed = speedIn;
		animator.speed = speedIn / 2.5f; // HACK: This hardcoded number is timed to the specific animation used.
	}

	public override void DetachController()
	{
		base.DetachController();

		desiredSpeed = 0f;
	}

	protected void Start()
	{
		SetSpeed(walkSpeed);
	}

	private bool oldbMoving = false;
	protected void Update()
	{
		animator.SetBool("bMoving", desiredSpeed != 0f);
		if (desiredSpeed > 0f)
		{
			mesh.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		}
		else if (desiredSpeed < 0f)
		{
			mesh.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		}

		bool newbMoving = desiredSpeed != 0f;
		if (carryObject != null)
		{
			if (oldbMoving != newbMoving && desiredSpeed != 0f)
			{
				carryObject.position = carryPoint.position;
			}
			else
			{
				carryObject.position = Vector3.Lerp(
					carryObject.position,
					carryPoint.position,
					1 - Mathf.Exp(-carrySmoothing * Time.deltaTime)
				);
			}
		}

		oldbMoving = newbMoving;
	}

	protected void FixedUpdate()
	{
		Vector3 newVelocity = Vector3.Lerp(
			physicsBody.linearVelocity,
			Vector3.forward * desiredSpeed,
			1 - Mathf.Exp(-speedSmoothing * Time.fixedDeltaTime)
		);
		physicsBody.linearVelocity = newVelocity;

		
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (animator == null) animator = GetComponent<Animator>();
		if (physicsBody == null) physicsBody = GetComponent<Rigidbody>();
		if (characterCollider == null) characterCollider = GetComponent<CapsuleCollider>();
	}
#endif
}
