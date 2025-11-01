using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonChangeScene : MonoBehaviour
{
	public Button thisButton;
	public string sceneName;

	protected void Start()
	{
		if (thisButton == null) thisButton = GetComponent<Button>();
		thisButton.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (thisButton == null) thisButton = GetComponent<Button>();
	}
#endif
}
