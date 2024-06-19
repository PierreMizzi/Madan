using System;
using UnityEngine;
using UnityEngine.UI;

public class ApplicationScreen : MonoBehaviour
{
	[SerializeField] protected ApplicationChannel m_applicationChannel;

	[SerializeField] protected ApplicationScreenType m_type;

	protected Canvas m_canvas;
	protected GraphicRaycaster m_graphicRaycaster;

	public ApplicationScreenType type => m_type;

	#region MonoBehaviour

	protected virtual void Awake()
	{
		m_canvas = GetComponent<Canvas>();
		m_graphicRaycaster = GetComponent<GraphicRaycaster>();

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
		m_graphicRaycaster.enabled = true;
	}

	public virtual void Hide()
	{
		m_canvas.enabled = false;
		m_graphicRaycaster.enabled = false;
	}


	protected virtual void CallbackDisplayScreen(ApplicationScreenType type, string[] options) { }

	protected virtual void CallbackAppDataLoaded() { }

}