using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour
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
	[SerializeField, Tooltip("End position of the UI camera (i.e. for normal gameplay)")]
	private Transform UICameraEndPoint;
	[SerializeField, Tooltip("Canvas for the interact prompt")]
	private Canvas interactPrompt;
	[SerializeField, Tooltip("Animator for the interact prompt")]
	private Animator interactPromptAnimator;
	[SerializeField, Tooltip("The seeing eye")]
	private SeeingEye seeingEye;
    [SerializeField, Tooltip("Canvas")]
    private Canvas gameCanvas;

    private PlayerController playerController;
	private Transform interactPromptPoint;
	private readonly List<HiddenObject> hiddenObjects = new();

	public void PlayOneShot(AudioClip clip) => SFXAudioSource.PlayOneShot(clip);

	public void StartGame()
	{
		cameraManager.UICamera.GetComponent<AudioListener>().enabled = false;
		cameraManager.UICameraPoint = UICameraEndPoint;
		// SceneManager.LoadScene("S_Game", LoadSceneMode.Additive);
		gameCanvas.gameObject.SetActive(true);
		LoadScene("S_RoomTest");
	}

	public void LoadScene(string scene)
	{
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
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

	public void PlaceInteractPrompt(Transform transformIn)
	{
		interactPromptPoint = transformIn;
		// Rotate 90 degrees to face player
		interactPrompt.transform.rotation = transformIn.rotation * Quaternion.Euler(0f, -90f, 0f);
		interactPromptAnimator.SetBool("bShown", true);
	}

	public void HideInteractPrompt()
	{
		interactPromptPoint = null;
		interactPromptAnimator.SetBool("bShown", false);
	}

	public void RegisterHiddenObject(HiddenObject hiddenObject) => hiddenObjects.Add(hiddenObject);

	public void UnregisterHiddenObject(HiddenObject hiddenObject) => hiddenObjects.Remove(hiddenObject);

	public List<HiddenObject> GetHiddenObjects() => hiddenObjects;

	public SeeingEye GetSeeingEye() => seeingEye;

	private void Awake()
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

	private void Start()
	{
		audioMixer.SetFloat("VolumeMaster", PlayerPrefs.GetFloat("VolumeMaster", 1f));
		audioMixer.SetFloat("VolumeMusic", PlayerPrefs.GetFloat("VolumeMusic", 1f));
		audioMixer.SetFloat("VolumeSFX", PlayerPrefs.GetFloat("VolumeSFX", 1f));
	}

	private void Update()
	{
		if (interactPromptPoint == null) return;
		interactPrompt.transform.position = interactPromptPoint.position;
	}
}
