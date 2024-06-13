using System;
using UnityEngine;

[Serializable]
public class TimeFrame
{
	[SerializeField] private string m_startTime = "20:00";
	[SerializeField] private string m_endTime = "23:45";

	[SerializeField] private DateTime m_startDateTime;
	[SerializeField] private DateTime m_endDateTime;

	public DateTime startTime => m_startDateTime;
	public DateTime endTime => m_endDateTime;

	public bool IsEarly()
	{
		return DateTime.Now < m_startDateTime;
	}

	public bool IsEarly(DateTime date)
	{
		return date < m_startDateTime;
	}

	public bool IsInTimeFrame()
	{
		return m_startDateTime < DateTime.Now && DateTime.Now < m_endDateTime;
	}

	public bool IsInTimeFrame(DateTime date)
	{
		return m_startDateTime < date && date < m_endDateTime;
	}

	public bool IsLate()
	{
		return DateTime.Now > m_endDateTime;
	}

	public bool IsLate(DateTime date)
	{
		return date > m_endDateTime;
	}

	public void Initialize()
	{
		m_startDateTime = DateTime.Parse(m_startTime);
		m_endDateTime = DateTime.Parse(m_endTime);
	}
}