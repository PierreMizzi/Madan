using System;
using System.Collections.Generic;
using PierreMizzi.Useful.SaveSystem;

[Serializable]
public class ApplicationData : BaseApplicationData
{
	public int userLevel;

	// 🟥 : Might be decrepated
	public List<WordData> dailyWords;

	// 🟥 : Needs to use DayData morningDailyCheckDone instead
	public DailyCheckData dailyCheckMorning;
	public DailyCheckData dailyCheckNoon;
	public DailyCheckData dailyCheckEvening;

	public TrialData trial;

	// Notification
	public NotificationManagerSaveData notificationManagerData;

	// Calendar
	public List<DayData> dayDatas = new List<DayData>();

	// SRS
	public SRSSaveData srsSaveData;

	// Timer
	public TimerData timerData;

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

		data += $"SRS : \n";
		data += srsSaveData.ToString();

		return data;
	}

}