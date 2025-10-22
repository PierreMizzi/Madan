using System;
using UnityEngine;



public class Timer : PierreMizzi.Extensions.Timer.Timer
{

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

	private void RestartFromComplete()
	{
		onRestartFromComplete.Invoke();
		Restart();
	}

	#endregion

}