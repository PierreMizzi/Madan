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

		public List<SRSCard> cards = new List<SRSCard>();

		public List<SRSCard> dailyNewCards = new List<SRSCard>();

		public List<SRSCard> dailyReviewCards = new List<SRSCard>();

		public SRSDeck() {}

		public SRSDeck(string name, string settingsName, List<SRSCard> allCards)
		{
			this.name = name;
			this.SRSSettingsName = settingsName;
			this.cards = new List<SRSCard>(allCards);
		}

		public SRSDeck(SRSDeck deck)
		{
			this.name = deck.name;
			this.SRSSettingsName = deck.SRSSettingsName;
			this.lastResetDate = deck.lastResetDate;

			deck.cards.CopyTo(this.cards.ToArray());
			deck.dailyNewCards.CopyTo(this.dailyNewCards.ToArray());
			deck.dailyReviewCards.CopyTo(this.dailyReviewCards.ToArray());
		}

		public SRSDeck(SRSDeckData deckData)
		{
			this.name = deckData.name;
			this.SRSSettingsName = deckData.SRSSettingsName;
			
			foreach (int cardID in deckData.cardIDs)
			{
				this.cards.Add(new SRSCard(cardID));
			}
		}

		public override string ToString()
		{
			string text = $"DECK : {name} \n";

			text += $" - allCard count : {cards.Count} \n";
			text += $" - dailyNewCards count : {dailyNewCards.Count} \n";
			text += $" - dailyReviewCards count : {dailyReviewCards.Count} \n";

			text += "\n";
			return text;
		}

		
	}
}