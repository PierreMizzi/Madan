using System;
using UnityEngine;

// 🟩 : Done ! Reward screen
// 🟩 : Pomodoro save system
// 🟩 : Study quality stats
// 🟧 : Set the timer
// 🟥 : Notifications
// 🟥 : Revamped UI
// 🟥 : Study session

public delegate void TimerCompleteDelegate(Pomodoro pomodoro);

public class Timer : PierreMizzi.Extensions.Timer.Timer
{

	#region MonoBehaviour

	protected override void Awake()
	{
		base.Awake();
		onSavePomodoro = (Pomodoro pomodoro) => { };
	}
		
	#endregion

	#region View

	public TimerUI UI => m_UI as TimerUI;

	protected override void AssignView(PierreMizzi.Extensions.Timer.TimerUI View)
	{
		base.AssignView(View);

		// UI
		UI.CompletePopUpRestartButton.onClick.AddListener(RestartFromComplete);

		onRestartFromComplete += UI.CallbackRestartFromComplete;
	}

	#endregion

	#region Behaviour

	public Action onRestartFromComplete;

	public TimerCompleteDelegate onSavePomodoro;

	public override void Play()
	{
		if (m_state == PlayPauseStates.None)
		{
			m_pomodoro = new Pomodoro()
			{
				startTime = DateTime.Now
			};
		}

		base.Play();
	}

	protected override void Complete()
	{
		base.Complete();

		if (m_pomodoro != null)
		{
			m_pomodoro.endTime = DateTime.Now;
			m_pomodoro.focus = UI.FocusValue;
			onSavePomodoro.Invoke(m_pomodoro);
		}
	}

	public override void Restart()
	{
		base.Restart();

		m_pomodoro = null;
	}

	private void RestartFromComplete()
	{
		onRestartFromComplete.Invoke();
		Restart();
	}

	#endregion

	#region Save

	protected Pomodoro m_pomodoro;


	#endregion

}

