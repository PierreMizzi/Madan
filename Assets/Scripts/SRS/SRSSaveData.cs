using System;
using System.Collections.Generic;
using PierreMizzi.Extensions.SRS;

[Serializable]
public class SRSSaveData
{
	public List<SRSDeck> allDecks;

	public override string ToString()
	{
		string text = "";
		foreach (SRSDeck deck in allDecks)
		{
			text += deck.ToString();
			text += "------------------------";
		}

		text += "\n";

		return text;
	}
	
}