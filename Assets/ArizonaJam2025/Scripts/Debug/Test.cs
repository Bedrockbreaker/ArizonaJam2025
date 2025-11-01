using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
	public AudioClip sound;

	public InputActionReference testAction;

	public void PlaySound(InputAction.CallbackContext context)
	{
		GameManager.Instance.PlayOneShot(sound);
	}

	protected void Start()
	{
		Debug.Log("Test");
		if (testAction == null) testAction = InputActionReference.Create(InputSystem.actions.FindAction("Jump"));
		testAction.action.performed += PlaySound;
	}

	protected void OnDestroy()
	{
		testAction.action.performed -= PlaySound;
	}
}
