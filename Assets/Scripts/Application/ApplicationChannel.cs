using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "ApplicationChannel", order = 0)]
public class ApplicationChannel : ScriptableObject
{
	/// MainMenu

	public WordDataDelegate onRefreshDailyWord;

	public Action onAppDataLoaded;
	public Action onDatabaseLoaded;

	public DisplayScreenDelegate onDisplayScreen;

	public CheckDailyCheckDelegate onCheckDailyCheck;

	private void OnEnable()
	{
		onRefreshDailyWord = (WordData data) => { };

		onAppDataLoaded = () => { };
		onDatabaseLoaded = () => { };

		onDisplayScreen = (ApplicationScreenType type, string[] options) => { };

		onCheckDailyCheck = (DailyCheckType type) => { };
	}

}

public delegate void WordDataDelegate(WordData data);
public delegate void DisplayScreenDelegate(ApplicationScreenType type, params string[] options);

public delegate void CheckDailyCheckDelegate(DailyCheckType type);