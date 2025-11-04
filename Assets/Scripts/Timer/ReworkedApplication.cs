using System.Collections.Generic;
using UnityEngine;

public class ReworkedApplication : MonoBehaviour
{
	[SerializeField] protected ApplicationChannel m_applicationChannel;

	#region MonoBehaviour

	protected virtual void Awake()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onDisplayScreen += CallbackDisplayScreen;
	}

	protected virtual void Start()
	{
		LoadAppData();
	}

	protected virtual void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onDisplayScreen -= CallbackDisplayScreen;
	}


	#endregion

	#region Save Manager

	public void LoadAppData()
	{
		SaveManager.Load();
		m_applicationChannel.onAppDataLoaded.Invoke();
	}

	#endregion

	#region Screens

	[SerializeField] protected List<ApplicationScreen> m_screens;

	protected virtual void CallbackDisplayScreen(ApplicationScreenType type, string[] param = null)
	{
		foreach (ApplicationScreen screen in m_screens)
		{
			if (screen.type == type)
				screen.Display(param);
			else
				screen.Hide();
		}
	}

	#endregion

}
