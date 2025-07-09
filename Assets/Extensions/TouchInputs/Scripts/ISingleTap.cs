using UnityEngine;

public interface ITappable
{
	public GameObject gameObject { get; }

	public void TriggerSingleTap(RaycastHit hitInfo)
	{
		
	}
	
	public void TriggerDoubleTap(Vector2 touchScreenPosition)
	{
		throw new System.NotImplementedException("Double tap not implemented for this ITappable interface.");
	}
}