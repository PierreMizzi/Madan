using System;
using TMPro;
using UnityEngine;

/*
3 states :
 - Not Available
 - Available
 - Checked
 - Missed
*/

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
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;

	}


	private void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;
	}


	#endregion

	#region Status

	private DailyCheckStatus m_status;
	[SerializeField] private TextMeshProUGUI m_statusLabel;

	private bool isEarly { get { return DateTime.Now < m_startTime; } }
	private bool isInTimeFrame { get { return m_startTime < DateTime.Now && DateTime.Now < m_endTime; } }
	private bool isLate { get { return DateTime.Now > m_endTime; } }

	private void CallbackAppDataLoaded()
	{
		Debug.Log("CallbackAppDataLoaded");
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
		ManageStatus();
	}

	/// <summary> 
	///	If it's a new day, we reset DailyCHeckData
	/// </summary>
	private void ManageData()
	{
		Debug.Log($"{DateTime.Now:d}");
		Debug.Log($"{data.checkTime:d}");

		Debug.Log($"{DateTime.Now.ToShortDateString()}");
		Debug.Log($"{data.checkTime.ToShortDateString()}");

		if (DateTime.Now.ToShortDateString() != data.checkTime.ToShortDateString())
			Clear();
	}

	private void ManageData(DateTime time)
	{
		Debug.Log($"{time:d}");
		Debug.Log($"{data.checkTime:d}");

		Debug.Log($"{time.ToShortDateString()}");
		Debug.Log($"{data.checkTime.ToShortDateString()}");

		if (time.ToShortDateString() != data.checkTime.ToShortDateString())
			Clear();
	}

	private void ManageStatus()
	{
		if (data.hasBeenChecked)
			SetStatus(DailyCheckStatus.Checked);

		else
		{
			if (isEarly)
				SetStatus(DailyCheckStatus.Unavailable);

			else if (isInTimeFrame)
				SetStatus(DailyCheckStatus.Available);

			else if (isLate)
				SetStatus(DailyCheckStatus.Missed);
		}
	}

	private void ManageStatus(DateTime time)
	{
		if (data.hasBeenChecked)
			SetStatus(DailyCheckStatus.Checked);

		else
		{
			if (time < m_startTime)
				SetStatus(DailyCheckStatus.Unavailable);

			else if (m_startTime < time && time < m_endTime)
				SetStatus(DailyCheckStatus.Available);

			else if (time > m_endTime)
				SetStatus(DailyCheckStatus.Missed);
		}
	}

	private void SetStatus(DailyCheckStatus status)
	{
		m_status = status;

		switch (m_status)
		{
			case DailyCheckStatus.Unavailable:
				SetStatus_Unavailable();
				break;
			case DailyCheckStatus.Available:
				SetStatus_Available();
				break;
			case DailyCheckStatus.Checked:
				SetStatus_Checked();
				break;
			case DailyCheckStatus.Missed:
				SetStatus_Missed();
				break;
			default:
				break;
		}
	}

	private void SetStatus_Unavailable()
	{
		m_statusLabel.text = "Wait";
		m_animator.SetInteger(k_status, (int)DailyCheckStatus.Unavailable);
	}

	private void SetStatus_Available()
	{
		m_statusLabel.text = "Available";
		m_animator.SetInteger(k_status, (int)DailyCheckStatus.Available);
	}

	private void SetStatus_Checked()
	{
		m_statusLabel.text = "Checked";
		m_animator.SetInteger(k_status, (int)DailyCheckStatus.Checked);
	}

	private void SetStatus_Missed()
	{
		m_statusLabel.text = "Missed";
		m_animator.SetInteger(k_status, (int)DailyCheckStatus.Missed);
	}

	#endregion

	#region Animation

	[SerializeField] private Animator m_animator;
	private const string k_status = "Status";

	#endregion

	#region OnClick

	public void OnClick()
	{
		if (m_debugTime < m_startTime)
			Debug.Log("Too early !");

		else if (m_startTime < m_debugTime && m_debugTime < m_endTime)
			Debug.Log("Right on time !");

		else if (m_debugTime > m_endTime)
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
		ManageStatus(m_debugTime);
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