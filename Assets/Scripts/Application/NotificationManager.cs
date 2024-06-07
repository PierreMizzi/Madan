using Unity.Notifications.Android;
using UnityEngine;

/*
https://docs.unity3d.com/Packages/com.unity.mobile.notifications@2.3/manual/Android.html

*/

public class NotificationManager : MonoBehaviour
{
	#region Daily Notification

	private AndroidNotificationChannelGroup group;
	private AndroidNotificationChannel channel;

	private void Start()
	{
		Initialize();
	}

	public void Initialize()
	{
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
}