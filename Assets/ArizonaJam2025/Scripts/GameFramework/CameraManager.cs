using UnityEngine;

public enum MainCameraState { None, Room, Mounted }

public class CameraManager : MonoBehaviour
{
	public Camera MainCamera;
	public Camera UICamera;
	public Transform UICameraPoint;

	public float uiSmoothing = 0.5f;
	public float mainSmoothing = 0.5f;

	private MainCameraState state = MainCameraState.None;
	private RoomBox room;
	private Transform mountPoint;

	public void UseRoomTrack(RoomBox room)
	{
		this.room = room;
		state = MainCameraState.Room;
	}

	public void UseMountPoint(Transform mountPoint)
	{
		this.mountPoint = mountPoint;
		state = MainCameraState.Mounted;
	}

	protected void UpdateUICameraPosition()
	{
		if (UICamera == null || UICameraPoint == null) return;

		UICamera.transform.SetPositionAndRotation(
			Vector3.Lerp(
				UICamera.transform.position,
				UICameraPoint.position,
				1 - Mathf.Exp(-uiSmoothing * Time.deltaTime)
			),
			Quaternion.Slerp(
				UICamera.transform.rotation,
				UICameraPoint.rotation,
				1 - Mathf.Exp(-uiSmoothing * Time.deltaTime)
			)
		);
	}

	protected void UpdatePositionToTrack()
	{
		if (MainCamera == null) return;

		PlayerController controller = GameManager.Instance.GetPlayerController();
		if (controller == null) return;
		Character pawn = controller.Pawn as Character;
		if (pawn == null) return;

		// Get the clostest point to the player on the line segment from cameraStart to cameraEnd
		Vector3 line = room.cameraEnd.position - room.cameraStart.position;
		Vector3 closestPoint =
			room.cameraStart.position
			+ Vector3.Project(pawn.cameraLookPoint.transform.position - room.cameraStart.position, line);
		float distanceSq = (room.cameraEnd.transform.position - room.cameraStart.transform.position).sqrMagnitude;
		if ((closestPoint - room.cameraStart.transform.position).sqrMagnitude > distanceSq)
		{
			closestPoint = room.cameraEnd.transform.position;
		} else if ((closestPoint - room.cameraEnd.transform.position).sqrMagnitude > distanceSq)
		{
			closestPoint = room.cameraStart.transform.position;
		}

		MainCamera.transform.position =
			Vector3.Lerp(MainCamera.transform.position, closestPoint, 1 - Mathf.Exp(-mainSmoothing * Time.deltaTime));

		// Calculate the percentage of the line that the player is on
		float distance = (closestPoint - room.cameraEnd.position).magnitude;
		float percent = distance / (room.cameraEnd.position - room.cameraStart.position).magnitude;

		// Interpolate the rotation between the start and end stops
		MainCamera.transform.rotation = Quaternion.Slerp(
			MainCamera.transform.rotation,
			Quaternion.Slerp(
				room.cameraStart.rotation,
				room.cameraEnd.rotation,
				percent
			),
			1 - Mathf.Exp(-mainSmoothing * Time.deltaTime)
		);
	}

	protected void UpdatePositionToMount()
	{
		if (MainCamera == null) return;

		MainCamera.transform.SetPositionAndRotation(
			Vector3.Lerp(
				MainCamera.transform.position,
				mountPoint.position,
				1 - Mathf.Exp(-mainSmoothing * Time.deltaTime)
			),
			Quaternion.Slerp(
				MainCamera.transform.rotation,
				mountPoint.rotation,
				1 - Mathf.Exp(-mainSmoothing * Time.deltaTime)
			)
		);
	}

	protected void Update()
	{
		UpdateUICameraPosition();
		switch (state)
		{
			case MainCameraState.None: break;
			case MainCameraState.Room: UpdatePositionToTrack(); break;
			case MainCameraState.Mounted: UpdatePositionToMount(); break;
		}
	}
}
