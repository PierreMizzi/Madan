using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SRSSettings", menuName = "SRS/SRSSettings", order = 0)]
public class SRSSettings : ScriptableObject
{
	[TextArea(3, 10)]
	public string description;

	#region Workload

	public int dailyNewCardsCount;

	public int dailyReviewCardsCount;
		
	#endregion

	#region Answer Rating

	[Header("Answer Rating")]
	public List<SRSAnswerRatingSettings> answerRatingSettings = new List<SRSAnswerRatingSettings>();

	#endregion

	#region Behaviour

	public SRSAnswerRatingSettings GetRatingSettings(SRSAnswerRating rating)
	{
		foreach (SRSAnswerRatingSettings setting in answerRatingSettings)
		{
			if (setting.rating == rating)
			{
				return setting;
			}
		}

		return default;
	}

	#endregion

	#region Debug

	public bool useCumulativeLastReviewDate = false;
		
	#endregion

}