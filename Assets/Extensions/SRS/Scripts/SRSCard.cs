using System;

namespace PierreMizzi.Extensions.SRS
{
	[Serializable]
	public class SRSCard
	{
		public int ID;

		public SRSCardStatus status = SRSCardStatus.New;

		/// <summary>
		/// The time when the card was last reviewed.
		/// </summary>
		public DateTime lastReviewedDate;

		/// <summary>
		/// The time when the card will be reviewed next.
		/// </summary>
		public DateTime nextReviewDate;

		/// <summary>
		/// X = Days | Y = Hours | Z = Minutes </summary>
		/// </summary>
		public TimeSpan interval => nextReviewDate - lastReviewedDate;

		public float ease = 1.0f;

		public int forgottonCount = 0;

		public bool hasBeenReviewed => lastReviewedDate != default;

		public override string ToString()
		{
			string log = $"SRSCard with ID : {ID} \n";
			log += $"lastReviewData : {lastReviewedDate} \n";
			log += $"nextReviewDate : {nextReviewDate} \n";
			log += $"interval : {interval.ToString()} \n";
			log += $"ease : {ease} \n";
			return log;
		}

		public SRSCard(){}

		public SRSCard(int ID)
		{
			this.ID = ID;
		}
	}
}