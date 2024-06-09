using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// 3 main features :
/// 
/// </summary>
public class DailyWordManager
{

	public Stack<WordData> dailyWords = new Stack<WordData>();

	public WordData wordOfTheDay { get { return dailyWords.Peek(); } }

	#region Daily timer

	/// <summary> 
	/// Whenever called, checks if we need to add a new daily word
	/// </summary>
	public void ManageNewDailyWord()
	{
		if (NeedNewDailyWord())
			dailyWords.Push(PickNewDailyWord());

		// LogDailyWords();
	}

	/// <summary> 
	/// 
	/// </summary>
	public void ManageNewDailyWord(DateTime dateNow)
	{
		if (NeedNewDailyWord(dateNow))
			dailyWords.Push(PickNewDailyWord());

		// LogDailyWords();
	}

	/// <summary> 
	/// Answer the question : "Is it a new day ?"
	/// </summary>
	public bool NeedNewDailyWord()
	{
		if (dailyWords.Count == 0)
			return true;
		else
			return dailyWords.Peek().date.Day != DateTime.Now.Day;
	}

	#endregion

	/// <summary> 
	/// Pick a WordData that is not already inside dailyWords
	/// Recursive function
	/// </summary>
	public WordData PickNewDailyWord()
	{
		WordData newWord = Database.PickRandomWord();
		newWord.date = DateTime.Now;

		if (IsAlreadyADailyWord(newWord))
			return PickNewDailyWord();
		else
			return newWord;
	}

	/// <summary> 
	/// Answer the question "Is it already inside dailyWords"
	/// </summary>
	public bool IsAlreadyADailyWord(WordData pickedWord)
	{
		if (dailyWords.Count == 0)
			return false;

		foreach (WordData dailyWord in dailyWords)
		{
			if (dailyWord.kanji == pickedWord.kanji)
				return true;
		}
		return false;
	}

	public void ClearDailyWorlds()
	{
		dailyWords.Clear();
	}

	#region SaveDailyWords

	public void Save()
	{
		SaveManager.data.dailyWords.Clear();
		for (int i = 0; i < dailyWords.Count; i++)
			SaveManager.data.dailyWords.Add(new WordData());

		int index = dailyWords.Count;

		foreach (WordData dailyWord in dailyWords)
		{
			SaveManager.data.dailyWords[index - 1] = dailyWord;
			index--;
		}
	}

	public void Load()
	{
		if (SaveManager.data.dailyWords == null)
			SaveManager.data.dailyWords = new List<WordData>();
		else
		{
			foreach (WordData wordData in SaveManager.data.dailyWords)
				dailyWords.Push(wordData);
		}


		// LogDailyWords();
	}

	#endregion

	#region Debug



	/// <summary> 
	/// Debug method
	/// </summary>
	public bool NeedNewDailyWord(DateTime dateNow)
	{
		if (dailyWords.Count == 0)
			return true;
		else
			return dailyWords.Peek().date.Day != dateNow.Day;
	}

	/// <summary> 
	/// Debug method 
	/// </summary>
	// public void PopulateDailyWords(int amount)
	// {
	// 	DateTime tempDate = DateTime.Now;
	// 	WordData tempWord;

	// 	for (int i = 0; i < amount; i++)
	// 	{
	// 		tempWord = PickNewDailyWord();
	// 		tempWord.date = tempDate;

	// 		tempDate = tempDate.AddDays(-1);
	// 		dailyWords.Push(tempWord);
	// 	}

	// 	LogDailyWords();
	// }


	public void LogDailyWords()
	{
		string str = "#### Daily Words #### \n";

		int index = 1;

		foreach (WordData dailyWord in dailyWords)
		{
			str += $"{index}. {dailyWord.ToString()} \n";
			index++;
		}

		// Debug.Log(str);
	}

	#endregion

}