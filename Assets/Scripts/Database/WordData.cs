using System;

[Serializable]
public class WordData
{

	public string kanji;
	public string furigana;
	public string level;
	public string traduction;

	public DateTime date;

	public override string ToString()
	{
		return $"{kanji} ({furigana}) : {traduction} [{date}]";
	}
}