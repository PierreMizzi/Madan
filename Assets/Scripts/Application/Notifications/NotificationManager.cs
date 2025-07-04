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
		int needsSave = 0;

		// Morning
		needsSave += ManageNotification(m_morningDailyCheckSettings, ref SavedData.morningNotificationID);

		// Afternoon
		needsSave += ManageNotification(m_afternoonDailyCheckSettings, ref SavedData.afternoonNotificationID);

		// Evening
		needsSave += ManageNotification(m_eveningDailyCheckSettings, ref SavedData.eveningNotificationID);

		// Trial
		needsSave += ManageNotification(m_trialSettings, ref SavedData.trialNotificationID);

		if (needsSave > 0)
		{
			SaveManager.Save();
		}

		LogStatus();
	}

	private int ManageNotification(NotificationSettings settings, ref int notificationIDSaveData)
	{
		if (CheckNeedsScheduling(ref notificationIDSaveData))
		{
			Debug.Log($"NOTIF | Creating a notification titled \"{settings.title} \"");
			AndroidNotification notification = settings.Create();
			notificationIDSaveData = AndroidNotificationCenter.SendNotification(notification, channel.Id);
			return 1;
		}
		else
		{
			Debug.Log($"NOTIF | Notification \"{settings.title}\" already exists with ID {notificationIDSaveData}.");
			return 0;
		}
	}

	private bool CheckNeedsScheduling(ref int notificationIDSaveData)
	{
		// notificationIDSaveData is invalid, we need to schedule it
		if (notificationIDSaveData == 0 || notificationIDSaveData == -1)
		{
			return true;
		}

		var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(notificationIDSaveData);

		switch (status)
		{
			default:
			case NotificationStatus.Unavailable:
			case NotificationStatus.Unknown:
				return true;

			case NotificationStatus.Scheduled:
			case NotificationStatus.Delivered:
				return false; 
		}
	}

	#endregion

	#region Debug

	public void LogStatus()
	{
		string log = $"//////////////// \n";
		log += $"Log Status : \n";

		log += $"-------------\n";
		log += $"Notifications Channel : \n";
		AndroidNotificationChannel[] channels = AndroidNotificationCenter.GetNotificationChannels();

		foreach (AndroidNotificationChannel channel in channels)
		{
			log += $"Channel {channel.Name} : Group {channel.Group}\n";
		}
		log += $"-------------\n";
		log += $"Daily Check Statuses : \n";

		var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(SavedData.morningNotificationID);
		log += $"Morning notif : {status}\n";

		status = AndroidNotificationCenter.CheckScheduledNotificationStatus(SavedData.eveningNotificationID);
		log += $"Evening notif : {status}\n";

		status = AndroidNotificationCenter.CheckScheduledNotificationStatus(SavedData.afternoonNotificationID);
		log += $"Afternoon notif : {status}\n";

		status = AndroidNotificationCenter.CheckScheduledNotificationStatus(SavedData.trialNotificationID);
		log += $"Trial notif : {status}\n";

		log += $"////////////////";
		Debug.Log(log);
	}

	#endregion

}