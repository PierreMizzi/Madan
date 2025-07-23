using System;
using System.Collections;
using System.Collections.Generic;
using PierreMizzi.Useful.SaveSystem;
using UnityEngine;
using Random = UnityEngine.Random;

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

		UnityEngine.Application.targetFrameRate = 60;

		yield return new WaitForSeconds(0.1f);

		CheckDayData();

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

	#region DayData

	public void CheckDayData()
	{
		DateTime today = DateTime.Now;
		DayData dayData = SaveManager.data.dayDatas.Find(data => data.date.ToString("d") == today.Date.ToString("d"));

		if (dayData == null)
		{
			dayData = new DayData(today, PickRandomDailyWords());
			SaveManager.data.dayDatas.Add(dayData);
			Debug.Log($"New DayData created for {today}");
		}
		else
		{
			Debug.Log($"DayData already exists for {today}");
		}
	}

	public void ClearDayData()
	{
		DateTime today = DateTime.Now;
		DayData dayData = SaveManager.data.dayDatas.Find(data => data.date.ToString("d") == today.Date.ToString("d"));

		if (dayData != null)
		{
			SaveManager.data.dayDatas.Remove(dayData);
			SaveManager.Save();
			Debug.Log($"DayData reset for {today}");
		}
		else
		{
			Debug.LogWarning($"No DayData found for {today} to reset");
		}
	}

	private List<int> PickRandomDailyWords(int wordCount = 5)
	{
		List<int> newIDs = new List<int>();

		List<int> neverSeenIDs = GetNeverSeenWordDataIDs();

		for (int i = 0; i < wordCount; i++)
		{
			newIDs.Add(neverSeenIDs[Random.Range(0, neverSeenIDs.Count)]);
			neverSeenIDs.Remove(newIDs[i]);
		}

		return newIDs;
	}

	/// <summary>
	/// 
	///  </summary>
	private List<int> GetNeverSeenWordDataIDs()
	{
		//Creates a list of all the daily words previously seen by the user.
		List<int> seenIDs = new List<int>();
		foreach (DayData dayData in SaveManager.data.dayDatas)
		{
			seenIDs.AddRange(dayData.newDailyWordsIDs);
		}

		List<int> neverSeenIDs = new List<int>();
		foreach (WordData wordData in Database.wordDatas)
		{
			if (seenIDs.Contains(wordData.ID) == false)
			{
				// Word not seen yet, add it to potential new daily words
				neverSeenIDs.Add(wordData.ID);
			}
		}

		return neverSeenIDs;
	}

	public void LogDayDatas()
	{
		string log = $"Title";
		log += $"content : {0}\n";
		log += $"////////////////";
		Debug.Log(log);
	}

	#endregion

	#region Daily Word

	// [SerializeField] private string dateWord = "May 21, 1996";
	[SerializeField] private string dateNow = "May 21, 2024";
	[SerializeField] private int dailyWordsPopulation = 10;

	public void ManageDailyWord()
	{
		m_dailyWordManager?.ManageNewDailyWord();
		m_applicationChannel.onChangeDailyWords.Invoke();
		SaveAppData();
	}

	[ContextMenu("TryAddDailyWorld")]
	public void TryAddDailyWorld()
	{
		DateTime dateTime = DateTime.Parse(dateNow);
		m_dailyWordManager?.ManageNewDailyWord(dateTime);
		m_applicationChannel.onChangeDailyWords.Invoke();
		SaveAppData();
	}

	#endregion

	#region Save

	[ContextMenu("Save")]
	public void SaveAppData()
	{
		m_dailyWordManager?.Save();

		SaveManager.Save();
	}

	[ContextMenu("Load")]
	public void LoadAppData()
	{
		SaveManager.Load();

		m_dailyWordManager?.Load();
		m_applicationChannel.onAppDataLoaded.Invoke();
	}

	public void ClearDailyWords()
	{
		m_dailyWordManager?.ClearDailyWorlds();
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

	[ContextMenu("GiveID")]
	public void GiveID()
	{
		Database.GiveID();
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