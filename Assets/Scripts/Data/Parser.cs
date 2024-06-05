using System.Collections.Generic;
using UnityEngine;

public static class Parser
{
	public static List<string> jsonPaths = new List<string>()
	{
		"Data/jlpt_n3_vocabulary"
	};

	public static List<WordData> wordDatas = new List<WordData>();

	public static void Load()
	{
		wordDatas.Clear();
		DictionaryData tempDictionary = new DictionaryData();

		foreach (string path in jsonPaths)
		{
			TextAsset tempJSON = Resources.Load<TextAsset>(path);
			Debug.Log(tempJSON.text);
			tempDictionary = JsonUtility.FromJson<DictionaryData>(tempJSON.text);
			Debug.Log(tempDictionary.wordDatas.Length);
			wordDatas.AddRange(tempDictionary.wordDatas);
		}
		Debug.Log(wordDatas[0].kanji);
	}
}