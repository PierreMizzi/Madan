using System;
using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Useful.SaveSystem;
using UnityEngine;

/// <summary> 
/// Root class of the application
/// Handles DailyWordManager
/// With many debug tools
/// </summary>
public class Application : MonoBehaviour
{

	[SerializeField] private ApplicationChannel m_applicationChannel;
	[SerializeField] private DailyWordManager m_dailyWordManager;

	private void Awake()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onDisplayScreen += CallbackDisplayScreen;
	}

	private IEnumerator Start()
	{
		LoadDatabase();

		LoadAppData();

		yield return new WaitForSeconds(0.1f);

		// Uncomment to reset dailyWorld everytime you press play
		// ClearDailyWords();
		ManageDailyWord();


		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onDisplayScreen -= CallbackDisplayScreen;
	}


	[ContextMenu("Load")]
	private void LoadDatabase()
	{
		Parser.LoadDatabase();
		m_applicationChannel.onDatabaseLoaded.Invoke();
	}

	#region Daily Word

	// [SerializeField] private string dateWord = "May 21, 1996";
	[SerializeField] private string dateNow = "May 21, 2024";
	[SerializeField] private int dailyWordsPopulation = 10;

	public void ManageDailyWord()
	{
		m_dailyWordManager.ManageNewDailyWord();
		m_applicationChannel.onChangeDailyWords.Invoke();
		SaveAppData();
	}

	[ContextMenu("TryAddDailyWorld")]
	public void TryAddDailyWorld()
	{
		DateTime dateTime = DateTime.Parse(dateNow);
		m_dailyWordManager.ManageNewDailyWord(dateTime);
		m_applicationChannel.onChangeDailyWords.Invoke();
		SaveAppData();
	}

	#endregion

	#region Save

	[ContextMenu("Save")]
	public void SaveAppData()
	{
		m_dailyWordManager.Save();

		SaveManager.Save();
	}

	[ContextMenu("Load")]
	public void LoadAppData()
	{
		SaveManager.Load();
		m_dailyWordManager.Load();
		m_applicationChannel.onAppDataLoaded.Invoke();
	}

	public void ClearDailyWords()
	{
		m_dailyWordManager.ClearDailyWorlds();
		SaveManager.data.dailyWords.Clear();
		SaveManager.Save();
	}

	public void ResetDailyCheck()
	{
		// Resets all ApplicationData.dailyCheckData
		SaveManager.data.dailyCheckMorning.Reset();
		SaveManager.data.dailyCheckNoon.Reset();
		SaveManager.data.dailyCheckEvening.Reset();

		// Locks all ApplicationData.dailyWords except today's word
		int count = SaveManager.data.dailyWords.Count;
		for (int i = 0; i < count; i++)
		{
			if (i != count - 1)
				SaveManager.data.dailyWords[i].dateUnlocked = new DateTime();
		}

		SaveManager.Save();

		m_applicationChannel.onRefreshDailyCheck.Invoke();
	}

	public void LogApplicationData()
	{
		Debug.Log("### Application Data");
		SaveManager.Log();
	}

	public void LogWrittenData()
	{
		Debug.Log("### Written Data");
		BaseSaveManager.Log();
	}

	public void LogWordOfTheDay()
	{
		Debug.Log("### Word of the day");
		Debug.Log(m_dailyWordManager.wordOfTheDay);
	}

	public void LogDataBase()
	{
		Debug.Log("### Database");
		Debug.Log(Database.wordDatas);
	}

	#endregion

	#region Screens

	[SerializeField] private List<ApplicationScreen> m_screens;

	private void CallbackDisplayScreen(ApplicationScreenType type, string[] param = null)
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