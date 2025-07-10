using System;
using System.Collections.Generic;
using PierreMizzi.Useful;
using UnityEngine;

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

		deck.newCards = GetNewCards(deck, settings.dailyNewCardsCount);
		deck.reviewCards = GetReviewCards(deck, settings.dailyReviewCardsCount);
	}

	public static List<SRSCard> GetNewCards(SRSDeck deck, int count)
	{
		DateTime noDate = new DateTime();
		List<SRSCard> allNewCards = deck.allCards.FindAll(card => card.lastReviewedDate == noDate);
		List<SRSCard> selectedNewCards = new List<SRSCard>();

		if (allNewCards.Count < count)
		{
			count = allNewCards.Count;
		}
		else if (allNewCards.Count == 0)
		{
			return selectedNewCards;
		}

		for (int i = 0; i < count; i++)
		{
			SRSCard card = allNewCards.PickRandom();
			allNewCards.Remove(card);
			selectedNewCards.Add(card);
		}

		return selectedNewCards;
	}

	public static List<SRSCard> GetReviewCards(SRSDeck deck, int count)
	{
		List<SRSCard> reviewCards = new List<SRSCard>();



		return reviewCards;
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

	#endregion
}