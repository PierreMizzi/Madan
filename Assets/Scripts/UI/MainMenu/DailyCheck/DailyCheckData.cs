using System;

[Serializable]
public class DailyCheckData
{

	public DailyCheckData(DailyCheckType type)
	{
		this.type = type;
		checkTime = DateTime.Now;
	}

	public DailyCheckType type;
	public DateTime checkTime;
	public bool hasBeenChecked;

	public void Reset()
	{
		checkTime = new DateTime();
		hasBeenChecked = false;
	}

	public override string ToString()
	{
		string data = $"type : {type} \n";
		data += $"checkTime : {checkTime} \n";
		data += $"hasBeenChecked : {hasBeenChecked} \n";
		return data;
	}
}