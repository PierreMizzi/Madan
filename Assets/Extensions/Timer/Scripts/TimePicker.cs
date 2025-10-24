using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PierreMizzi.Extensions.Timer
{


	public class TimePicker : MonoBehaviour
	{

		#region UI

		[Header("UI")]
		[SerializeField] private Button m_button;
		[SerializeField] private NumberPicker m_hoursNumberPicker;
		[SerializeField] private NumberPicker m_minutesNumberPicker;
		[SerializeField] private NumberPicker m_secondsNumberPicker;

		#endregion

		#region MonoBehaviour

		protected virtual void Awake()
		{
			onTimeChanged = (TimeSpan timeSpan) => { };
			m_button.onClick += CallbackTimer
		}

		protected virtual void Start()
		{
			if (m_hoursNumberPicker != null)
				m_hoursNumberPicker.onNumberPicked += CallbackHourNumberPicked;

			if (m_minutesNumberPicker != null)
				m_minutesNumberPicker.onNumberPicked += CallbackMinuteNumberPicked;

			if (m_secondsNumberPicker != null)
				m_secondsNumberPicker.onNumberPicked += CallbackSecondsNumberPicked;
		}

		protected virtual void OnDestroy()
		{
			if (m_hoursNumberPicker != null)
				m_hoursNumberPicker.onNumberPicked -= CallbackHourNumberPicked;

			if (m_minutesNumberPicker != null)
				m_minutesNumberPicker.onNumberPicked -= CallbackMinuteNumberPicked;

			if (m_secondsNumberPicker != null)
				m_secondsNumberPicker.onNumberPicked -= CallbackSecondsNumberPicked;
		}

		#endregion

		#region Behaviour

		protected int m_hours;
		protected int m_minutes;
		protected int m_seconds;

		public TimeSpan TimeSpan => new TimeSpan(m_hours, m_minutes, m_seconds);

		public TimespanDelegate onTimeChanged;

		private void CallbackHourNumberPicked(int hoursNumber)
		{
			m_hours = hoursNumber;
			onTimeChanged.Invoke(TimeSpan);
		}

		private void CallbackMinuteNumberPicked(int minutesNumber)
		{
			m_minutes = minutesNumber;
			onTimeChanged.Invoke(TimeSpan);
		}

		private void CallbackSecondsNumberPicked(int secondsNumber)
		{
			m_seconds = secondsNumber;
			onTimeChanged.Invoke(TimeSpan);
		}

		public void SetTime(TimeSpan timeSpan)
		{
			m_hours = timeSpan.Hours;
			m_hoursNumberPicker.SetNumber(m_hours);

			m_minutes = timeSpan.Minutes;
			m_minutesNumberPicker.SetNumber(m_minutes);

			m_seconds = timeSpan.Seconds;
			m_secondsNumberPicker.SetNumber(m_seconds);
		}

		#endregion
	}
}

