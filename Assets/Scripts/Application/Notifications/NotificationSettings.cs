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

	[Tooltip("TimeSpan : X = Days | Y = Hours | Z = Minutes | W = Seconds")]
	public Vector4 delayedTimespan;

	[Tooltip("TimeSpan : X = Days | Y = Hours | Z = Minutes | W = Seconds")]
	public Vector4 repeatIntervals;

	public AndroidNotification Create()
	{
		DateTime fireTime = DateTime.Now;

		if (delayedTimespan != Vector4.zero)
		{
			fireTime += Convert(delayedTimespan);
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

		if (repeatIntervals != Vector4.zero)
		{
			notification.RepeatInterval = Convert(repeatIntervals);
		}

		return notification;
	}
	
	private TimeSpan Convert(Vector4 timeSpan)
	{
		return new TimeSpan((int)timeSpan.x, (int)timeSpan.y, (int)timeSpan.z, (int)timeSpan.w);
	}

}