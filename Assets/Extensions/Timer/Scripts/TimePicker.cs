using System;
using UnityEngine;
using UnityEngine.UI;

namespace PierreMizzi.Extensions.Timer
{

	public class TimePicker : MonoBehaviour
	{

		#region UI

		[Header("UI")]
		[SerializeField] private NumberPicker m_hoursNumberPicker;
		[SerializeField] private NumberPicker m_minutesNumberPicker;
		[SerializeField] private NumberPicker m_secondsNumberPicker;

		#endregion

		#region MonoBehaviour

		protected virtual void Awake()
		{
			onTimePicked = (TimeSpan timeSpan) => { };
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

		public TimespanDelegate onTimePicked;

		public void Initialize()
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
			m_hoursNumberPicker.Populate();
			m_minutesNumberPicker.Populate();
			m_secondsNumberPicker.Populate();
		}

		private void CallbackHourNumberPicked(int hoursNumber)
		{
			m_hours = hoursNumber;
			onTimePicked.Invoke(TimeSpan);
		}

		private void CallbackMinuteNumberPicked(int minutesNumber)
		{
			m_minutes = minutesNumber;
			onTimePicked.Invoke(TimeSpan);
		}

		private void CallbackSecondsNumberPicked(int secondsNumber)
		{
			m_seconds = secondsNumber;
			onTimePicked.Invoke(TimeSpan);
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

