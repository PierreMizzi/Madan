using System.Collections.Generic;
using PierreMizzi.Useful;

public static class Database
{
	public static List<WordData> wordDatas = new List<WordData>();

	public static WordData PickRandomWord()
	{
		return UtilsClass.PickRandom(wordDatas);
	}

}