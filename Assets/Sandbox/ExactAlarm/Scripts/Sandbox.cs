using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;
using System.Collections;
using System;

public class Sandbox : MonoBehaviour
{
	private AndroidNotificationChannelGroup m_group;
	private AndroidNotificationChannel m_channel;

	private const string k_mainChannelID = "420";

	public IEnumerator Start()
	{
		AndroidNotificationCenter.Initialize();

		Debug.Log("Start");
		yield return RequestNotificationPermission();
		// yield return ManagePermissions();

		LogStatus();

		// Creates a group & register it
		m_group = new AndroidNotificationChannelGroup()
		{
			Id = "Main",
			Name = "Main notifications",
		};
		AndroidNotificationCenter.RegisterNotificationChannelGroup(m_group);

		// Creates a channel & register it
		m_channel = new AndroidNotificationChannel()
		{
			Id = k_mainChannelID,
			Name = "Default Channel",
			Importance = Importance.Default,
			Description = "Generic notifications",
			Group = "Main",  // must be same as Id of previously registered group
		};
		AndroidNotificationCenter.RegisterNotificationChannel(m_channel);

		yield return null;
	}


	#region Permissions

	private IEnumerator ManagePermissions()
	{
		if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
		{
			Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
		}

		yield return null;
	}

	IEnumerator RequestNotificationPermission()
	{
		PermissionRequest request = new PermissionRequest();
		while (request.Status == PermissionStatus.RequestPending)
			yield return null;
		// here use request.Status to determine user's response
		Debug.Log($"PermissionRequest status : {request.Status}");
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

		var status = AndroidNotificationCenter.CheckScheduledNotificationStatus(m_oneMinuteNotifID);
		log += $"OneMinuteNotif : {status}\n";
		status = AndroidNotificationCenter.CheckScheduledNotificationStatus(m_repeatedNotifID);
		log += $"RepeatedNotif : {status}\n";

		log += $"////////////////";
		Debug.Log(log);
	}

	#endregion

	#region Functions

	[SerializeField] private NotificationSettings m_oneMinuteSettings;
	private int m_oneMinuteNotifID;

	[SerializeField] private NotificationSettings m_repeatSettings;
	private int m_repeatedNotifID;

	public void SendNotificationInOneMinute()
	{
		Debug.Log($"OneMinuteNotif : {DateTime.Now}");
		AndroidNotification notification = m_oneMinuteSettings.Create();

		m_oneMinuteNotifID = AndroidNotificationCenter.SendNotification(notification, k_mainChannelID);
	}

	public void SendNotificationRepeated()
	{
		Debug.Log($"RepeatedNotif : {DateTime.Now}");
		AndroidNotification notification = m_repeatSettings.Create();

		m_repeatedNotifID = AndroidNotificationCenter.SendNotification(notification, k_mainChannelID);
	}

	#endregion


}
