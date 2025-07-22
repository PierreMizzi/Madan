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
		/// Cards to review, will reappear
		/// </summary>
		public List<SRSCard> reviewCards = new List<SRSCard>();

		public void Initialize(SRSDeck deck)
		{
			if (deck == null)
			{
				return;
			}

			currentDeck = deck;
			currentSettings = SRSUtility.GetSettingsFromName(deck.SRSSettingsName);

			if (currentSettings == null)
			{
				return;
			}


			studyCards = new List<SRSCard>();
			studyCards.AddRange(deck.dailyNewCards);
			studyCards.AddRange(deck.dailyReviewCards);
			studyCards.Shuffle();

			currentCard = PickNextCard();
		}

		public void ManageCardAfterFeedback(SRSAnswerRating rating)
		{
			SRSUtility.ManageCard(currentSettings, currentCard, rating);

			// 🟥 : content
			bool isCardDone = currentCard.interval > currentSettings.reviewTimespanTreshold;
			ManageCardInStudySession(isCardDone);
			SRSUtility.ManageCardInDeck(currentDeck, currentCard, isCardDone);
		}

		public void ManageCardInStudySession(bool isCardDone)
		{
			// Card has been reviewed, and shouldn't reappear. We remove it from everywhere
			if (isCardDone)
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
			// If after reviewing it's supposed to reappear, we add it to reviewCards
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
			List<SRSCard> dueReviewCards = SRSUtility.GetDueCards(reviewCards);

			// Find the first card inside review cards that is due
			if (dueReviewCards.Count > 0)
			{
				SRSUtility.OrderCardByReviewTime(ref dueReviewCards);
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