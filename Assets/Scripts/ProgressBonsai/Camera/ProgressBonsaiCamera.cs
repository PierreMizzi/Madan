using PierreMizzi.Useful;
using UnityEngine;

public class ProgressBonsaiCamera : SphericalCameraController
{
	#region Behaviour

	[SerializeField] private float m_rotatingSpeed = 0.3f;
	
	#endregion

	#region MonoBehaviour

	protected override void Update()
	{
		phi = Time.time % 360 * m_rotatingSpeed;

		base.Update();
	}
		
	#endregion	
}