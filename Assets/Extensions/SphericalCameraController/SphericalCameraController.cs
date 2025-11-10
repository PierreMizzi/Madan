using UnityEngine;

namespace PierreMizzi.Useful
{
	[ExecuteInEditMode]
	public class SphericalCameraController : MonoBehaviour
	{
		#region Behaviour

		public bool isEnabled = true;

		[SerializeField] protected Transform m_origin;
		protected Vector3 Origin => m_origin != null ? m_origin.position : Vector3.zero;
		[SerializeField] protected Transform m_target;

		[SerializeField] protected float radius; // Distance from origin
		[SerializeField] protected float phi; // angle on the x / y axis. 0 is at (1, 0, 0) 0 to 180*

		[Range(-89.5f, 89.5f)]
		[SerializeField] protected float rho; // angle on the x / z axis. 0 is at (1, 0, 0). 90* to -90* degree

		protected virtual void UpdatePosition()
		{
			Vector3 phiVector = Quaternion.Euler(0, -phi, 0) * Vector3.right;
			rho = Mathf.Clamp(rho, -89.5f, 89.5f);
			Quaternion rhoQuaternion = Quaternion.AngleAxis(rho, Vector3.Cross(phiVector, Vector3.up));
			phiVector = rhoQuaternion * phiVector;
			transform.position = m_origin.position + phiVector * radius;
		}

		protected virtual void UpdateRotation()
		{
			if (m_target == null)
			{
				transform.forward = -transform.position.normalized;
			}
			else
			{
				transform.LookAt(m_target);
			}
		}

		#endregion


		#region MonoBehaviour

		protected virtual void Update()
		{
			if (isEnabled)
			{
				UpdatePosition();
				UpdateRotation();
			}
		}

		#endregion
	}
}
