using System;
using System.Collections.Generic;
using PierreMizzi.Extensions.SRS;
using UnityEditor.Overlays;
using UnityEngine;

[ExecuteInEditMode]
public class SRSManager : MonoBehaviour
{

	#region MonoBehaviour

	private void OnEnable()
	{
		Debug.Log("OnEnable");
		SRSUtility.settings = new List<SRSSettings>() { m_SRSsettings };
	}

	private void Awake()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded += Initialize;
		}
	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded -= Initialize;
		}
	}

	#endregion

	#region Behaviour

	[SerializeField] public ApplicationChannel m_applicationChannel;

	[SerializeField] private SRSSettings m_SRSsettings;

	private void Initialize()
	{
		Load();
	}

	#endregion

	#region Decks



	#endregion

	#region Study Session


	[Header("Test Deck")]

	public SRSDeck debugDeck
	{
		get
		{
			if (savedData == null || savedData.decks.Count == 0)
			{
				return null;
			}
			else
			{
				return savedData.decks[0];
			}
		}
	}

	public SRSStudySession currentStudySession;
	[HideInInspector] public SRSCardFace isFrontOrBack = SRSCardFace.None;

	public string currentCardFront;
	public string currentCardBack;

	// [Obsolete]
	public void InitializeDebugDeck()
	{
		SRSUtility.settings = new List<SRSSettings>() { m_SRSsettings };

		SRSUtility.ApplyDailyReset(debugDeck);
	}

	public void StartStudySession()
	{
		if (debugDeck == null)
		{
			return;
		}

		currentStudySession = new SRSStudySession(debugDeck);
		SetCardFront();
	}

	public void SetCardFront()
	{
		isFrontOrBack = SRSCardFace.Front;
		currentCardFront = GetCurrentCardFront();
		currentCardBack = "";
	}

	public void SetCardBack()
	{
		isFrontOrBack = SRSCardFace.Back;
		currentCardBack = GetCurrentCardFront();
		currentCardBack = GetCurrentCardBack();
	}

	private string GetCurrentCardFront()
	{
		if (currentStudySession.currentCard == null)
		{
			return "";
		}
		else
		{
			if (Database.wordDatass.ContainsKey(currentStudySession.currentCard.ID))
			{
				return Database.wordDatass[currentStudySession.currentCard.ID].kanji;
			}
			else
			{
				return "";
			}
		}
	}

	private string GetCurrentCardBack()
	{
		if (currentStudySession.currentCard == null)
		{
			return "";
		}
		else
		{
			if (Database.wordDatass.ContainsKey(currentStudySession.currentCard.ID))
			{
				return Database.wordDatass[currentStudySession.currentCard.ID].traduction;
			}
			else
			{
				return "";
			}
		}
	}

	public void ManageCardAfterFeedback(SRSAnswerRating rating)
	{
		currentStudySession.ManageCardAfterFeedback(rating);

		currentStudySession.currentCard = currentStudySession.PickNextCard();

		if (currentStudySession.currentCard == null)
		{
			Debug.Log("Study session is over !!!");
			return;
		}
		else
		{
			SetCardFront();
		}

		Save();
	}

	public void StopStudySession()
	{
		currentStudySession = null;
		isFrontOrBack = SRSCardFace.Front;
		currentCardFront = "";
		currentCardBack = "";
	}

	#endregion

	#region SaveData

	public SRSSaveData savedData
	{
		get
		{
			if (SaveManager.data != null && SaveManager.data.srsSaveData != null)
			{
				return SaveManager.data.srsSaveData;
			}
			else
				return null;
		}
	}

	// 🟥 : Test link between SaveManager and SRSManager
	// 🔜 : Load DebugDeck from savedData
	public void Load()
	{
		if (savedData == null)
		{
			// Database
			CreateSRSSaveData();
		}

		foreach (SRSDeck deck in savedData.decks)
		{
			if (ChecksNeedsToDailyReset(deck))
			{
				Debug.Log($"[SRS MANAGER] We daily reset deck \"{deck.name}\"");
				SRSUtility.ApplyDailyReset(deck);
			}
		}

		SaveManager.Save();
	}

	private bool ChecksNeedsToDailyReset(SRSDeck deck)
	{
		SRSSettings settings = SRSUtility.GetSettingsFromName(deck.SRSSettingsName);

		if (settings == null)
		{
			return false;
		}
		else
		{
			return DateTime.Now > settings.TodayResetTime && deck.lastResetDate < settings.TodayResetTime;
		}
	}


	/// <summary>
	/// Translate the SRSDeck from the Database to an actual usable SRSDeck
	/// </summary>
	private void CreateSRSSaveData()
	{
		SaveManager.data.srsSaveData = new SRSSaveData();

		foreach (SRSDeckData deckData in Database.srsDeckDatas)
		{
			SRSDeck deck = new SRSDeck(deckData);
			savedData.decks.Add(deck);
		}
	}

	public void Save()
	{
		if (savedData == null)
		{
			return;
		}
		SaveDeck(debugDeck);
	}

	private void SaveDeck(SRSDeck deck)
	{
		SRSDeck savedDeck = savedData.decks.Find(item => item.name == deck.name);

		if (savedDeck == null)
		{
			savedData.decks.Add(deck);
		}
		else
		{
			savedDeck = debugDeck;
		}
		SaveManager.Save();
		SaveManager.Log();
	}

	#endregion
}