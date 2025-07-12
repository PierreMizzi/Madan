using System;
using System.Collections.Generic;

namespace PierreMizzi.Extensions.SRS
{
	[Serializable]
	public class SRSDeck
	{
		public string name;

		public string SRSSettingsName;

		public DateTime lastResetDate;

		public List<SRSCard> allCards = new List<SRSCard>();

		public List<SRSCard> dailyNewCards = new List<SRSCard>();

		public List<SRSCard> dailyReviewCards = new List<SRSCard>();

		public SRSDeck(string name, string settingsName, List<SRSCard> allCards)
		{
			this.name = name;
			this.SRSSettingsName = settingsName;
			this.allCards = new List<SRSCard>(allCards);
		}
	}
}