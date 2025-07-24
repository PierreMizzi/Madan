using System;
using System.Collections.Generic;
using PierreMizzi.Extensions.SRS;

[Serializable]
public class SRSSaveData
{
	public List<SRSDeck> decks = new List<SRSDeck>();

	public override string ToString()
	{
		string text = "";
		foreach (SRSDeck deck in decks)
		{
			text += deck.ToString();
			text += "------------------------";
		}

		text += "\n";

		return text;
	}

	public SRSSaveData()
	{
		decks = new List<SRSDeck>();
	}
	
}