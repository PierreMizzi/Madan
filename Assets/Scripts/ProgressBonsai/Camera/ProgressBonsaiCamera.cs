using PierreMizzi.Useful;
using UnityEngine;

public class ProgressBonsaiCamera : SphericalCameraController
{
	#region Behaviour

	[SerializeField] private float m_rotatingSpeed = 0.3f;

	[SerializeField] private bool m_isIdleRotation;

	
	#endregion

	#region MonoBehaviour

	protected void Start()
	{
		m_isIdleRotation = true;
	}

	protected override void Update()
	{
		if (m_isIdleRotation)
		{
			phi = Mathf.Sin(Time.time * m_rotatingSpeed) * 0.5f + 0.5f;
			phi = Mathf.Lerp(-60f, -120f, phi);
		}

		base.Update();
	}
		
	#endregion	
}