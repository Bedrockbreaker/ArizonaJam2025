using UnityEngine.UI;

public class ButtonPlay : Button
{
    public void Play()
    {
        GameManager.Instance.StartGame();
    }

    protected override void Start()
	{
        base.Start();
        onClick.AddListener(Play);
	}
}
