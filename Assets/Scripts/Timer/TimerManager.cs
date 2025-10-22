using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	#region Behaviour

	[SerializeField] private ApplicationChannel m_applicationChannel;

	#endregion

	#region MonoBehaviour

	protected virtual void Awake()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
	}

	protected virtual void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;
	}

	private void CallbackAppDataLoaded()
	{

	}

	#endregion

	#region Save Manager
	
	public void SavePomodoro()
	{
		
	}
		
	#endregion
}