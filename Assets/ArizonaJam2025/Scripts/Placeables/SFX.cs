using UnityEngine;

public class SFX : MonoBehaviour
{
	public AudioClip sound;

	public void PlaySound()
	{
		GameManager.Instance.PlayOneShot(sound);
	}
}
