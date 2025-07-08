using System;
using System.Collections.Generic;
using PierreMizzi.Useful.SaveSystem;

[Serializable]
public class ApplicationData : BaseApplicationData
{
	public int userLevel;

	public List<WordData> dailyWords;

	// 🟥 : Needs to use DayData morningDailyCheckDone instead
	public DailyCheckData dailyCheckMorning;
	public DailyCheckData dailyCheckNoon;
	public DailyCheckData dailyCheckEvening;

	public NotificationManagerSaveData notificationManagerData;

	public TrialData trial;

	public List<DayData> dayDatas = new List<DayData>();

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