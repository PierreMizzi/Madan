using System;
using UnityEngine;
using UnityEngine.UI;

public class Application : MonoBehaviour
{

	private DailyWordManager dailyWordManager;

	

	private void Start()
	{
		Load();

		dailyWordManager = new DailyWordManager();
	}

	[ContextMenu("Load")]
	private void Load()
	{
		Parser.Load();
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

	[SerializeField] private string dateWord = "May 21, 1996";
	[SerializeField] private string dateNow = "May 21, 2024";
	[SerializeField] private int dailyWordsPopulation = 10;

	[ContextMenu("Test need daily word")]
	public void TestNeedDailyWord()
	{
		DateTime dateTimeWord = DateTime.Parse(dateWord);
		DateTime dateTimeNow = DateTime.Parse(dateNow);

		Debug.Log("Need new daily word ? " + dailyWordManager.NeedNewDailyWord(dateTimeWord, dateTimeNow));
	}

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

	#endregion
}