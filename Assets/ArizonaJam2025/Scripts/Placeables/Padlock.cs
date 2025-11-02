using UnityEngine;

public class Padlock : MonoBehaviour
{
	public PadlockController padlockController;
	public int digit;

	private bool bPressed = false;

	public void Press()
	{
		if (bPressed) return;
		bPressed = true;
		padlockController.TryDigit(digit);
	}

	public void Reset()
	{
		bPressed = false;
	}
}
