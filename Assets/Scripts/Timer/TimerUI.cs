using UnityEngine;
using UnityEngine.UI;

public class TimerUI : PierreMizzi.Extensions.Timer.TimerUI
{

	#region UI

	[SerializeField] private Button m_completePopUpRestartButton;

	public Button CompletePopUpRestartButton => m_completePopUpRestartButton;

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

		
	}

	private void CallbackRestartFromComplete()
	{

	}

	#endregion

	#region Animation

	private Animator m_animator;

	private const string k_is_complete_param = "IsComplete";

	private bool m_isComplete;

	#endregion
}