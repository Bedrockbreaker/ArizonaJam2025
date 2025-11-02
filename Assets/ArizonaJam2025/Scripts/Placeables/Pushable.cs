using UnityEngine;

public class Pushable : MonoBehaviour
{
	public void TogglePushing()
	{
		PlayerController controller = GameManager.Instance.GetPlayerController();
		if (controller == null) return;
		Character pawn = controller.Pawn as Character;
		if (pawn == null) return;
		pawn.Carry(transform);
	}
}
