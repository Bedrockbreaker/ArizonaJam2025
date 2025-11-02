using UnityEngine;

public class LockedDoor : MonoBehaviour
{
	public Transform lockedTransform;
	public Transform unlockedTransform;
	public bool bLocked = true;
	public float transformTimeSeconds = 0.5f;

	private Transform desiredTransform;
	private float transformTimer = 0f;

	public void Unlock()
	{
		if (bLocked == false) return;

		bLocked = false;

		desiredTransform = unlockedTransform;
		transformTimer = 0f;
	}

	public void Lock()
	{
		if (bLocked == true) return;

		bLocked = true;

		desiredTransform = lockedTransform;
		transformTimer = 0f;
	}

	protected void Start()
	{
		// Uncouple the transforms from the game object
		lockedTransform.parent = null;
		unlockedTransform.parent = null;

		// Set the initial transform
		desiredTransform = bLocked ? lockedTransform : unlockedTransform;		
	}

	protected void Update()
	{
		transformTimer += Time.deltaTime;

		transform.SetPositionAndRotation(
			Vector3.Lerp(
				transform.position,
				desiredTransform.position,
				transformTimer / transformTimeSeconds
			),
			Quaternion.Lerp(
				transform.rotation,
				desiredTransform.rotation,
				transformTimer / transformTimeSeconds
			)
		);
	}
}
