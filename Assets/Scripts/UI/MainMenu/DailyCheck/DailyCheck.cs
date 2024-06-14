using System;
using TMPro;
using UnityEngine;

public class DailyCheck : MonoBehaviour
{

	[SerializeField] private ApplicationChannel m_applicationChannel;

	#region Status

	[SerializeField] private DailyCheckType m_type;

	private DailyCheckData data;

	#endregion

	#region Hours

	[SerializeField] private TextMeshProUGUI m_availableHoursLabel;

	[SerializeField] private string m_startHourSettings;
	[SerializeField] private string m_endHourSettings;

	private DateTime m_startTime;
	private DateTime m_endTime;

	#endregion

	#region MonoBehaviour

	private void Awake()
	{
		m_startTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + m_startHourSettings);
		m_endTime = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + m_endHourSettings);

		m_availableHoursLabel.text = $"{m_startTime:t}-{m_endTime:t}";

		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
			m_applicationChannel.onCheckDailyCheck += CallbackCheck;
			m_applicationChannel.onRefreshDailyCheck += CallbackRefresh;
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
			m_applicationChannel.onCheckDailyCheck -= CallbackCheck;
			m_applicationChannel.onRefreshDailyCheck -= CallbackRefresh;
		}
	}

	#endregion

	#region Status

	private CommonStatus m_state;
	[SerializeField] private TextMeshProUGUI m_textLabel;

	private bool isEarly { get { return DateTime.Now < m_startTime; } }
	private bool isInTimeFrame { get { return m_startTime < DateTime.Now && DateTime.Now < m_endTime; } }
	private bool isLate { get { return DateTime.Now > m_endTime; } }

	private void CallbackAppDataLoaded()
	{
		switch (m_type)
		{
			case DailyCheckType.Morning:
				data = SaveManager.data.dailyCheckMorning;
				break;

			case DailyCheckType.Noon:
				data = SaveManager.data.dailyCheckNoon;
				break;

			case DailyCheckType.Evening:
				data = SaveManager.data.dailyCheckEvening;
				break;
			default:
				break;
		}

		ManageData();
		ManageState();
	}

	private void CallbackCheck(DailyCheckType type)
	{
		if (type == m_type)
		{
			data.checkTime = DateTime.Now;
			data.hasBeenChecked = true;
			SetState(CommonStatus.Checked);
		}
	}

	private void CallbackRefresh()
	{
		ManageState();
	}

	/// <summary> 
	///	If it's a new day, we reset DailyCHeckData
	/// </summary>
	private void ManageData()
	{
		// Debug.Log($"{DateTime.Now:d}");
		// Debug.Log($"{data.checkTime:d}");

		// Debug.Log($"{DateTime.Now.ToShortDateString()}");
		// Debug.Log($"{data.checkTime.ToShortDateString()}");

		if (DateTime.Now.ToShortDateString() != data.checkTime.ToShortDateString())
			Clear();
	}

	private void ManageData(DateTime time)
	{
		// Debug.Log($"{time:d}");
		// Debug.Log($"{data.checkTime:d}");

		// Debug.Log($"{time.ToShortDateString()}");
		// Debug.Log($"{data.checkTime.ToShortDateString()}");

		if (time.ToShortDateString() != data.checkTime.ToShortDateString())
			Clear();
	}

	public void ManageState()
	{
		if (data.hasBeenChecked)
			SetState(CommonStatus.Checked);

		else
		{
			if (isEarly)
				SetState(CommonStatus.Unavailable);

			else if (isInTimeFrame)
				SetState(CommonStatus.Available);

			else if (isLate)
				SetState(CommonStatus.Missed);
		}
	}

	private void ManageState(DateTime time)
	{
		if (data.hasBeenChecked)
			SetState(CommonStatus.Checked);

		else
		{
			if (time < m_startTime)
				SetState(CommonStatus.Unavailable);

			else if (m_startTime < time && time < m_endTime)
				SetState(CommonStatus.Available);

			else if (time > m_endTime)
				SetState(CommonStatus.Missed);
		}
	}

	private void SetState(CommonStatus state)
	{
		m_state = state;

		switch (m_state)
		{
			case CommonStatus.Unavailable:
				SetState_Unavailable();
				break;
			case CommonStatus.Available:
				SetState_Available();
				break;
			case CommonStatus.Checked:
				SetState_Checked();
				break;
			case CommonStatus.Missed:
				SetState_Missed();
				break;
			default:
				break;
		}
	}

	private void SetState_Unavailable()
	{
		m_textLabel.text = "Wait";
		m_animator.SetInteger(k_state, (int)CommonStatus.Unavailable);
	}

	private void SetState_Available()
	{
		m_textLabel.text = "Available";
		m_animator.SetInteger(k_state, (int)CommonStatus.Available);
	}

	private void SetState_Checked()
	{
		m_textLabel.text = "Checked";
		m_animator.SetInteger(k_state, (int)CommonStatus.Checked);

	}

	private void SetState_Missed()
	{
		m_textLabel.text = "Missed";
		m_animator.SetInteger(k_state, (int)CommonStatus.Missed);
	}

	#endregion

	#region Animation

	[SerializeField] private Animator m_animator;
	public const string k_state = "Status";

	#endregion

	#region OnClick

	public const string k_dailyCheckOption = "DailyCheck";

	public void OnClick()
	{
		if (isEarly)
			Debug.Log("Too early !");

		else if (isInTimeFrame && !data.hasBeenChecked)
			m_applicationChannel.onDisplayScreen.Invoke(
				ApplicationScreenType.DailyWordHistory,
				k_dailyCheckOption,
				(int)m_type + ""
			);

		else if (isLate)
			Debug.Log("Too late !");
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

		ManageData(m_debugTime);
		ManageState(m_debugTime);
	}

	[ContextMenu("Set Checked")]
	public void SetChecked()
	{
		data.hasBeenChecked = true;
		SaveManager.Save();

		Test();
	}

	[ContextMenu("Clear")]
	public void Clear()
	{
		Debug.Log("Cleared !");
		data.checkTime = DateTime.Now;
		data.hasBeenChecked = false;
		SaveManager.Save();
	}

	#endregion

}