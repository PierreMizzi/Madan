using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Application))]
public class ApplicationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		GUIStyle style = new GUIStyle();
		style.fontSize = 25;
		style.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Date is MM/DD/YYYY", style);
		base.OnInspectorGUI();
	}
}