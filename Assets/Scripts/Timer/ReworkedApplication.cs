using UnityEngine;

public class ReworkedApplication : MonoBehaviour
{
	[SerializeField] private ApplicationChannel m_applicationChannel;

	#region MonoBehaviour

	protected virtual void Start()
	{
		LoadAppData();
	}

	#endregion

	#region Save Manager

	public void LoadAppData()
	{
		SaveManager.Load();
		m_applicationChannel.onAppDataLoaded.Invoke();
	}
		
	#endregion

}
