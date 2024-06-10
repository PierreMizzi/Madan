using System;
using UnityEngine;

public class ApplicationScreen : MonoBehaviour
{
	[SerializeField] protected ApplicationChannel m_applicationChannel;

	[SerializeField] protected ApplicationScreenType m_type;

	[SerializeField] protected Canvas m_canvas;

	public ApplicationScreenType type => m_type;

	public virtual void Display(params string[] options)
	{
		m_canvas.enabled = true;
	}

	internal void Hide()
	{
		m_canvas.enabled = false;
	}
}