using System;
using System.Collections.Generic;
using PierreMizzi.Useful.SaveSystem;
using UnityEngine.Rendering;

[Serializable]
public class ApplicationData : BaseApplicationData
{
	public List<WordData> dailyWords;

	public DailyCheckData dailyCheckMorning;
	public DailyCheckData dailyCheckNoon;
	public DailyCheckData dailyCheckEvening;

	public override string ToString()
	{
		string data = "List<WordData> dailyWorlds : \n";

		int index = 1;
		for (int i = dailyWords.Count - 1; i >= 0; i--)
		{
			data += $"{index}. {dailyWords[i]} \n";
			index++;
		}

		data += $"dailyCheckMorning : \n";
		data += dailyCheckMorning.ToString();

		data += $"dailyCheckNoon : \n";
		data += dailyCheckNoon.ToString();

		data += $"dailyCheckEvening : \n";
		data += dailyCheckEvening.ToString();

		return data;
	}

}