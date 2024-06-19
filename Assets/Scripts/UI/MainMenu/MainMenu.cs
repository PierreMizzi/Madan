using System;
using TMPro;
using UnityEngine;

public class MainMenu : ApplicationScreen
{

	[SerializeField] private DailyWordUI m_dailyWordUI;

	[SerializeField] private TextMeshProUGUI m_userLevelLabel;



	protected override void CallbackAppDataLoaded()
	{
		m_userLevelLabel.text = SaveManager.data.userLevel.ToString();
	}

	protected override void CallbackDisplayScreen(ApplicationScreenType type, string[] options)
	{
		m_userLevelLabel.text = SaveManager.data.userLevel.ToString();
	}

	public void OnClickDailyWordHistoryButton()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.DailyWordHistory);
	}

}