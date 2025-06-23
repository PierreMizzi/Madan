using UnityEngine;

[CreateAssetMenu(fileName = "SRSSettings", menuName = "SRSSettings", order = 0)]
public class SRSSettings : ScriptableObject
{
	public float strength;

	public AnimationCurve repetitionMultiplier;
}