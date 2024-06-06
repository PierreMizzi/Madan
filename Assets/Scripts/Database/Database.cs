using System.Collections.Generic;
using PierreMizzi.Useful;

/// <summary> 
/// Static class containing all loaded informations 
/// </summary>
public static class Database
{
	public static List<WordData> wordDatas = new List<WordData>();

	public static WordData PickRandomWord()
	{
		return UtilsClass.PickRandom(wordDatas);
	}

}