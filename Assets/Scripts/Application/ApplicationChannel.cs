using System;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "ApplicationChannel", order = 0)]
public class ApplicationChannel : ScriptableObject
{
	/// MainMenu

	public WordDataDelegate onRefreshDailyWord;

	public Action onAppDataLoaded;
	public Action onDatabaseLoaded;

	private void OnEnable()
	{
		onRefreshDailyWord = (WordData data) => { };

		onAppDataLoaded = () => { };
		onDatabaseLoaded = () => { };
	}

}

public delegate void WordDataDelegate(WordData data);
