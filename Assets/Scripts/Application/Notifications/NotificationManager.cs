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
		Initialize();
	}

	#endregion

	#region Notification

	private AndroidNotificationChannelGroup group;
	private AndroidNotificationChannel channel;

	private const string k_mainChannelID = "420";

	private string nowDate
	{
		get
		{
			DateTime now = DateTime.Now;
			return $"{now.Month}/{now.Day}/{now.Year}";
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
			Id = "420",
			Name = "Default Channel",
			Importance = Importance.Default,
			Description = "Generic notifications",
			Group = "Main",  // must be same as Id of previously registered group
		};
		AndroidNotificationCenter.RegisterNotificationChannel(channel);

		// // Morning

		// DateTime morningNotificationTime = DateTime.Parse(nowDate + "09:30:00");

		// var morningNotification = new AndroidNotification
		// {
		// 	Title = "Morning notification !",
		// 	Text = "Tu as un nouveau mot du jour !",
		// 	FireTime = morningNotificationTime
		// };

		// AndroidNotificationCenter.SendNotification(morningNotification, channel.Id);

		// // Noon
		// DateTime noonNotificationTime = DateTime.Parse(nowDate + "13:00:00");

		// var noonNotification = new AndroidNotification
		// {
		// 	Title = "Noon notification !",
		// 	Text = "Tu as un nouveau mot du jour !",
		// 	FireTime = morningNotificationTime
		// };

		// AndroidNotificationCenter.SendNotification(morningNotification, channel.Id);

	}

	[ContextMenu("SendSimpleNotification")]
	public void SendSimpleNotification()
	{
		Debug.Log("SendSimpleNotification");
		var notification = new AndroidNotification
		{
			Title = "Notification sympa !",
			Text = "Aller il faut réviser",
			FireTime = System.DateTime.Now.AddMinutes(1)
		};

		AndroidNotificationCenter.SendNotification(notification, channel.Id);
	}

	#endregion

	#region Daily Check

	private void InitializeDailyCheck()
	{
		
	}
		
	#endregion
}