using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace PierreMizzi.Extensions.SRS
{
	[ExecuteInEditMode]
	public class SRSDebugger : MonoBehaviour
	{
		#region Behaviour

		[SerializeField] private SRSSettings m_SRSsettings;

		private void OnEnable()
		{
			Debug.Log("OnEnable");
			SRSUtility.settings = new List<SRSSettings>() { m_SRSsettings };
		}

		#endregion

		#region Test Card

		[Header("Test Card")]
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
				SRSUtility.ManageCard(m_SRSsettings, m_testCard, rating);
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

		#endregion

		#region Test Deck

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
		}

		public void StopStudySession()
		{
			currentStudySession = null;
			isFrontOrBack = SRSCardFace.Front;
			currentCardFront = "";
			currentCardBack = "";
		}

		#endregion

	}

}
