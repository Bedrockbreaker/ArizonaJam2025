using UnityEngine;
using UnityEngine.UI;

public class ButtonDisableObjects : MonoBehaviour
{
	public Button thisButton;
	public GameObject[] gameObjectsToDisable;
	public GameObject[] gameObjectsToEnable;

	public void DisableObjects()
	{
		foreach (GameObject gameObject in gameObjectsToDisable) gameObject.SetActive(false);
		foreach (GameObject gameObject in gameObjectsToEnable) gameObject.SetActive(true);
	}

	protected void Start()
	{
		if (thisButton == null) thisButton = GetComponent<Button>();
		thisButton.onClick.AddListener(() => DisableObjects());
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (thisButton == null) thisButton = GetComponent<Button>();
	}
#endif
}
