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

	private void OnEnable()
	{
		onRefreshDailyWord = (WordData data) => { };

		onAppDataLoaded = () => { };
		onDatabaseLoaded = () => { };

		onDisplayScreen = (ApplicationScreenType type, string[] options) => { };
	}

}

public delegate void WordDataDelegate(WordData data);
public delegate void DisplayScreenDelegate(ApplicationScreenType type, params string[] options);