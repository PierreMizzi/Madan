using System;
using System.Collections;
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
	private DailyWordManager m_dailyWordManager;

	private IEnumerator Start()
	{
		LoadDatabase();


		m_dailyWordManager = new DailyWordManager();

		LoadSave();

		yield return new WaitForSeconds(0.1f);

		// Uncomment to reset dailyWorld everytime you press play
		// ClearDailyWords();
		ManageDailyWord();
	}

	[ContextMenu("Load")]
	private void LoadDatabase()
	{
		Parser.LoadDatabase();
	}

	[ContextMenu("label")]
	private void SerializeDateTime()
	{
		string test = JsonUtility.ToJson(DateTime.Today);
		Debug.Log(test);
		Debug.Log(DateTime.Now);
		Debug.Log(DateTime.Today);
	}

	#region Daily Word

	// [SerializeField] private string dateWord = "May 21, 1996";
	[SerializeField] private string dateNow = "May 21, 2024";
	[SerializeField] private int dailyWordsPopulation = 10;

	public void ManageDailyWord()
	{
		DateTime dateTime = DateTime.Parse(dateNow);
		m_dailyWordManager.ManageNewDailyWord();
		m_applicationChannel.onRefreshDailyWord.Invoke(m_dailyWordManager.wordOfTheDay);
		Save();
	}

	[ContextMenu("TryAddDailyWorld")]
	public void TryAddDailyWorld()
	{
		DateTime dateTime = DateTime.Parse(dateNow);
		m_dailyWordManager.ManageNewDailyWord(dateTime);
		m_applicationChannel.onRefreshDailyWord.Invoke(m_dailyWordManager.wordOfTheDay);
		Save();
	}

	[ContextMenu("ClearDailyWords")]
	public void ClearDailyWords()
	{
		m_dailyWordManager.ClearDailyWorlds();
		SaveManager.Save();
	}

	#endregion

	#region Save

	[ContextMenu("Save")]
	public void Save()
	{
		m_dailyWordManager.Save();

		SaveManager.Save();
	}

	[ContextMenu("Load")]
	public void LoadSave()
	{
		SaveManager.Load();

		m_dailyWordManager.Load();
		m_applicationChannel.onRefreshDailyWord.Invoke(m_dailyWordManager.wordOfTheDay);
	}

	public void LogApplicationData()
	{
		Debug.Log("### Application Data");
		Debug.Log(SaveManager.data.ToString());
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

}