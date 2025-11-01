using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[Header("Singleton References")]
	[SerializeField, Tooltip("The global audio mixer")]
	private AudioMixer audioMixer;
	[SerializeField, Tooltip("The audio source used for SFX")]
	private AudioSource SFXAudioSource;
	[SerializeField, Tooltip("The audio source used for music")]
	private AudioSource MusicAudioSource;

	public void PlayOneShot(AudioClip clip) => SFXAudioSource.PlayOneShot(clip);

	protected void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	protected void Start()
	{
		audioMixer.SetFloat("VolumeMaster", PlayerPrefs.GetFloat("VolumeMaster", 1f));
		audioMixer.SetFloat("VolumeMusic", PlayerPrefs.GetFloat("VolumeMusic", 1f));
		audioMixer.SetFloat("VolumeSFX", PlayerPrefs.GetFloat("VolumeSFX", 1f));
	}
}
