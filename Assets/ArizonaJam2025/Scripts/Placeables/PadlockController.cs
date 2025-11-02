using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PadlockController : MonoBehaviour
{
	public List<int> code;
	public List<Padlock> locks;
	public UnityEvent OnOpen;

	private int index = 0;

	public void TryDigit(int digit)
	{
		if (code[index] == digit)
		{
			index++;
			if (index == code.Count) OnOpen.Invoke();
		}
		else
		{
			index = 0;
			foreach (Padlock padlock in locks) padlock.Reset();
		}
	}
}
