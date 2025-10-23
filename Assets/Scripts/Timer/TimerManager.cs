using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	#region Behaviour

	[SerializeField] private ApplicationChannel m_applicationChannel;

	[SerializeField] private Timer m_timer;

	#endregion

	#region MonoBehaviour

	protected virtual void Start()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;

		if (m_timer != null)
			m_timer.onSavePomodoro += CallbackSavePomodoro;

	}

	protected virtual void OnDestroy()
	{
		if (m_applicationChannel != null)
			m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;

		if (m_timer != null)
			m_timer.onSavePomodoro -= CallbackSavePomodoro;
	}

	private void CallbackAppDataLoaded()
	{
		LoadData();
	}

	private void CallbackSavePomodoro(Pomodoro pomodoro)
	{
		savedData.pomodoros.Add(pomodoro);
		Save();
	}

	#endregion

	#region Save Manager

	public TimerData savedData
	{
		get
		{
			if (SaveManager.data != null && SaveManager.data.timerData != null)
			{
				return SaveManager.data.timerData;
			}
			else
				return null;
		}
	}

	public void LoadData()
	{
		if (savedData == null)
		{
			SaveManager.data.timerData = new TimerData();
		}

		SaveManager.Save();
	}

	public void Save()
	{
		if (savedData == null)
		{
			return;
		}

		SaveManager.Save();
	}

	#endregion
}