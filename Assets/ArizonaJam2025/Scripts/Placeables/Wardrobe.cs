using UnityEngine;

public class Wardrobe : MonoBehaviour
{
	public Transform cameraMountPoint;

	public void Interact()
	{
		PlayerController controller = GameManager.Instance.GetPlayerController();
		if (controller == null) return;
		Character pawn = controller.Pawn as Character;
		if (pawn == null) return;

		pawn.Hide();

		GameManager.Instance.GetCameraManager().UseMountPoint(cameraMountPoint);
	}
}
