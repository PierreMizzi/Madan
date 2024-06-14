using System;
using TMPro;
using UnityEngine;

public class TrialButton : MonoBehaviour
{

	#region Application
	[SerializeField] private ApplicationChannel m_applicationChannel;

	private void CallbackAppDataLoaded()
	{
		ManageState();
	}

	#endregion

	#region Behaviour

	[SerializeField] private TimeFrame m_timeFrame;

	[SerializeField] private TextMeshProUGUI m_textLabel;

	public void OnClick()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.TrialMenu);

		return;
		if (m_timeFrame.IsEarly())
			Debug.Log("Too early !");

		else if (m_timeFrame.IsInTimeFrame() && !SaveManager.data.trial.hasBeenPassed)
			m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.TrialMenu);

		else if (m_timeFrame.IsLate())
			Debug.Log("Too late !");
	}

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		m_timeFrame.Initialize();

		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		if (SaveManager.data != null && focusStatus)
			ManageState();
	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;
		}
	}

	#endregion

	#region Status

	private CommonStatus m_state;

	private void ManageState()
	{
		if (SaveManager.data.trial.hasBeenPassed)
			SetState(CommonStatus.Checked);
		else
		{
			if (m_timeFrame.IsEarly())
				SetState(CommonStatus.Unavailable);
			else if (m_timeFrame.IsInTimeFrame())
				SetState(CommonStatus.Available);
		}
	}

	private void SetState(CommonStatus state)
	{
		m_state = state;

		switch (m_state)
		{
			case CommonStatus.Unavailable:
				SetStatus_Unavailable();
				break;
			case CommonStatus.Available:
				SetStatus_Available();
				break;
			case CommonStatus.Checked:
				SetStatus_Checked();
				break;
		}
	}

	#endregion

	#region Animation

	[SerializeField] private Animator m_animator;

	private void SetStatus_Unavailable()
	{
		m_textLabel.text = "Not yet !";
		m_animator.SetInteger(DailyCheck.k_state, (int)CommonStatus.Unavailable);
	}

	private void SetStatus_Available()
	{
		m_textLabel.text = "Start <size=110%>Trial !</size>";
		m_animator.SetInteger(DailyCheck.k_state, (int)CommonStatus.Available);
	}

	private void SetStatus_Checked()
	{
		m_textLabel.text = "Checked";
		m_animator.SetInteger(DailyCheck.k_state, (int)CommonStatus.Checked);

	}

	#endregion

	#region Debug

	[Header("Debug")]
	[SerializeField] private string m_debugTimeString;
	private DateTime m_debugTime;




	[ContextMenu("Test")]
	public void Test()
	{
		m_debugTime = DateTime.Parse(m_debugTimeString);

		ManageState(m_debugTime);
	}

	private void ManageState(DateTime now)
	{
		if (SaveManager.data.trial.hasBeenPassed)
			SetState(CommonStatus.Checked);
		else
		{
			if (m_timeFrame.IsEarly(now))
				SetState(CommonStatus.Unavailable);
			else if (m_timeFrame.IsInTimeFrame(now))
				SetState(CommonStatus.Available);
		}
	}

	[ContextMenu("Set Checked")]
	public void SetChecked()
	{
		Debug.Log("Set Checked");
		SaveManager.data.trial.hasBeenPassed = true;
		SaveManager.Save();

		Test();
	}

	[ContextMenu("Clear")]
	public void Clear()
	{
		Debug.Log("Cleared !");
		SaveManager.data.trial.hasBeenPassed = false;
		SaveManager.data.trial.passDate = DateTime.Now;
		SaveManager.Save();
	}

	#endregion

	/*

		Idée refacto dans ce script
	
		TODO : StatePattern

		TODO : TimeframeElement

		TODO : DataLinkedElement
	
	*/


}

