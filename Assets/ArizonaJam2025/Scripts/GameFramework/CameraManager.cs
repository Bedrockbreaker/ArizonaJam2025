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
		Pawn pawn = controller.Pawn;
		if (pawn == null) return;

		// Get the clostest point to the player on the line from cameraStart to cameraEnd
		Vector3 line = room.cameraEnd.position - room.cameraStart.position;
		Vector3 closestPoint = room.cameraStart.position + Vector3.Project(pawn.transform.position - room.cameraStart.position, line);
		MainCamera.transform.position = closestPoint;
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
