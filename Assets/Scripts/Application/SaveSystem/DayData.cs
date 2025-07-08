using System;
using System.Collections.Generic;

[Serializable]
public class DayData
{
	public DateTime date;
	public bool morningDailyCheckDone = false;
	public bool afternoonDailyCheckDone = false;
	public bool eveningDailyCheckDone = false;
	public bool trialDone = false;

	public List<int> newDailyWordsIDs = new List<int>();

	public DayData() {}

	public DayData(DateTime today, List<int> newIDs)
	{
		this.date = today;
		newDailyWordsIDs = newIDs;
	}

}