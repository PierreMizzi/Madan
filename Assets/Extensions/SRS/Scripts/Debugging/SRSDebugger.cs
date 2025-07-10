using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SRSDebugger : MonoBehaviour
{
	[Header("Test")]

	[SerializeField] private SRSSettings m_SRSsettings;
	[SerializeField] private SRSDebuggingSettings m_debuggingSettings;
	[SerializeField] private SRSCard m_testCard;

	public void ResetTestCard()
	{
		m_testCard = new SRSCard
		{
			ID = 1,
			lastReviewedDate = DateTime.Now,
			nextReviewDate = DateTime.Now,
			ease = 1.0f
		};
	}

	[ContextMenu("Test SRS Algorythm")]
	public void TestSRSAlgorythm()
	{
		m_SRSsettings.useCumulativeLastReviewDate = true;
		ResetTestCard();

		string log = $"TestSRSAlgorythm \n";
		log += $"----------------------\n";
		int index = 1;
		DateTime firstReviewTime = DateTime.Now;
		foreach (SRSAnswerRating rating in m_debuggingSettings.successiveRatings)
		{
			SRSManager.ManageCardAfterFeedback(m_SRSsettings, m_testCard, rating);
			log += $"Review n*{index} with rating : {rating} \n";
			log += $"Card {m_testCard} \n";
			index++;
		}

		log += $"----------------------\n";
		log += $"First review time : {firstReviewTime} \n";
		log += $"Next review time : {m_testCard.nextReviewDate}\n";
		log += $"First and next difference : {m_testCard.nextReviewDate - firstReviewTime}\n";

		Debug.Log(log);
		Debug.Log(new DateTime());

		m_SRSsettings.useCumulativeLastReviewDate = false;

	}

	#region Test Multiple Days

	public SRSDeck debugDeck;

	public void InitializeDebugDeck()
	{
		List<SRSCard> cards = new List<SRSCard>();
		int length = 20;
		for (int i = 0; i < length; i++)
		{
			SRSCard newCard = new SRSCard() { ID = UnityEngine.Random.Range(0, 1000000) };
			cards.Add(newCard);
		}

		debugDeck = new SRSDeck("Debug Deck", "SRSSettings", cards);

		SRSManager.settings = new List<SRSSettings>() { m_SRSsettings };
		SRSManager.DeckDailyReset(debugDeck);

	}

	#endregion

}