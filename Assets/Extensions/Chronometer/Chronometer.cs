using System;
using System.Collections;
using UnityEngine;

public delegate void TimespanDelegate(TimeSpan timeSpan);

public class Chronometer : MonoBehaviour
{
	[SerializeField] private ChronometerUI m_chronometerUI;

	[SerializeField] private Vector3Int m_customDuration;

	private bool hasStarted = false;
	private IEnumerator behaviourCoroutine;

	private TimeSpan duration = new TimeSpan(0, 30, 0);
	private TimeSpan totalDuration;
	public TimeSpan Duration => duration;
	public TimespanDelegate onRefreshDuration;
	private TimeSpan oneSecond = new TimeSpan(0, 0, 1);


	public double NormalizedProgress => 1.0f - (duration.TotalSeconds / totalDuration.TotalSeconds);
	public Action<double> onRefreshProgress;

	#region MonoBehaviour

	private void Awake()
	{
		onRefreshDuration = (TimeSpan timeSpan) => { };
		SetDuration(m_customDuration.x, m_customDuration.y, m_customDuration.z);

		AssignView(m_chronometerUI);
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
		while(true)
		{
			if (duration.TotalSeconds <= 0)
			{
				StopBehaviour();
				yield break;
			}

			yield return new WaitForSeconds(1);

			duration = duration.Subtract(oneSecond);
			onRefreshDuration.Invoke(duration);
			onRefreshProgress.Invoke(NormalizedProgress);
		}
	}

	#endregion

	#region Controls

	public Action onPlay;
	public Action onPause;
	public Action onRestart;

	public void Play()
	{
		if (hasStarted == false)
		{
			totalDuration = new TimeSpan(duration.Hours, duration.Minutes, duration.Seconds);
			hasStarted = true;
		}

		StartBehaviour();
		onPlay.Invoke();
	}

	public void Pause()
	{
		StopBehaviour();
		onPause.Invoke();
	}

	public void Restart()
	{
		hasStarted = false;

		StopBehaviour();
		SetDuration(m_customDuration.x, m_customDuration.y, m_customDuration.z);
		onRefreshDuration.Invoke(duration);
		onRefreshProgress.Invoke(0);
		onRestart.Invoke();
	}

	public void SetDuration(int hours, int minutes, int seconds)
	{
		duration = new TimeSpan(hours, minutes, seconds);
	}

	#endregion

	#region View

	private void AssignView(ChronometerUI UI)
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

		onRefreshDuration += UI.CallbackRefreshDuration;
		onRefreshDuration.Invoke(duration);

		onRefreshProgress += UI.CallbackRefreshProgress;
		onRefreshProgress.Invoke(0);
	}

	#endregion


}