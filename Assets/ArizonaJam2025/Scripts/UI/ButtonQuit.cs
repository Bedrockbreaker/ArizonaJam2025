using UnityEngine;
using UnityEngine.UI;

public class ButtonQuit : Button
{
	protected override void Start()
	{
        base.Start();
        onClick.AddListener(Quit);
	}

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
