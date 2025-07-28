using System;
using System.Collections.Generic;
using UnityEngine;



/// <summary> 
/// Parses data from a file into the application's database
/// </summary>
public static class Parser
{
	public static void LoadDatabase()
	{
		LoadWordDatas();
		LoadSRSDecks();
	}

	#region Word Data

	public static List<string> jsonPaths = new List<string>()
	{
		"Data/jlpt_n3_vocabulary"
	};

	[Serializable]
	public class WordDataObject
	{
		public WordData[] wordDatas;
	}

	public static void LoadWordDatas()
	{
		Database.wordDatas.Clear();
		WordDataObject tempDictionary = new WordDataObject();

		foreach (string path in jsonPaths)
		{
			TextAsset tempJSON = Resources.Load<TextAsset>(path);
			tempDictionary = JsonUtility.FromJson<WordDataObject>(tempJSON.text);
			Database.wordDatas.AddRange(tempDictionary.wordDatas);

			foreach (WordData wordData in tempDictionary.wordDatas)
			{
				Database.wordDatass.Add(wordData.ID, wordData);
			}
		}
	}

	#endregion

	#region SRS Decks

	public static List<string> srsDeckPaths = new List<string>()
	{
		"Data/SRSDecks"
	};

	[Serializable]
	public class SRSDecksObject
	{
		public SRSDeckData[] deckDatas;
	}

	public static void LoadSRSDecks()
	{
		Database.srsDeckDatas.Clear();
		SRSDecksObject tempObject = new SRSDecksObject();

		foreach (string path in srsDeckPaths)
		{
			TextAsset tempJSON = Resources.Load<TextAsset>(path);
			tempObject = JsonUtility.FromJson<SRSDecksObject>(tempJSON.text);
			Database.srsDeckDatas.AddRange(tempObject.deckDatas);
		}
	}

	#endregion


}