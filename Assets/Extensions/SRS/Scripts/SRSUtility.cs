using System;
using UnityEngine;

public enum SRSAnswerRating
{
	None,
	Forgotten,
	Hard,
	Correct,
	Easy,
}

public enum SRSCardStatus
{
	None,
	New,
	Reviewed,
	Leeched,
}

[Serializable]
public class SRSAnswerRatingSettings
{
	[Tooltip("Name. Mostly used for Editor readability")]
	public string name;
	public SRSAnswerRating rating;

	/// <summary>
	/// X = Days | Y = Hours | Z = Minutes </summary>
	/// </summary>
	[Tooltip("X = Days | Y = Hours | Z = Minutes")]
	public Vector3 interval = new Vector3(0, 0, 0);

	public float easeModifier = 0.15f;

	public TimeSpan ComputeNextReviewTimespan(SRSCard card, bool useEase = false)
	{
		if (card == null)
		{
			throw new ArgumentNullException(nameof(card), "Card cannot be null.");
		}

		if (useEase == false)
		{
			return SRSUtility.ToTimeSpan(interval);
		}
		else
		{
			return SRSUtility.ToTimeSpan(interval) * card.ease;
		}
	}
}

public static class SRSUtility
{
	/// <summary>
	/// Converts a Vector3 to a TimeSpan.
	/// </summary>
	/// <param name="interval">The Vector3 to convert.</param>
	/// <returns>A TimeSpan representing the interval.</returns>
	public static TimeSpan ToTimeSpan(Vector3 interval)
	{
		return new TimeSpan((int)interval.x, (int)interval.y, (int)interval.z, 0);
	}
}

