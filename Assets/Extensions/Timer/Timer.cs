using System;
using System.Collections;
using UnityEngine;

// 🟩 : Done ! Reward screen
// 🟥 : Pomodoro save system
// 🟥 : Study quality stats
// 🟥 : Notifications
// 🟥 : Revamped UI
// 🟥 : Set the timer
// 🟥 : Study session
namespace PierreMizzi.Extensions.Timer
{
	
	public delegate void TimespanDelegate(TimeSpan timeSpan);

	public class Timer : MonoBehaviour
	{
		[SerializeField] protected TimerUI m_UI;

		[SerializeField] private Vector3Int m_totalDurationSettings;

		private PlayPauseStates m_state;
		private IEnumerator behaviourCoroutine;

		private TimeSpan oneSecond = new TimeSpan(0, 0, 1);

		private TimeSpan elapsedTime;
		private TimeSpan totalTime;
		private DateTime endTime;

		private TimeSpan RemainingTime => totalTime - elapsedTime;
		public double NormalizedProgress => elapsedTime.TotalMilliseconds / totalTime.TotalMilliseconds;
		public TimespanDelegate onRefreshRemainingTime;
		public Action<double> onRefreshProgress;

		#region MonoBehaviour

		private void Awake()
		{
			onRefreshRemainingTime = (TimeSpan timeSpan) => { };
			elapsedTime = new TimeSpan(0);
			SetDuration(m_totalDurationSettings.x, m_totalDurationSettings.y, m_totalDurationSettings.z);

			AssignView(m_UI);
		}

		protected void Update()
		{
			if (m_state == PlayPauseStates.Playing)
			{
				onRefreshProgress.Invoke(NormalizedProgress);
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus == false && m_state == PlayPauseStates.Playing)
			{
				// We regain focus on the app while chronometer is running
				elapsedTime = totalTime - (endTime - DateTime.Now);
				onRefreshRemainingTime.Invoke(RemainingTime);
				onRefreshProgress.Invoke(NormalizedProgress);

				if (RemainingTime.TotalSeconds <= 0)
				{
					Complete();
				}
			}
		}

		#endregion

		#region Behaviour

		private void StartBehaviour()
		{
			if (behaviourCoroutine == null)
			{
				behaviourCoroutine = Behaviour();
				StartCoroutine(behaviourCoroutine);
			}
		}

		public void StopBehaviour()
		{
			if (behaviourCoroutine != null)
			{
				StopCoroutine(behaviourCoroutine);
				behaviourCoroutine = null;
			}
		}

		public IEnumerator Behaviour()
		{
			while (true)
			{
				if (RemainingTime.TotalSeconds <= 0)
				{
					Complete();
					yield break;
				}

				yield return new WaitForSeconds(1);

				elapsedTime = elapsedTime.Add(oneSecond);
				onRefreshRemainingTime.Invoke(RemainingTime);
			}
		}

		protected virtual void Complete()
		{
			StopBehaviour();
			onComplete.Invoke();
			onRefreshProgress.Invoke(1.0f);
			m_state = PlayPauseStates.Completed;
		}

		#endregion

		#region Controls

		public Action onPlay;
		public Action onPause;
		public Action onRestart;
		public Action onComplete;

		public virtual void Play()
		{
			if (m_state == PlayPauseStates.None)
			{
				endTime = DateTime.Now + totalTime;
				elapsedTime = new TimeSpan(0, 0, 0);
			}

			m_state = PlayPauseStates.Playing;
			StartBehaviour();
			onPlay.Invoke();
		}

		public virtual void Pause()
		{
			m_state = PlayPauseStates.Paused;
			StopBehaviour();
			onPause.Invoke();
		}

		public virtual void Restart()
		{
			// hasStarted = false;
			m_state = PlayPauseStates.None;

			StopBehaviour();
			SetDuration(m_totalDurationSettings.x, m_totalDurationSettings.y, m_totalDurationSettings.z);
			elapsedTime = new TimeSpan(0);
			onRefreshRemainingTime.Invoke(RemainingTime);
			onRefreshProgress.Invoke(0);
			onRestart.Invoke();
		}

		public virtual void SetDuration(int hours, int minutes, int seconds)
		{
			totalTime = new TimeSpan(hours, minutes, seconds);
		}

		#endregion

		#region View

		protected virtual void AssignView(TimerUI UI)
		{
			if (UI == null)
			{
				return;
			}

			UI.PlayButton.onClick.AddListener(Play);
			UI.PauseButton.onClick.AddListener(Pause);
			UI.RestartButton.onClick.AddListener(Restart);

			onPlay += UI.CallbackPlay;
			onPause += UI.CallbackPause;
			onRestart += UI.CallbackRestart;
			onComplete += UI.CallbackComplete;

			onRefreshRemainingTime += UI.CallbackRefreshDuration;
			onRefreshRemainingTime.Invoke(totalTime);

			onRefreshProgress += UI.CallbackRefreshProgress;
			onRefreshProgress.Invoke(0);
		}

		#endregion

		#region App Focus

		private void LoseFocus()
		{

		}

		private void GainFocus()
		{

		}

		#endregion
	}

}
