using System;
using System.Collections.Generic;
using UnityEngine;

namespace PierreMizzi.Extensions.SRS
{

	[CreateAssetMenu(fileName = "SRSSettings", menuName = "SRS/SRSSettings", order = 0)]
	public class SRSSettings : ScriptableObject
	{
		[TextArea(3, 10)]
		public string description;

		#region Workload

		public int dailyNewCardsCount;

		public int dailyReviewCardsCount;

		/// <summary>
		/// X = Hour | Y = Minutes | Z = Seconds
		/// </summary>
		public Vector3 resetHour = new Vector3();

		#endregion

		#region Answer Rating

		[Header("Answer Rating")]
		public List<SRSAnswerRatingSettings> answerRatingSettings = new List<SRSAnswerRatingSettings>();

		/// <summary>
		/// During a study session, maximum interval time possible for a SRSCard 
		/// to be still reviewable during this study session.
		/// Otherwise, the card is considered reviewed for today
		/// X = Days | Y = Hours | Z = Minutes
		/// </summary>
		public Vector3 reviewTimeTreshold;

		public TimeSpan reviewTimespanTreshold => SRSUtility.ToTimeSpan(reviewTimeTreshold);

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
}