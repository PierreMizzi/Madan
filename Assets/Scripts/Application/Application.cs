using System;
using UnityEngine;

/// <summary> 
/// Root class of the application
/// Handles DailyWordManager
/// With many debug tools
/// </summary>
public class Application : MonoBehaviour
{

	private DailyWordManager dailyWordManager;

	private void Start()
	{
		LoadDatabase();

		dailyWordManager = new DailyWordManager();

		LoadSave();
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

	// [ContextMenu("Test need daily word")]
	// public void TestNeedDailyWord()
	// {
	// 	DateTime dateTimeWord = DateTime.Parse(dateWord);
	// 	DateTime dateTimeNow = DateTime.Parse(dateNow);

	// 	Debug.Log("Need new daily word ? " + dailyWordManager.NeedNewDailyWord(dateTimeWord, dateTimeNow));
	// }

	// [ContextMenu("ManageNewDailyWord")]
	// public void ManageNewDailyWord()
	// {
	// 	dailyWordManager.ManageNewDailyWord();
	// }

	[ContextMenu("PopulateDailyWords")]
	public void PopulateDailyWords()
	{
		dailyWordManager.PopulateDailyWords(dailyWordsPopulation);
	}

	[ContextMenu("AddDailyWorld")]
	public void AddDailyWord()
	{
		DateTime dateTime = DateTime.Parse(dateNow);
		dailyWordManager.ManageNewDailyWord(dateTime);
		Save();
	}

	[ContextMenu("ClearDailyWords")]
	public void ClearDailyWords()
	{
		dailyWordManager.ClearDailyWorlds();
		SaveManager.Save();
	}

	#endregion

	#region Save

	[ContextMenu("Save")]
	public void Save()
	{
		dailyWordManager.Save();

		SaveManager.Save();
	}

	[ContextMenu("Load")]
	public void LoadSave()
	{
		SaveManager.Load();

		dailyWordManager.Load();

		LogSave();
	}

	[ContextMenu("Log")]
	public void LogSave()
	{
		SaveManager.Log();
	}

	#endregion

}