using UnityEngine;

public class ProgressBonsaiApplication : ReworkedApplication
{
	public void DisplayProgressBonsaiScreen()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.ProgressBonsai);
	}
	
	public void DisplayTimerScreen()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.Timer);
	}
}