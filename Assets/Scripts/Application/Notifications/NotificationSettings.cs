using System;
using System.Security.Cryptography;
using JetBrains.Annotations;
using Unity.Notifications.Android;
using UnityEngine;

[CreateAssetMenu(fileName = "NotificationSettings", menuName = "Madan/NotificationSettings", order = 0)]
public class NotificationSettings : ScriptableObject
{
	public string title;
	[TextArea(2, 4)]
	public string text;
	public string smallIcon;
	public string largeIcon;
	public NotificationStyle style;

	[Tooltip("HH:MM:SS AM/PM")]
	public string fixedFireTime = "12:15:12 PM";

	[Tooltip("TimeSpan : X = Days | Y = Hours | Z = Minutes | W = Seconds")]
	public Vector4 delayedTimespan;

	[Tooltip("TimeSpan : X = Days | Y = Hours | Z = Minutes | W = Seconds")]
	public Vector4 repeatTimespan;

	public AndroidNotification Create()
	{
		DateTime fireTime = DateTime.Now;

		if (delayedTimespan != Vector4.zero)
		{
			fireTime += Convert(delayedTimespan);
		}
		else if (String.IsNullOrEmpty(fixedFireTime) == false)
		{
			fireTime = DateTime.Parse(DateTime.Now.Date.ToString("d") + " " + fixedFireTime);
			Debug.Log($"fireTime : {fireTime}");
		}

		AndroidNotification notification = new AndroidNotification()
		{
			Title = title,
			Text = text,
			SmallIcon = smallIcon,
			LargeIcon = largeIcon,
			Style = style,
			FireTime = fireTime,
		};

		if (repeatTimespan != Vector4.zero)
		{
			notification.RepeatInterval = Convert(repeatTimespan);
		}

		return notification;
	}
	
	private TimeSpan Convert(Vector4 timeSpan)
	{
		return new TimeSpan((int)timeSpan.x, (int)timeSpan.y, (int)timeSpan.z, (int)timeSpan.w);
	}

}