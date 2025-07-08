using System;
using TMPro;
using UnityEngine;

public class CalendarDay : MonoBehaviour
{

	#region Behaviour

	private CalendarManager m_manager;

	public int Index { get; private set; }

	public int Row { get; private set; }

	public int Column { get; private set; }

	[SerializeField] private TextMeshPro m_dayText;

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		// Initialization logic here
	}

	private void OnDestroy()
	{
		// Cleanup logic here
	}

	#endregion

	#region Initialization

	public void Initialize(CalendarManager manager, int index, int row, int column)
	{
		m_manager = manager;
		Index = index;
		Row = row;
		Column = column;
	}

	public void RefreshDay(int day)
	{
		m_dayText.text = day.ToString();
		MatchAppearance(null);
	}

	public void RefreshDay(DayData data)
	{
		m_dayText.text = data.date.Day.ToString();
		MatchAppearance(data);
	}

	#endregion

	#region Appearance

	[Header("Appearance")]
	[SerializeField] private SpriteRenderer m_morningDailyCheckCorner;
	[SerializeField] private SpriteRenderer m_afternoonDailyCheckCorner;
	[SerializeField] private SpriteRenderer m_eveningDailyCheckCorner;
	[SerializeField] private SpriteRenderer m_trialCheckCorner;

	[SerializeField] private SpriteRenderer m_todayMarker;

	[SerializeField] private Color m_normalColor = Color.blue;
	[SerializeField] private Color m_perfectColor = Color.yellow;

	public void MatchAppearance(DayData data)
	{
		if (data != null)
		{
			m_todayMarker?.gameObject.SetActive(DateTime.Now.Date.ToString("d") == data.date.ToString("d"));
		}
		else
		{
			m_todayMarker?.gameObject.SetActive(false);
		}

		MathDailyCheckCorners(data);
	}
	
	private void MathDailyCheckCorners(DayData data)
	{
		if (data == null)
		{
			m_morningDailyCheckCorner.gameObject.SetActive(false);
			m_afternoonDailyCheckCorner.gameObject.SetActive(false);
			m_eveningDailyCheckCorner.gameObject.SetActive(false);
			m_trialCheckCorner.gameObject.SetActive(false);
			return;
		}

		m_morningDailyCheckCorner.gameObject.SetActive(data.morningDailyCheckDone);
		m_afternoonDailyCheckCorner.gameObject.SetActive(data.afternoonDailyCheckDone);
		m_eveningDailyCheckCorner.gameObject.SetActive(data.eveningDailyCheckDone);
		m_trialCheckCorner.gameObject.SetActive(data.trialDone);

		if (data.morningDailyCheckDone &&
			data.afternoonDailyCheckDone &&
			data.eveningDailyCheckDone &&
			data.trialDone)
		{
			m_morningDailyCheckCorner.color = m_perfectColor;
			m_afternoonDailyCheckCorner.color = m_perfectColor;
			m_eveningDailyCheckCorner.color = m_perfectColor;
			m_trialCheckCorner.color = m_perfectColor;
		}
		else
		{
			m_morningDailyCheckCorner.color = m_normalColor;
			m_afternoonDailyCheckCorner.color = m_normalColor;
			m_eveningDailyCheckCorner.color = m_normalColor;
			m_trialCheckCorner.color = m_normalColor;
		}
	}

	#endregion
}