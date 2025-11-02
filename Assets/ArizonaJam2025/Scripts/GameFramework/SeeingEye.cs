using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeeingEye : MonoBehaviour
{
	public RectTransform tvPlane;
	public RectTransform worldPlane;
	public float revealConeDegrees = 33.3f;
	public float gamepadSensitivity = 1.0f;

	private Vector2 tvCanvasPercent;

	// private Plane worldPlane = new(-Vector3.right, 0f);

	/**
	 * <summary>
	 * main menu UI Camera
	 * 	-> Project to main menu Tv Screen
	 * 	-> Map to World space canvas
	 * 	-> Project to World space
	 * 	-> reveal objects in a cone
	 * </summary>
	 * <param name="context"></param>
	 */
	public void Look(InputAction.CallbackContext context)
	{
		if (context.control.device is Pointer pointer)
		{
			// Raycast from camera to projected plane (treat the world-space canvas as a plane)
			Camera UICamera = GameManager.Instance.GetCameraManager().UICamera;
			Ray ray = UICamera.ScreenPointToRay(pointer.position.ReadValue());

			Plane plane = new(tvPlane.transform.forward, tvPlane.transform.position);

			if (plane.Raycast(ray, out float enter))
			{
				Vector2 tvCanvasPosition = tvPlane.transform.InverseTransformPoint(ray.GetPoint(enter));

				// test if position is inside the canvas
				if (
					tvCanvasPosition.x >= -tvPlane.rect.width * 0.5f && tvCanvasPosition.x <= tvPlane.rect.width * 0.5f
					&& tvCanvasPosition.y >= -tvPlane.rect.height && tvCanvasPosition.y <= tvPlane.rect.height * 0.5f
				)
				{
					// convert tvCanvasPosition to percent
					tvCanvasPercent.x = (tvCanvasPosition.x + tvPlane.rect.width * 0.5f) / tvPlane.rect.width;
					tvCanvasPercent.y = (tvCanvasPosition.y + tvPlane.rect.height * 0.5f) / tvPlane.rect.height;
				}
				else
				{
					return;
				}
			}
			else
			{
				Debug.LogError("Failed to raycast to tv screen.");
			}
		}
		else
		{
			// Locally move tvCanvasPercent around using the joystick
			tvCanvasPercent += context.ReadValue<Vector2>() * gamepadSensitivity;
			tvCanvasPercent.x = Mathf.Clamp01(tvCanvasPercent.x);
			tvCanvasPercent.y = Mathf.Clamp01(tvCanvasPercent.y);
		}

		// Convert tvCanvasPercent to world space position

		Vector3 worldCanvasPosition = worldPlane.transform.TransformPoint(new Vector3(
			worldPlane.rect.width * tvCanvasPercent.x - worldPlane.rect.width * 0.5f,
			worldPlane.rect.height * tvCanvasPercent.y - worldPlane.rect.height * 0.5f,
			0f
		));

		Camera mainCamera = GameManager.Instance.GetCameraManager().MainCamera;
		Vector3 worldDirection = worldCanvasPosition - mainCamera.transform.position;

		// If object is in cone (vertical is oriented main camera -> worldCanvasPosition), show it
		// If object is outside cone, hide it

		GameManager.Instance.GetHiddenObjects().ForEach(hiddenObject =>
		{
			Vector3 hiddenObjectDirection = hiddenObject.transform.position - mainCamera.transform.position;

			float angle = Vector3.Angle(worldDirection, hiddenObjectDirection);

			if (angle <= revealConeDegrees)
			{
				hiddenObject.gameObject.SetActive(true);
			}
			else
			{
				hiddenObject.gameObject.SetActive(false);
			}
		});
	}
}
