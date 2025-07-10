using System;
using UnityEngine;

public static class SRSManager
{

	#region Card Review

	public static void ManageCardAfterFeedback(SRSSettings SRSSettings, SRSCard card, SRSAnswerRating rating)
	{
		SRSAnswerRatingSettings settings = SRSSettings.GetRatingSettings(rating);

		if (settings == null)
		{
			Debug.LogWarning($"No settings found for status: {card.status} and difficulty: {rating}");
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