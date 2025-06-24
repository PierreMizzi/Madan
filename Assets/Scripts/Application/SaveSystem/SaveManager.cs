using Newtonsoft.Json;
using PierreMizzi.Useful.SaveSystem;
using UnityEngine;

/// <summary> 
/// Save dynamic data of the game
/// Different from static data inside Database
/// </summary>
public static class SaveManager
{
	public static ApplicationData data;

	public static void Load()
	{
		data = BaseSaveManager.Load<ApplicationData>();

		bool needSave = false;

		if (data.dailyCheckMorning == null)
		{
			data.dailyCheckMorning = new DailyCheckData(DailyCheckType.Morning);
			data.dailyCheckNoon = new DailyCheckData(DailyCheckType.Noon);
			data.dailyCheckEvening = new DailyCheckData(DailyCheckType.Evening);
			needSave = true;
		}

		if (data.notificationManagerData == null)
		{
			data.notificationManagerData = new NotificationManagerSaveData();
			needSave = true;
		}

		if (data.trial == null)
		{
			data.trial = new TrialData();
			needSave = true;
		}

		if (data.userLevel == 0)
		{
			data.userLevel = data.dailyWords.Count - 1;
			needSave = true;
		}

		if (needSave)
			Save();
	}

	public static void Save()
	{
		BaseSaveManager.Save(data);
	}

	public static void Log()
	{
		Debug.Log("## Current ApplicationData");
		Debug.Log(data.ToString());
		Debug.Log("## Current ApplicationData (in JSON)");
		Debug.Log(JsonConvert.SerializeObject(data));
	}
}