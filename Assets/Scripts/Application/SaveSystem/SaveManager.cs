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
	}

	public static void Save()
	{
		BaseSaveManager.Save(data);
		Log();
	}

	public static void Log()
	{
		BaseSaveManager.Log();

		Debug.Log(JsonUtility.ToJson(data));
	}
}