using System;
using System.Collections.Generic;
using System.IO;
using PierreMizzi.Useful;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary> 
/// Static class containing all loaded informations 
/// </summary>
public static class Database
{
	public static List<WordData> wordDatas = new List<WordData>();
	public static List<SRSDeckData> srsDeckDatas = new List<SRSDeckData>();

	public static WordData PickRandomWord()
	{
		return UtilsClass.PickRandom(wordDatas);
	}

	#region Debug

	private static string saveFolder = "/Saves/";
	private static string fileName = "TempDataBase";
	private static string fileExtension = ".json";

	private static string directoryPath
	{
		get
		{
			return UnityEngine.Application.persistentDataPath + saveFolder;
		}
	}

	private static string path
	{
		get
		{
			return UnityEngine.Application.persistentDataPath + saveFolder + fileName + fileExtension;
		}
	}

	public static void GiveID()
	{
		TempDataBase dataBase = new TempDataBase();
		dataBase.wordDatas = wordDatas;

		int seed = 69420;
		Random.InitState(seed);

		foreach (WordData wordData in dataBase.wordDatas)
		{
			wordData.ID = Random.Range(0, 1000000);
		}

		string newWordDatas = JsonUtility.ToJson(dataBase, true);
		Directory.CreateDirectory(directoryPath);

		using StreamWriter streamWriter = new StreamWriter(path);
		streamWriter.Write(newWordDatas);
		Debug.Log("Path : " + path);
	}

	#endregion

}

[Serializable]
public class TempDataBase
{
	public List<WordData> wordDatas = new List<WordData>();
}

