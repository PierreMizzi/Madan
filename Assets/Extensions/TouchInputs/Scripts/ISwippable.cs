using UnityEngine;

public interface ISwippable
{
	public GameObject gameObject { get; }

	public void TriggerSwipe(RaycastHit hitInfo, Vector2 swipeDirection)
	{
		// Default implementation can be empty or provide basic functionality
		// Debug.Log($"Swipe detected in direction: {swipeDirection}");
	}
}