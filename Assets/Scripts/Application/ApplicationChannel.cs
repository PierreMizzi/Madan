using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "ApplicationChannel", order = 0)]
public class ApplicationChannel : ScriptableObject
{
	// Application
	public Action onAppDataLoaded;
	public Action onDatabaseLoaded;
	public DisplayScreenDelegate onDisplayScreen;

	/// MainMenu
	public WordDataDelegate onRefreshDailyWord;
	public Action onRefreshDailyCheck;
	public CheckDailyCheckDelegate onCheckDailyCheck;

	// TrialMenu
	

	private void OnEnable()
	{
		// Application
		onAppDataLoaded = () => { };
		onDatabaseLoaded = () => { };
		onDisplayScreen = (ApplicationScreenType type, string[] options) => { };

		onRefreshDailyWord = (WordData data) => { };
		onRefreshDailyCheck = () => { };
		onCheckDailyCheck = (DailyCheckType type) => { };
	}

}

public delegate void WordDataDelegate(WordData data);
public delegate void DisplayScreenDelegate(ApplicationScreenType type, params string[] options);
public delegate void CheckDailyCheckDelegate(DailyCheckType type);