using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class WeightedButton : MonoBehaviour
{
	public Collider trigger;
	public UnityEvent OnPressed;
	public UnityEvent OnReleased;

	private int triggers = 0;

	protected void OnTriggerEnter(Collider other)
	{
		if (!other.TryGetComponent<Pushable>(out _) && !other.TryGetComponent<Character>(out _)) return;
		if (triggers == 0) OnPressed.Invoke();
		triggers++;
	}

	protected void OnTriggerExit(Collider other)
	{
		if (!other.TryGetComponent<Pushable>(out _) && !other.TryGetComponent<Character>(out _)) return;
		triggers--;
		if (triggers == 0) OnReleased.Invoke();
	}

#if UNITY_EDITOR
	protected void Reset()
	{
		if (trigger == null) trigger = GetComponent<Collider>();
		if (trigger == null) throw new System.Exception("WeightedButton must have a Collider");
		trigger.isTrigger = true;
	}
#endif
}
