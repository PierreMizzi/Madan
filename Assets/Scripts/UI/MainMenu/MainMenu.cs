using System;
using TMPro;
using UnityEngine;

public class MainMenu : ApplicationScreen
{

	[SerializeField] private DailyWordUI m_dailyWordUI;

	[SerializeField] private TextMeshProUGUI m_userLevelLabel;

	protected override void Awake()
	{
		base.Awake();

		if (m_applicationChannel != null)
			m_applicationChannel.onRefreshDailyWord += CallbackRefreshDailyWorld;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		if (m_applicationChannel != null)
			m_applicationChannel.onRefreshDailyWord -= CallbackRefreshDailyWorld;
	}

	protected override void CallbackAppDataLoaded()
	{
		m_userLevelLabel.text = SaveManager.data.userLevel.ToString();
	}

	protected override void CallbackDisplayScreen(ApplicationScreenType type, string[] options)
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