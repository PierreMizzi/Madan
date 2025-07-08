using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace PierreMizzi.Useful.SaveSystem
{
	/*
	
		SavedData is very simple : 

		static BaseSaveManager

		static SaveManager

		ApplicationData : BaseApplicationData
			bestScore : GameData
			{
				Time : 
				KillCount : 
			}

	*/


	public static class BaseSaveManager
	{

		#region Fields 

		private static string saveFolder = "/Saves/";
		private static string fileName = "ApplicationData";
		private static string fileExtension = ".json";

		private static string directoryPath
		{
			get
			{
				return Application.persistentDataPath + saveFolder;
			}
		}

		private static string path
		{
			get
			{
				return Application.persistentDataPath + saveFolder + fileName + fileExtension;
			}
		}

		public static bool isLoaded = false; 

		#endregion

		#region Methods 

		public static T Load<T>() where T : BaseApplicationData, new()
		{
			Debug.Log($"path : {path}");
			Debug.Log($"directoryPath : {directoryPath}");

			if (File.Exists(path))
			{
				using StreamReader streamReader = new StreamReader(path);
				string dataString = streamReader.ReadToEnd();
				isLoaded = true;
				// JSONConvert
				return JsonConvert.DeserializeObject<T>(dataString);
			}
			else
			{
				Debug.Log($"Path (({path})) doesn't exist, creating");
				Create<T>();
				return Load<T>();
			}
		}

		public static void Create<T>() where T : BaseApplicationData, new()
		{
			Directory.CreateDirectory(directoryPath);

			T data = new T();
			Save(data);
		}

		public static void Save(BaseApplicationData data)
		{
			// JSONConvert
			string dataString = JsonConvert.SerializeObject(data);

			using StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(dataString);
		}

		public static void Log()
		{
			string log = "";
			log += $"path : {path}\r\n";
			log += $"directoryPath : {directoryPath}\r\n";

			if (File.Exists(path))
			{
				using StreamReader streamReader = new StreamReader(path);
				string dataString = streamReader.ReadToEnd();
				Debug.Log(dataString);
			}
			else
				Debug.Log($"Path (({path})) doesn't exist");

		}

		#endregion

	}
}