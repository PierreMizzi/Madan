using System.Collections.Generic;
using PierreMizzi.Useful;

namespace PierreMizzi.Extensions.SRS
{
	public class SRSStudySession
	{
		public SRSDeck currentDeck;
		public SRSSettings currentSettings;

		/// <summary>
		/// All the cards about to be studied during the session
		/// </summary>
		public List<SRSCard> studyCards = new List<SRSCard>();

		/// <summary>
		/// Cards we studied and are about to re-appear
		/// </summary>
		public List<SRSCard> reviewCards = new List<SRSCard>();

		public void Initialize(SRSDeck deck)
		{
			if (deck == null)
			{
				return;
			}

			currentDeck = deck;
			currentSettings = SRSManager.GetSettingsFromName(deck.SRSSettingsName);

			studyCards = new List<SRSCard>();
			studyCards.AddRange(deck.dailyNewCards);
			studyCards.AddRange(deck.dailyReviewCards);

			studyCards.Shuffle();
		}

		public SRSCard PickNextCard()
		{
			SRSCard nextCard = null;

			if (reviewCards.Count == 0)
			{
				return (reviewCards.Count > 0) ? reviewCards[0] : null;
			}
			else
			{
				List<SRSCard> dueCards = SRSManager.GetDueCards(reviewCards);


			}

			return nextCard;
		}

		public SRSStudySession(SRSDeck deck)
		{
			Initialize(deck);
		}	

	}
}