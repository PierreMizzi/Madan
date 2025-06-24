using System;
using Unity.Notifications.Android;
using UnityEngine;

/*
	https://docs.unity3d.com/Packages/com.unity.mobile.notifications@2.3/manual/Android.html
*/

public class NotificationManager : MonoBehaviour
{
	#region MonoBehaviour

	private void Start()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded += Initialize;
		}
	}

	private void OnDestroy()
	{
		if (m_applicationChannel != null)
		{
			m_applicationChannel.onAppDataLoaded -= Initialize;
		}
	}

	#endregion

	#region Behaviour

	[SerializeField] private ApplicationChannel m_applicationChannel;

	private AndroidNotificationChannelGroup group;
	private AndroidNotificationChannel channel;

	private const string k_mainChannelID = "420";

	public NotificationManagerSaveData SavedData
	{
		get
		{
			return SaveManager.data.notificationManagerData;
		}
	}

	public void Initialize()
	{
		AndroidNotificationCenter.Initialize();

		// Creates a group & register it
		group = new AndroidNotificationChannelGroup()
		{
			Id = "Main",
			Name = "Main notifications",
		};
		AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

		// Creates a channel & register it
		channel = new AndroidNotificationChannel()
		{
			Id = k_mainChannelID,
			Name = "Default Channel",
			Importance = Importance.Default,
			Description = "Generic notifications",
			Group = "Main",  // must be same as Id of previously registered group
		};
		AndroidNotificationCenter.RegisterNotificationChannel(channel);

		ManageDailyCheckNotifications();

	}

	#endregion

	#region Daily Check

	[Header("Daily Check Notifications")]

	[SerializeField] private NotificationSettings m_morningDailyCheckSettings;
	[SerializeField] private NotificationSettings m_afternoonDailyCheckSettings;
	[SerializeField] private NotificationSettings m_eveningDailyCheckSettings;
	[SerializeField] private NotificationSettings m_trialSettings;

	private void ManageDailyCheckNotifications()
	{
		bool needsSave = false;

		// Morning
		if (SavedData.morningNotificationID == 0)
		{
			AndroidNotification morningNotification = m_morningDailyCheckSettings.Create();
			SavedData.morningNotificationID = AndroidNotificationCenter.SendNotification(morningNotification, channel.Id);
			needsSave = true;
		}

		// Afternoon
		AndroidNotification afternoonNotification = m_afternoonDailyCheckSettings.Create();
		int afternoonNotificationID = AndroidNotificationCenter.SendNotification(afternoonNotification, channel.Id);

		// Evening
		AndroidNotification eveningNotification = m_eveningDailyCheckSettings.Create();
		int eveningNotificationID = AndroidNotificationCenter.SendNotification(eveningNotification, channel.Id);

		// Evening
		AndroidNotification trialNotification = m_trialSettings.Create();
		int trialNotificationID = AndroidNotificationCenter.SendNotification(eveningNotification, channel.Id);


	}

	public 


	#endregion
}