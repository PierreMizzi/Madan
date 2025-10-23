using System;
using System.Collections.Generic;

[Serializable]
public class TimerData
{
	public List<Pomodoro> pomodoros = new List<Pomodoro>();

	public override string ToString()
	{
		string data = "TimerData datas : \n";
		data += $"pomodoros.Count : {pomodoros.Count}";
		return data;
	}
}

[Serializable]
public class Pomodoro
{
	public DateTime startTime;
	public DateTime endTime;
	public float focus;
}