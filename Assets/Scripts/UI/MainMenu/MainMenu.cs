using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private ApplicationChannel m_applicationChannel;

	[SerializeField] private DailyWordUI m_dailyWordUI;

	private void Start()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onRefreshDailyWord += CallbackRefreshDailyWorld;
	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onRefreshDailyWord -= CallbackRefreshDailyWorld;
	}

	private void CallbackRefreshDailyWorld(WordData data)
	{
		m_dailyWordUI.Refresh(data);
	}

}