using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Extensions.SRS
{
	public class SRSStudySession
	{
		public SRSDeck currentDeck;
		public SRSSettings currentSettings;
		public SRSCard currentCard;

		/// <summary>
		/// All the cards about to be studied during the session
		/// </summary>
		public List<SRSCard> studyCards = new List<SRSCard>();

		/// <summary>
		/// Cards we've seen during the session and are about to re-appear
		/// </summary>
		public List<SRSCard> reviewCards = new List<SRSCard>();

		public void Initialize(SRSDeck deck)
		{
			if (deck == null)
			{
				return;
			}

			currentDeck = deck;
			currentSettings = SRSManager.GetSettingsFromName(deck.SRSSettingsName);

			studyCards = new List<SRSCard>();
			studyCards.AddRange(deck.dailyNewCards);
			studyCards.AddRange(deck.dailyReviewCards);

			studyCards.Shuffle();

			currentCard = PickNextCard();
		}

		public void ManageCardAfterFeedback(SRSAnswerRating rating)
		{
			SRSManager.ManageCardAfterFeedback(currentSettings, currentCard, rating);

			if (currentCard.interval > currentSettings.reviewTimespanTreshold)
			{
				if (reviewCards.Contains(currentCard))
				{
					reviewCards.Remove(currentCard);
				}
				else if (studyCards.Contains(currentCard))
				{
					studyCards.Remove(currentCard);
				}
			}
			else
			{
				if (studyCards.Contains(currentCard))
				{
					studyCards.Remove(currentCard);
				}
				if (reviewCards.Contains(currentCard) == false)
				{
					reviewCards.Add(currentCard);
				}
			}
		}

		public SRSCard PickNextCard()
		{
			List<SRSCard> dueReviewCards = SRSManager.GetDueCards(reviewCards);

			if (dueReviewCards.Count > 0)
			{
				SRSManager.OrderCardByReviewTime(ref dueReviewCards);
				return dueReviewCards[0];

			}
			else if (studyCards.Count > 0)
			{
				return studyCards[0];
			}
			else
			{
				Debug.Log("Study session is over !!!");
				return null;
			}
		}

		public SRSStudySession(SRSDeck deck)
		{
			Initialize(deck);
		}

	}
}