public interface IInteractable
{
	// Called when player gets within interaction distance
	void Approach();
	// Called when player leaves interaction distance
	void StopApproach();

	void Interact();
}
