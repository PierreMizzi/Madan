using System;
using System.Collections.Generic;
using PierreMizzi.Useful.SaveSystem;

[Serializable]
public class ApplicationData : BaseApplicationData
{
	public List<WordData> dailyWords;

	public override string ToString()
	{
		string data = "List<WordData> dailyWorlds : ";

		int index = 1;
		for (int i = dailyWords.Count - 1; i >= 0; i--)
		{
			data += $"{index}. {dailyWords[i]} \n";
		}

		return data;
	}

}