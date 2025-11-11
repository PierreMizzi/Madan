using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using PierreMizzi.Useful;

namespace PierreMizzi.Extensions.Timer
{
	public class TimerUI : MonoBehaviour
	{
		#region Behaviour

		public void Initialize()
		{
			m_playButton.gameObject.SetActive(true);
			m_pauseButton.gameObject.SetActive(false);
			m_restartButton.gameObject.SetActive(false);

			m_timePicker.Initialize();
		}

		public virtual void CallbackRefreshRemaining(TimeSpan timeSpan)
		{
			m_durationText.text = RefreshDurationText(timeSpan);
		}

		public virtual  void CallbackSetTotalTime(TimeSpan timeSpan)
		{
			m_durationText.text = RefreshDurationText(timeSpan);
			m_timePicker.SetTime(timeSpan);
		}

		protected string RefreshDurationText(TimeSpan timeSpan)
		{
			if (timeSpan.Hours > 0)
			{
				return $"{timeSpan.Hours}:{CorrectMinutes(timeSpan.Minutes)}:{CorrectSeconds(timeSpan.Seconds)}";
			}
			else if (timeSpan.Minutes > 0)
			{
				return $"{timeSpan.Minutes}:{CorrectSeconds(timeSpan.Seconds)}";
			}
			else if (timeSpan.Seconds > 0)
			{
				return $"{timeSpan.Seconds}";
			}
			else
			{
				return "";
			}
		}

		private string CorrectMinutes(int value)
		{
			if (value == 0)
			{
				return "00";
			}
			else return value.ToString();
		}

		private string CorrectSeconds(int value)
		{
			if (value == 0)
			{
				return "00";
			}
			else return UtilsClass.TwoDigit(value.ToString());
		}

		public virtual void CallbackPlay()
		{
			m_playButton.gameObject.SetActive(false);
			m_pauseButton.gameObject.SetActive(true);
			m_restartButton.gameObject.SetActive(true);
		}

		public virtual void CallbackPause()
		{
			m_playButton.gameObject.SetActive(true);
			m_pauseButton.gameObject.SetActive(false);
			m_restartButton.gameObject.SetActive(false);
		}

		public virtual void CallbackRestart()
		{
			m_playButton.gameObject.SetActive(true);
			m_pauseButton.gameObject.SetActive(false);
			m_restartButton.gameObject.SetActive(false);
		}

		public virtual void CallbackComplete()
		{
			m_playButton.gameObject.SetActive(false);
			m_pauseButton.gameObject.SetActive(false);
			m_restartButton.gameObject.SetActive(true);
		}

		public void CallbackRefreshProgress(double progress)
		{
			m_background.fillAmount = (float)progress;
		}

		#endregion

		#region UI

		[Header("UI")]
		[SerializeField] protected TextMeshProUGUI m_durationText;

		[SerializeField] protected Button m_playButton;
		[SerializeField] protected Button m_pauseButton;
		[SerializeField] protected Button m_restartButton;

		[SerializeField] protected Image m_background;

		[SerializeField] protected TimePicker m_timePicker;

		public Button PlayButton => m_playButton;
		public Button PauseButton => m_pauseButton;
		public Button RestartButton => m_restartButton;

		public TimePicker TimePicker => m_timePicker;

		#endregion


	}

}
