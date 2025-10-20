using System;
using System.Collections.Generic;


/*


*/
namespace PierreMizzi.Extensions.SRS
{
	/// <summary>
	/// Deck of SRSCards. There are different list that either contains the whole deck or what the player is supposed to review at a given time.
	/// </summary>
	[Serializable]
	public class SRSDeck
	{
		public string name;

		public string SRSSettingsName;

		/// <summary>
		/// Last time the deck has been reviewed
		/// </summary>
		public DateTime lastResetDate;

		/// <summary>
		/// All the cards contained in the deck
		/// </summary>
		public List<SRSCard> cards = new List<SRSCard>();

		/// <summary>
		/// Today's new cards that will appear for the first time. Based on setting's "dailyNewCardsCount".
		/// </summary>
		public List<SRSCard> dailyNewCards = new List<SRSCard>();

		/// <summary>
		/// Today's cards that are due for review. Based on setting's "dailyReviewCardsCount"
		/// </summary>
		public List<SRSCard> dailyReviewCards = new List<SRSCard>();

		public void Reset()
		{
			lastResetDate = DateTime.Now;

			foreach (SRSCard card in cards)
			{
				card.Reset();
			}

			dailyNewCards.Clear();
			dailyReviewCards.Clear();
		}

		public SRSDeck() { }

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
			this.lastResetDate = DateTime.Now;

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