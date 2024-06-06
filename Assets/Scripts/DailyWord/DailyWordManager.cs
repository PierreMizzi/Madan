using System;
using System.Collections.Generic;
using UnityEngine;

public class DailyWordManager
{

	public Stack<WordData> dailyWords = new Stack<WordData>();

	#region Daily timer

	public void ManageNewDailyWord()
	{
		if (NeedNewDailyWord())
			dailyWords.Push(PickNewDailyWord());

		LogDailyWords();
	}

	public bool NeedNewDailyWord()
	{
		if (dailyWords.Count == 0)
			return true;
		else
			return dailyWords.Peek().date.Day != DateTime.Now.Day;
	}

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
	public bool NeedNewDailyWord(DateTime dateWord, DateTime dateNow)
	{
		if (dailyWords.Count == 0)
			return true;
		else
			return dateWord.Day != dateNow.Day;
	}

	/// <summary> 
	/// Debug method 
	/// </summary>
	public void PopulateDailyWords(int amount)
	{
		DateTime tempDate = DateTime.Now;
		WordData tempWord;

		for (int i = 0; i < amount; i++)
		{
			tempWord = PickNewDailyWord();
			tempWord.date = tempDate;

			Debug.Log(tempWord);
			Debug.Log(tempWord.date);

			tempDate = tempDate.AddDays(-1);
			dailyWords.Push(tempWord);
		}
	}

	#endregion

	public WordData PickNewDailyWord()
	{
		WordData newWord = Database.PickRandomWord();

		if (IsAlreadyADailyWord(newWord))
			return PickNewDailyWord();
		else
			return newWord;
	}

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

	public void LogDailyWords()
	{
		foreach (WordData dailyWord in dailyWords)
		{
			Debug.Log(dailyWord.ToString());
		}
	}

}