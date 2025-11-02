using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Trigger : MonoBehaviour
{
	public Collider trigger;
	public bool bDisableSelfOnEnter = false;
	public UnityEvent OnTriggerEnterEvent;
	public UnityEvent OnTriggerExitEvent;

	protected void OnTriggerEnter(Collider other)
	{
		if (!TryGetComponent(out Character _)) return;
		OnTriggerEnterEvent.Invoke();
		if (bDisableSelfOnEnter) gameObject.SetActive(false);
	}

	protected void OnTriggerExit(Collider other)
	{
		if (!TryGetComponent(out Character _)) return;
		OnTriggerExitEvent.Invoke();
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (trigger == null) trigger = GetComponent<Collider>();
		trigger.isTrigger = true;
	}
#endif
}
