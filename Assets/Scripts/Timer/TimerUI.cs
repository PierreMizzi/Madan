using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : PierreMizzi.Extensions.Timer.TimerUI
{

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



	#region Behaviour

	[SerializeField] private Button m_startTimePickingButton;
	[SerializeField] private Button m_stopTimePickingButton;

	public Button StartTimePickingButton => m_startTimePickingButton;
	public Button StopTimePickingButton => m_stopTimePickingButton;

	public void CallbackStartTimePicking()
	{
		m_animator.SetBool(k_is_time_picking, true);
	}

	public void CallbackStopTimePicking()
	{
		m_animator.SetBool(k_is_time_picking, false);
	}

	#endregion

	#region Animation

	private Animator m_animator;

	private const string k_is_complete_param = "IsComplete";
	private const string k_is_time_picking = "IsTimePicking";


	#endregion
}