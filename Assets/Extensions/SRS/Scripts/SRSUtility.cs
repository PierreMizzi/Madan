using System;
using System.Collections.Generic;
using System.Linq;
using PierreMizzi.Useful;
using UnityEngine;

namespace PierreMizzi.Extensions.SRS
{

	public enum SRSAnswerRating
	{
		None,
		Forgotten,
		Hard,
		Correct,
		Easy,
	}

	public enum SRSCardStatus
	{
		None,
		New,
		Reviewed,
		Leeched,
	}

	public enum SRSCardFace
	{
		None,
		Front,
		Back,
	}

	[Serializable]
	public class SRSAnswerRatingSettings
	{
		[Tooltip("Name. Mostly used for Editor readability")]
		public string name;
		public SRSAnswerRating rating;

		/// <summary>
		/// X = Days | Y = Hours | Z = Minutes </summary>
		/// </summary>
		[Tooltip("X = Days | Y = Hours | Z = Minutes")]
		public Vector3 interval = new Vector3(0, 0, 0);

		public float easeModifier = 0.15f;

		public TimeSpan ComputeNextReviewTimespan(SRSCard card, bool useEase = false)
		{
			if (card == null)
			{
				throw new ArgumentNullException(nameof(card), "Card cannot be null.");
			}

			if (useEase == false)
			{
				return SRSUtility.ToTimeSpan(interval);
			}
			else
			{
				return SRSUtility.ToTimeSpan(interval) * card.ease;
			}
		}
	}

	public static class SRSUtility
	{
		/// <summary>
		/// Converts a Vector3 to a TimeSpan.
		/// </summary>
		/// <param name="interval">The Vector3 to convert.</param>
		/// <returns>A TimeSpan representing the interval.</returns>
		public static TimeSpan ToTimeSpan(Vector3 interval)
		{
			return new TimeSpan((int)interval.x, (int)interval.y, (int)interval.z, 0);
		}

		#region Deck

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

		#endregion

		#region Study Session

		/// <summary>
		/// Sets cards's data based on the rating it received
		/// </summary>
		/// <param name="SRSSettings"></param>
		/// <param name="card"></param>
		/// <param name="rating"></param>
		public static void ManageCard(SRSSettings SRSSettings, SRSCard card, SRSAnswerRating rating)
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

		public static void ManageCardInDeck(SRSDeck deck, SRSCard card, bool isCardDone)
		{
			// Null checks
			if (deck == null || card == null)
			{
				return;
			}

			// Make sure this card belongs to the deck
			if (deck.allCards.Contains(card) == false)
			{
				return;
			}

			// NewCard reviewed for the first time
			if (deck.dailyNewCards.Contains(card))
			{
				deck.dailyNewCards.Remove(card);

				// Card has to be reviewd later, we add it to reviewCards
				if (isCardDone == false && deck.dailyReviewCards.Contains(card) == false)
				{
					deck.dailyReviewCards.Add(card);
				}
			}
			else
			{
				// Card needed to be reviewd and is done, we remove it
				if (isCardDone && deck.dailyReviewCards.Contains(card))
				{
					deck.dailyReviewCards.Remove(card);
				}
			}
		}

		#endregion

		#region Settings

		public static List<SRSSettings> settings = new List<SRSSettings>();

		public static SRSSettings GetSettingsFromName(string settingsName)
		{
			if (settings.Count == 0)
			{
				Debug.LogWarning($"SRS : No settings was found with name {settingsName}");
				return null;
			}

			return settings.Find(settings => settings.name == settingsName);
		}

		#endregion

	}
}

