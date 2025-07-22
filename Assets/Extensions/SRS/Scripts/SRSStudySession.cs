using System;
using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Extensions.SRS
{
	[Serializable]
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

			// Find the first card inside review cards that is due
			if (dueReviewCards.Count > 0)
			{
				SRSManager.OrderCardByReviewTime(ref dueReviewCards);
				return dueReviewCards[0];
			}
			// If no due card inside reviewCards, we pick a card to study
			else if (studyCards.Count > 0)
			{
				return studyCards[0];
			}
			// If no more cards to study, we pick inside reviewCards
			else if (reviewCards.Count > 0)
			{
				// First, we find a different card than the (last) currentOne
				if (reviewCards.Count > 1)
				{
					return reviewCards.Find(card => card != currentCard);
				}
				// There is only one left, we pick it anyway !
				else
				{
					return reviewCards[0];
				}
			}
			else
			{
				return null;
			}
		}

		public SRSStudySession(SRSDeck deck)
		{
			Initialize(deck);
		}

	}
}