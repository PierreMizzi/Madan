using UnityEngine;

public class ProgressBonsaiApplication : ReworkedApplication
{

	#region MonoBehaviour

	protected override void Start()
	{
		base.Start();

		DisplayTimerScreen();
	}
		
	#endregion

	#region Reworked Application
		
	public void DisplayProgressBonsaiScreen()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.ProgressBonsai);
	}

	public void DisplayTimerScreen()
	{
		m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.Timer);
	}
	
	#endregion
}