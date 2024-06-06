using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Parses data from a file into the application's database
/// </summary>
public static class Parser
{
	public static List<string> jsonPaths = new List<string>()
	{
		"Data/jlpt_n3_vocabulary"
	};

	public static void LoadDatabase()
	{
		Database.wordDatas.Clear();
		DictionaryData tempDictionary = new DictionaryData();

		foreach (string path in jsonPaths)
		{
			TextAsset tempJSON = Resources.Load<TextAsset>(path);
			tempDictionary = JsonUtility.FromJson<DictionaryData>(tempJSON.text);
			Database.wordDatas.AddRange(tempDictionary.wordDatas);
		}
	}

	public static void SaveDailyWords(Stack<WordData> wordDatas)
	{

	}
}