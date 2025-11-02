using UnityEngine;

public class HiddenObject : MonoBehaviour
{
	protected void Start()
	{
		GameManager.Instance.RegisterHiddenObject(this);
	}

	protected void OnDestroy()
	{
		GameManager.Instance.UnregisterHiddenObject(this);
	}
}
