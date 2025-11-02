using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
	[SerializeField, Tooltip("The camera manager")]
	private CameraManager cameraManager;

	private PlayerController playerController;

	public void PlayOneShot(AudioClip clip) => SFXAudioSource.PlayOneShot(clip);

	public void StartGame()
	{
		SceneManager.LoadScene("S_Game");
	}

	public CameraManager GetCameraManager() => cameraManager;

	public void RegisterController(PlayerController controller)
	{
		playerController = controller;
	}

	public void UnregisterController()
	{
		playerController = null;
	}

	public PlayerController GetPlayerController() => playerController;

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
