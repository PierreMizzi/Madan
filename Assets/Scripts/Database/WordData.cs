using System;

[Serializable]
public class WordData
{

	public int ID;
	public string kanji;
	public string furigana;
	public string level;
	public string traduction;

	public DateTime dateChosen;
	public DateTime dateUnlocked;

	/// <summary> 
	///	WordData is unlocked (today) when the current date is the same as the date it was unlocked
	/// </summary>
	public bool isUnlocked => dateUnlocked.ToShortDateString() == DateTime.Now.ToShortDateString();

	#region Flashcard

	public DateTime dueDate;

	#endregion

	public override string ToString()
	{
		return $"{kanji} ({furigana}) : {traduction} [{dateChosen}]";
	}
}