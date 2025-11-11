using System;
using PierreMizzi.Extensions.Timer;
using UnityEngine;

// 🟩 : Done ! Reward screen
// 🟩 : Pomodoro save system
// 🟩 : Study quality stats
// 🟩 : Set the timer
// 🟥 : Notifications
// 🟥 : Revamped UI
// 🟥 : Study session
// 🟥 : Add pre-set durations

public delegate void TimerCompleteDelegate(StudyTime pomodoro);

public class Timer : PierreMizzi.Extensions.Timer.Timer
{

	#region MonoBehaviour

	protected override void Awake()
	{
		base.Awake();
		onStudyTimeCompleted = (StudyTime studyTime) => { };
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

		UI.StartTimePickingButton.onClick.AddListener(CallbackStartTimePicking);
		onStartTimePicking += UI.CallbackStartTimePicking;

		UI.StopTimePickingButton.onClick.AddListener(CallbackStopTimePicking);
		onStopTimePicking += UI.CallbackStopTimePicking;

		// Notepad
		UI.NotepadOpenButton.onClick.AddListener(CallbackOpenNotepad);
		onOpenNotepad += UI.CallbackOpenNotepad;

		UI.NotepadCloseButton.onClick.AddListener(CallbackCloseNotepad);
		onCloseNotepad += UI.CallbackCloseNotepad;
	}

	#endregion

	#region Behaviour

	public Action onRestartFromComplete;

	public TimerCompleteDelegate onStudyTimeCompleted;

	public Action onStartTimePicking;
	private Action onStopTimePicking;

	public override void Play()
	{
		if (m_state == PlayPauseStates.None)
		{
			m_currentStudyTime = new StudyTime()
			{
				category = "Study",
				totalTime = totalTime,
				startTime = DateTime.Now,
			};
		}

		base.Play();
	}

	protected override void Complete()
	{
		base.Complete();

		if (m_currentStudyTime != null)
		{
			m_currentStudyTime.endTime = DateTime.Now;
			m_currentStudyTime.focus = UI.FocusValue;
			onStudyTimeCompleted.Invoke(m_currentStudyTime);
		}
	}

	public override void Restart()
	{
		base.Restart();

		m_currentStudyTime = null;
	}

	private void RestartFromComplete()
	{
		onRestartFromComplete.Invoke();
		Restart();
	}

	private void CallbackStartTimePicking()
	{
		if (m_state != PlayPauseStates.None)
		{
			return;
		}

		onStartTimePicking.Invoke();
	}

	private void CallbackStopTimePicking()
	{
		onStopTimePicking.Invoke();
	}

	#endregion

	#region Save

	protected StudyTime m_currentStudyTime;


	#endregion

	#region Notepad

	public Action onOpenNotepad;
	public Action onCloseNotepad;

	private void CallbackOpenNotepad()
	{
		onOpenNotepad.Invoke();
	}

	private void CallbackCloseNotepad()
	{
		onCloseNotepad.Invoke();
	}

	#endregion

}

