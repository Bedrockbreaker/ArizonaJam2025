using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour, IInteractable
{
	public Collider trigger;
	public Transform interactPromptPoint;
	public UnityEvent OnInteract;

	public virtual void Approach()
	{
		GameManager.Instance.PlaceInteractPrompt(interactPromptPoint);
	}

	public virtual void Interact()
	{
		OnInteract.Invoke();
	}

	public virtual void StopApproach()
	{
		GameManager.Instance.HideInteractPrompt();
	}

	protected void OnTriggerEnter(Collider other)
	{
		if (!other.TryGetComponent<Character>(out _)) return;
		Approach();
	}

	protected void OnTriggerExit(Collider other)
	{
		if (!other.TryGetComponent<Character>(out _)) return;
		StopApproach();
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (trigger == null) trigger = GetComponent<Collider>();
		trigger.isTrigger = true;
		trigger.includeLayers = 1 << LayerMask.NameToLayer("Player");
	}
#endif
}
