using UnityEngine;
using UnityEngine.UI;

public class ButtonPlay : MonoBehaviour
{
    public Button thisButton;
    public Canvas UICanvas;

    public void Play()
    {
        UICanvas.enabled = false;
        GameManager.Instance.StartGame();
    }

    protected void Start()
    {
        thisButton.onClick.AddListener(Play);
    }

#if UNITY_EDITOR
    protected void Reset()
	{
		if (thisButton == null) thisButton = GetComponent<Button>();
	}
#endif
}
