using System;
using System.Collections.Generic;
using PierreMizzi.Extensions.SRS;
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

	#endregion

	#region Test Deck

	[SerializeField] private SRSSettings m_SRSsettings;

	[Header("Test Deck")]

	public SRSDeck debugDeck;
	public SRSStudySession currentStudySession;
	[HideInInspector] public SRSCardFace isFrontOrBack = SRSCardFace.None;

	public string currentCardFront;
	public string currentCardBack;

	public void InitializeDebugDeck()
	{
		List<SRSCard> cards = new List<SRSCard>();

		int length = 20;
		for (int i = 0; i < length; i++)
		{
			SRSCard newCard = new SRSCard() { ID = i };
			cards.Add(newCard);
		}

		debugDeck = new SRSDeck("Debug Deck", "SRSSettings", cards);

		SRSUtility.settings = new List<SRSSettings>() { m_SRSsettings };

		SRSUtility.DeckDailyReset(debugDeck);
	}

	public void StartStudySession()
	{
		currentStudySession = new SRSStudySession(debugDeck);
		SetCardFront();
	}

	public void SetCardFront()
	{
		isFrontOrBack = SRSCardFace.Front;
		currentCardFront = currentStudySession.currentCard.ID.ToString();
		currentCardBack = "";
	}

	public void SetCardBack()
	{
		isFrontOrBack = SRSCardFace.Back;
		currentCardBack = currentStudySession.currentCard.ID.ToString();
		currentCardBack = ((DayOfWeek)(currentStudySession.currentCard.ID / 7.0f)).ToString();
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
			if (SaveManager.data == null || SaveManager.data.srsSaveData == null)
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
		SRSDeck savedDeck = savedData.allDecks.Find(item => item.name == deck.name);

		if (savedDeck == null)
		{
			savedData.allDecks.Add(deck);
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