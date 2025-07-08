
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Application))]
public class ApplicationEditor : Editor
{
	Application m_application;
	public override void OnInspectorGUI()
	{
		m_application = (Application)target;

		GUIStyle style = new GUIStyle();
		style.fontStyle = FontStyle.Bold;
		EditorGUILayout.LabelField("Date is MM/DD/YYYY", style);
		base.OnInspectorGUI();



		if (GUILayout.Button("Log Application Data"))
			m_application.LogApplicationData();

		if (GUILayout.Button("Log Written Data"))
			m_application.LogWrittenData();

		if (GUILayout.Button("Log Word of the day"))
			m_application.LogWordOfTheDay();

		if (GUILayout.Button("Log Database"))
			m_application.LogDataBase();

		if (GUILayout.Button("Log DayDatas"))
			m_application.LogDayDatas();


		EditorGUILayout.LabelField("Clear application", style);
		if (GUILayout.Button("Clear Daily Words"))
			m_application.ClearDailyWords();

		if (GUILayout.Button("Reset Daily Check"))
			m_application.ResetDailyCheck();

		if (GUILayout.Button("Clear Today DayData"))
			m_application.ClearDayData();


	}

}