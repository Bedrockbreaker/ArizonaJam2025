using UnityEngine;

public class WorldCanvasRegisterer : MonoBehaviour
{
	public RectTransform thisRectTransform;

	protected void Start()
	{
		GameManager.Instance.GetSeeingEye().worldPlane = thisRectTransform;
	}
}
