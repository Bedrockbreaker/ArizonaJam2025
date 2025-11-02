using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{
	public List<GameObject> objectsToDisable;
	public List<GameObject> objectsToEnable;

	public void DisableObject()
	{
		foreach (GameObject go in objectsToDisable) go.SetActive(false);
		foreach (GameObject go in objectsToEnable) go.SetActive(true);
		gameObject.SetActive(false);
	}
}
