using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CalendarManager : MonoBehaviour
{

	#region Behaviour

	[SerializeField] private CalendarDay m_calendarDayPrefab;

	[SerializeField] private Transform m_calendarDayOriginPosition;


	[SerializeField] private float m_spacingCards = 0.2f;

	private const float k_cardWidth = 0.635f;
	private const float k_cardHeight = 0.88f;
	public float OffsetRow => k_cardHeight + m_spacingCards;
	public float OffsetColumn => k_cardWidth + m_spacingCards;


	public List<List<CalendarDay>> m_calendarDays = new List<List<CalendarDay>>();

	#endregion

	#region Ordering

	private int m_currentMonth = DateTime.Now.Month;
	private int CurrentMonth
	{
		get => m_currentMonth;

		set
		{
			m_currentMonth = Mathf.Clamp(value, 1, 12);
			RefreshCalendarDay();
		}
	}

	private DateTime m_currentMonthDate;
	private DateTime CurrentMonthDate
	{
		get => new DateTime(2025, m_currentMonth, 1);
	}


	private void RefreshCalendarDay()
	{
		DayOfWeek dayOfWeek = CurrentMonthDate.DayOfWeek;
		DateTime currentDate = CurrentMonthDate - TimeSpan.FromDays((int)dayOfWeek);
		DayData currentDayData = new DayData();

		// int day
		foreach (List<CalendarDay> row in m_calendarDays)
		{
			foreach (CalendarDay calendarDay in row)
			{
				currentDayData = SaveManager.data.dayDatas.Find(dayData => dayData.date.Date.ToString("d") == currentDate.Date.ToString("d"));

				if (currentDayData != null)
				{
					calendarDay.RefreshDay(currentDayData);
				}
				else
				{
					calendarDay.RefreshDay(currentDate.Day);
				}

				currentDate = currentDate.AddDays(1);
			}
		}
	}

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		// TEMPORARY
		SaveManager.Load();

		// Initialization logic herez
		InitializeInterface();
		Initialize();
	}

	#endregion

	#region Initialization

	private void Initialize()
	{
		int index = 0;
		for (int i = 0; i < 5; i++)
		{
			List<CalendarDay> days = new List<CalendarDay>();
			for (int y = 0; y < 7; y++)
			{
				CalendarDay calendarDay = Instantiate(m_calendarDayPrefab, m_calendarDayOriginPosition);
				calendarDay.transform.localPosition = GetPositionFromRowColumn(i, y);
				calendarDay.transform.forward = Vector3.down;
				calendarDay.Initialize(this, index, i, y);
				days.Add(calendarDay);

				index++;
			}
			m_calendarDays.Add(days);
		}

		RefreshCalendarDay();
	}

	private Vector3 GetPositionFromRowColumn(int row, int column)
	{
		return m_calendarDayOriginPosition.transform.position + new Vector3(column * OffsetColumn, 0f, -row * OffsetRow);
	}

	#endregion

	#region Calendar Interface

	[Header("Calendar Interface")]
	[SerializeField] private TextMeshPro m_monthText;
	[SerializeField] private Transform m_weekdaysLetterContainer;

	private void InitializeInterface()
	{
		m_monthText.text = MonthIndexToMonthName(CurrentMonthDate.Month) + " " + CurrentMonthDate.Year;

		int index = 0;
		foreach (Transform child in m_weekdaysLetterContainer)
		{
			child.localPosition = new Vector3(OffsetColumn * index, 0f, 0f);
			index++;
		}
	}

	public string MonthIndexToMonthName(int monthIndex)
	{
		string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(monthIndex);
		return char.ToUpper(monthName[0]) + monthName.Substring(1);
	}

	#endregion

	#region Data
		
	#endregion

}