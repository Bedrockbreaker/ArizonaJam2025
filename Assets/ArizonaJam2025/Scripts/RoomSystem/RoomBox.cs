using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoomBox : MonoBehaviour
{
	public BoxCollider trigger;
	public Transform cameraStart;
	public Transform cameraEnd;

	public void OnTriggerEnter(Collider other)
	{
		if (!other.TryGetComponent<Character>(out _)) return;
		GameManager.Instance.GetCameraManager().UseRoomTrack(this);
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (trigger == null) trigger = GetComponent<BoxCollider>();
		if (trigger == null) throw new System.Exception("RoomBox must have a BoxCollider");

		trigger.isTrigger = true;
		trigger.size = new Vector3(4, 4, 50);
		trigger.includeLayers = 1 << LayerMask.NameToLayer("Player");
	}

	protected void OnDrawGizmos()
	{
		// Draw a line between the camera transforms
		if (cameraStart == null || cameraEnd == null) return;
		Gizmos.color = Color.green;
		Gizmos.DrawLine(cameraStart.position, cameraEnd.position);
	}
#endif
}
