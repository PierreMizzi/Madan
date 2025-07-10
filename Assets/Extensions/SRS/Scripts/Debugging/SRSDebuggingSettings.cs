using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SRSDebuggingSettings", menuName = "SRS/SRSDebuggingSettings", order = 0)]
public class SRSDebuggingSettings : ScriptableObject
{

	[TextArea(3, 10)]
	public string description;

	public List<SRSAnswerRating> successiveRatings = new List<SRSAnswerRating> { };

}