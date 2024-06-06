using System;

[Serializable]
public class WordData
{
	public string kanji;
	public string hiragana;
	public string level;
	public string traduction;

	public DateTime date;

	public override string ToString()
	{
		return $"{kanji} ({hiragana}) : {traduction}";
	}
}