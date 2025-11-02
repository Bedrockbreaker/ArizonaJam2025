using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]
public class Character : Pawn
{
	[Header("Stats")]
	public float speed = 5f;
	[Tooltip("Greater values are more responsive"), Min(float.Epsilon)]
	public float speedSmoothing = 0.5f;
	public float cameraLookAheadScale = 2.5f;

	[Header("Component References")]
	public Animator animator;
	public Rigidbody physicsBody;
	public GameObject mesh;
	public Transform cameraLookPoint;

	private float desiredSpeed = 0f;

	public override void Move(InputAction.CallbackContext context)
	{
		Vector2 input = context.ReadValue<Vector2>();

		Debug.Log(input.x);

		// Eat fast inputs (only polled once per physics tick below)
		// Good enough for a jam 
		desiredSpeed = input.x * speed;
		cameraLookPoint.transform.position = transform.position + cameraLookAheadScale * input.x * Vector3.forward;
	}

	public override void StopMove(InputAction.CallbackContext context)
	{
		desiredSpeed = 0f;
		cameraLookPoint.transform.position = transform.position;
	}

	public override void DetachController()
	{
		base.DetachController();

		desiredSpeed = 0f;
	}

	protected void Update()
	{
		animator.SetBool("bMoving", desiredSpeed != 0f);
		if (desiredSpeed > 0f)
		{
			mesh.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		} else if (desiredSpeed < 0f)
		{
			mesh.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
		}
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
	}
#endif
}
