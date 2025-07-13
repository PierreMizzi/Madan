using System;
using System.Collections.Generic;
using System.Linq;
using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Extensions.SRS
{

	public static class SRSManager
	{

		public static List<SRSSettings> settings = new List<SRSSettings>();

		#region Card Review

		public static void DeckDailyReset(SRSDeck deck)
		{
			SRSSettings settings = GetSettingsFromName(deck.SRSSettingsName);

			if (settings == null)
			{
				return;
			}

			RefillDailyNewCards(deck, settings.dailyNewCardsCount);
			RefillDailyReviewCards(deck, settings.dailyReviewCardsCount);
		}

		public static void RefillDailyNewCards(SRSDeck deck, int count)
		{
			if (deck == null || deck.allCards.Count == 0)
			{
				return;
			}

			List<SRSCard> candidateCards = GetNewCards(deck);

			// We remove reviewCards from dueCards
			foreach (SRSCard newCard in deck.dailyNewCards)
			{
				if (candidateCards.Contains(newCard))
				{
					candidateCards.Remove(newCard);
				}
			}

			RefillCards(ref deck.dailyNewCards, ref candidateCards, count);
		}

		public static List<SRSCard> GetNewCards(SRSDeck deck)
		{
			if (deck == null || deck.allCards.Count == 0)
			{
				return new List<SRSCard>();
			}
			else
			{
				return deck.allCards.FindAll(card => card.hasBeenReviewed == false);
			}
		}

		public static void RefillDailyReviewCards(SRSDeck deck, int count)
		{
			if (deck == null || deck.allCards.Count == 0)
			{
				return;
			}
			// We get all due cards
			List<SRSCard> candidateCards = GetDueCards(deck.allCards);

			// We remove reviewCards from dueCards
			foreach (SRSCard reviewCard in deck.dailyReviewCards)
			{
				if (candidateCards.Contains(reviewCard))
				{
					candidateCards.Remove(reviewCard);
				}
			}

			RefillCards(ref deck.dailyReviewCards, ref candidateCards, count);
		}

		public static void RefillCards(ref List<SRSCard> currentCards, ref List<SRSCard> candidateCards, int count)
		{
			if (currentCards == null ||
				candidateCards == null || candidateCards.Count == 0 ||
				count == 0)
			{
				return;
			}

			int neededCount = count - currentCards.Count;

			neededCount = Mathf.Min(neededCount, candidateCards.Count);

			for (int i = 0; i < neededCount; i++)
			{
				SRSCard dueCard = candidateCards.PickRandom();
				currentCards.Add(dueCard);
				candidateCards.Remove(dueCard);
			}
		}

		public static List<SRSCard> GetDueCards(List<SRSCard> cards, DateTime revisionDate = default)
		{
			List<SRSCard> dueCards = new List<SRSCard>();
			if (cards == null || cards.Count == 0)
			{
				return dueCards;
			}

			// If no revisionDate is specified, then the revisionDate is Today !
			if (revisionDate == default)
			{
				revisionDate = DateTime.Today;
			}

			foreach (SRSCard card in cards)
			{
				if (card.hasBeenReviewed && card.nextReviewDate < revisionDate)
				{
					dueCards.Add(card);
				}
			}

			return dueCards;
		}

		public static SRSSettings GetSettingsFromName(string settingsName)
		{
			if (settings.Count == 0)
			{
				Debug.LogWarning($"SRS : No settings was found with name {settingsName}");
				return null;
			}

			return settings.Find(settings => settings.name == settingsName);
		}

		public static void ManageCardAfterFeedback(SRSSettings SRSSettings, SRSCard card, SRSAnswerRating rating)
		{
			SRSAnswerRatingSettings settings = SRSSettings.GetRatingSettings(rating);

			if (settings == null)
			{
				Debug.LogWarning($"No settings found for rating : {rating}");
				return;
			}

			if (SRSSettings.useCumulativeLastReviewDate)
			{
				card.lastReviewedDate = card.nextReviewDate;
			}
			else
			{
				card.lastReviewedDate = DateTime.Now;
			}

			switch (rating)
			{
				case SRSAnswerRating.Forgotten:
					card.forgottonCount++;
					card.ease += settings.easeModifier;
					card.nextReviewDate = card.lastReviewedDate + settings.ComputeNextReviewTimespan(card);
					break;
				case SRSAnswerRating.Hard:
				case SRSAnswerRating.Correct:
				case SRSAnswerRating.Easy:
					card.ease += settings.easeModifier;
					card.nextReviewDate = card.lastReviewedDate + settings.ComputeNextReviewTimespan(card, true);
					break;
				default:
					break;
			}
		}

		public static void OrderCardByReviewTime(ref List<SRSCard> cards)
		{
			cards.OrderBy(card => card.nextReviewDate);
		}

		#endregion

	}
}