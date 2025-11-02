using UnityEngine;
using UnityEngine.SceneManagement;

public enum MainCameraState { None, Room, Mounted }

public class CameraManager : MonoBehaviour
{
	public Camera MainCamera;
	public Camera UICamera;
	public RenderTexture tvScreen;

	public float mainSmoothing = 0.5f;

	private MainCameraState state = MainCameraState.None;
	private RoomBox room;

	public void UseRoomTrack(RoomBox room)
	{
		this.room = room;
		state = MainCameraState.Room;
	}

	protected void UpdatePositionToTrack()
	{
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
		float distance = (MainCamera.transform.position - closestPoint).magnitude;
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
		
	}

	protected void Start()
	{
		// If starting from the main menu, we additvely load scenes. If we start from the game scene, we don't.
		if (SceneManager.loadedSceneCount > 1)
		{
			MainCamera.targetTexture = tvScreen;
		}
	}

	protected void Update()
	{
		switch (state)
		{
			case MainCameraState.None: break;
			case MainCameraState.Room: UpdatePositionToTrack(); break;
			case MainCameraState.Mounted: UpdatePositionToMount(); break;
		}
	}
}
