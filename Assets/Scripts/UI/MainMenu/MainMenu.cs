using System;
using TMPro;
using UnityEngine;

public class MainMenu : ApplicationScreen
{

	[SerializeField] private DailyWordUI m_dailyWordUI;

	[SerializeField] private TextMeshProUGUI m_userLevelLabel;

	private void Start()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onRefreshDailyWord += CallbackRefreshDailyWorld;
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
		}
	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onRefreshDailyWord -= CallbackRefreshDailyWorld;
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
		}
	}

	private void CallbackAppDataLoaded()
	{
		m_userLevelLabel.text = SaveManager.data.userLevel.ToString();
	}

	private void CallbackRefreshDailyWorld(WordData data)
	{
		m_dailyWordUI.Refresh(data);
	}

	public void OnClickDailyWordHistoryButton()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.DailyWordHistory);
	}

}