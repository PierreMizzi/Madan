using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : PierreMizzi.Extensions.Timer.TimerUI
{

	#region Complete PopUp

	[Header("Complete PopUp")]
	[SerializeField] private Button m_completePopUpRestartButton;

	public Button CompletePopUpRestartButton => m_completePopUpRestartButton;

	private bool m_isComplete;

	public bool IsComplete => m_isComplete;

	private Action onCompleteToBaseAnimEnd;

	public void CallbackRestartFromComplete()
	{
		m_animator.SetBool(k_is_complete_param, false);
	}

	public void CompleteToBaseAnimEnd()
	{
		m_isComplete = false;
	}

	#endregion

	#region MonoBehaviour

	protected void Awake()
	{
		m_animator = GetComponent<Animator>();
	}

	#endregion

	#region Behaviour

	public override void CallbackComplete()
	{
		base.CallbackComplete();

		m_animator.SetBool(k_is_complete_param, true);
		m_isComplete = true;
	}

	#endregion

	#region Animation

	private Animator m_animator;

	private const string k_is_complete_param = "IsComplete";


	#endregion
}