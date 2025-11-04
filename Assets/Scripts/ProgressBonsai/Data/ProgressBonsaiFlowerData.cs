
using System;
using PierreMizzi.Extensions.Timer;

[Serializable]
public class ProgressBonsaiFlowerData
{
	public int spotChildIndex;
	public string category;
	public DateTime startTime;
	public DateTime endTime;
	public TimeSpan totalTime;
	public float focus;

	public ProgressBonsaiFlowerData()
	{
		
	}

	public ProgressBonsaiFlowerData(StudyTime pomodoro, int spotChildIndex)
	{
		this.spotChildIndex = spotChildIndex;

		category = pomodoro.category;
		startTime = pomodoro.startTime;
		endTime = pomodoro.endTime;
		totalTime = pomodoro.totalTime;
		focus = pomodoro.focus;
	}
}