using UnityEngine;
using UnityEngine.UI;

public class ButtonPlay : Button
{
    public void Play()
    {
        Debug.Log("Play");
        // gamemanager.play()
    }

    protected override void Start()
	{
        base.Start();
        onClick.AddListener(Play);
	}
}
