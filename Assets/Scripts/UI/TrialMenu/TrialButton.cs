using TMPro;
using UnityEngine;

public class TrialButton : MonoBehaviour
{

	#region Time

	[SerializeField] private TimeFrame m_timeFrame;

	#endregion

	#region UI

	[SerializeField] private TextMeshProUGUI m_textLabel;

	#endregion

	#region MonoBehaviour

	private void Start()
	{
		m_timeFrame.Initialize();
		ManageState();
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		ManageState();
	}

	#endregion

	#region Status

	private TrialButtonState m_state;

	private const string k_readyText = "Start Trial";
	private const string k_notReadyText = "Not ready !";
	private const string k_passedText = "Passed !";

	enum TrialButtonState
	{
		None,
		NotReady,
		Ready,
		Passed,
	}

	private void ManageState()
	{
		if (SaveManager.data.trial.hasBeenPassed)
			SetState(TrialButtonState.Passed);
		else
		{
			if (m_timeFrame.IsEarly())
				SetState(TrialButtonState.NotReady);
			else if (m_timeFrame.IsInTimeFrame())
				SetState(TrialButtonState.Ready);
		}
	}

	private void SetState(TrialButtonState state)
	{
		m_state = state;

		switch (m_state)
		{
			case TrialButtonState.NotReady:
				SetStateNotReady();
				break;
			case TrialButtonState.Ready:
				SetStateReady();
				break;
			case TrialButtonState.Passed:
				SetStatePassed();
				break;
		}
	}

	private void SetStateNotReady()
	{
		m_textLabel.text = k_notReadyText;
	}
	private void SetStateReady()
	{
		m_textLabel.text = k_readyText;
	}

	private void SetStatePassed()
	{
		m_textLabel.text = k_passedText;
	}

	#endregion

}

