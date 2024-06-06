using System;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplicationChannel", menuName = "ApplicationChannel", order = 0)]
public class ApplicationChannel : ScriptableObject
{
	/// MainMenu

	public WordDataDelegate onRefreshDailyWord;

	private void OnEnable()
	{
		onRefreshDailyWord = (WordData data) => { };
	}

}

public delegate void WordDataDelegate(WordData data);
