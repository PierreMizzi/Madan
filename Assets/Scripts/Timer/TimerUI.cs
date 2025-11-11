using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : PierreMizzi.Extensions.Timer.TimerUI
{

	#region Behaviour

	public override void CallbackRefreshRemaining(TimeSpan timeSpan)
	{
		base.CallbackRefreshRemaining(timeSpan);

		m_notepadDurationText.text = RefreshDurationText(timeSpan);
	}

	public override void CallbackSetTotalTime(TimeSpan timeSpan)
	{
		base.CallbackSetTotalTime(timeSpan);

		m_notepadDurationText.text = RefreshDurationText(timeSpan);
	}

	public override void CallbackPlay()
	{
		base.CallbackPlay();
		m_startTimePickingButton.gameObject.SetActive(false);
	}

	public override void CallbackPause()
	{
		base.CallbackPause();
	}

	public override void CallbackRestart()
	{
		base.CallbackRestart();
		m_startTimePickingButton.gameObject.SetActive(true);
	}

	#endregion

	#region Complete PopUp

	[Header("Complete PopUp")]
	[SerializeField] private Button m_completePopUpRestartButton;
	[SerializeField] private Slider m_focusSlider;

	public Button CompletePopUpRestartButton => m_completePopUpRestartButton;

	public float FocusValue => m_focusSlider.value;

	private Action onCompleteToBaseAnimEnd;

	public void CallbackRestartFromComplete()
	{
		m_animator.SetBool(k_is_complete_param, false);
	}

	[Obsolete]
	public void CompleteToBaseAnimEnd()
	{
		// m_isComplete = false;
	}

	public override void CallbackComplete()
	{
		base.CallbackComplete();

		m_animator.SetBool(k_is_complete_param, true);
	}

	#endregion

	#region MonoBehaviour

	protected void Awake()
	{
		m_animator = GetComponent<Animator>();
	}

	#endregion

	#region Time Picker

	[Header("Time Picker")]
	[SerializeField] private Button m_startTimePickingButton;
	[SerializeField] private Button m_stopTimePickingButton;

	public Button StartTimePickingButton => m_startTimePickingButton;
	public Button StopTimePickingButton => m_stopTimePickingButton;

	public Action OnClickStartTimePicking;
	public Action OnClickStopTimePicking;

	public void CallbackStartTimePicking()
	{
		m_animator.SetBool(k_is_time_picking, true);
	}

	public void CallbackStopTimePicking()
	{
		m_animator.SetBool(k_is_time_picking, false);
		m_startTimePickingButton.gameObject.SetActive(true);
	}



	#endregion

	#region Notepad

	[Header("Notepad")]

	[SerializeField] private Button m_notepadOpenButton;
	[SerializeField] private Button m_notepadCloseButton;
	[SerializeField] private TMP_InputField m_notepadInputField;
	[SerializeField] private TMP_Text m_notepadDurationText;

	public Button NotepadOpenButton => m_notepadOpenButton;
	public Button NotepadCloseButton => m_notepadCloseButton;

	public void CallbackOpenNotepad()
	{
		m_animator.SetBool(k_is_notepad, true);
	}

	public void CallbackCloseNotepad()
	{
		m_animator.SetBool(k_is_notepad, false);
	}

	#endregion

	#region Animation

	private Animator m_animator;

	private const string k_is_complete_param = "IsComplete";
	private const string k_is_time_picking = "IsTimePicking";
	private const string k_is_notepad = "IsNotepad";


	#endregion
}