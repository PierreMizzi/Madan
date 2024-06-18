using System;
using UnityEngine;

public class ApplicationScreen : MonoBehaviour
{
	[SerializeField] protected ApplicationChannel m_applicationChannel;

	[SerializeField] protected ApplicationScreenType m_type;

	[SerializeField] protected Canvas m_canvas;

	public ApplicationScreenType type => m_type;

	#region MonoBehaviour

	protected virtual void Awake()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onDisplayScreen += CallbackDisplayScreen;
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
		}
	}

	protected virtual void OnDestroy()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onDisplayScreen -= CallbackDisplayScreen;
			m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;
		}
	}

	#endregion

	public virtual void Display(params string[] options)
	{
		m_canvas.enabled = true;
	}

	public virtual void Hide()
	{
		m_canvas.enabled = false;
	}


	protected virtual void CallbackDisplayScreen(ApplicationScreenType type, string[] options) { }

	protected virtual void CallbackAppDataLoaded() { }

}