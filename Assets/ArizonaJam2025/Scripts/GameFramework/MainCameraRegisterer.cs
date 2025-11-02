using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCameraRegisterer : MonoBehaviour
{
	public Camera thisCamera;

	protected void Start()
	{
		GameManager.Instance.GetCameraManager().MainCamera = thisCamera;
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (thisCamera == null) thisCamera = GetComponent<Camera>();
	}
#endif
}
