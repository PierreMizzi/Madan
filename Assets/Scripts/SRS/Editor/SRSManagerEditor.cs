using UnityEngine;
using UnityEditor;
using PierreMizzi.Extensions.SRS;

[CustomEditor(typeof(SRSManager))]
public class SRSManagerEditor : Editor
{
	SRSManager m_target = null;

	SerializedProperty settings;

	public override void OnInspectorGUI()
	{
		// base.OnInspectorGUI();
		settings = serializedObject.FindProperty("m_SRSsettings");
		EditorGUILayout.PropertyField(settings);
		settings.serializedObject.ApplyModifiedProperties();

		settings = serializedObject.FindProperty("m_applicationChannel");
		EditorGUILayout.PropertyField(settings);
		settings.serializedObject.ApplyModifiedProperties();


		m_target = target as SRSManager;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("debugDeck"));

		GUILayout.Space(EditorGUIUtility.singleLineHeight);
		if (m_target.debugDeck == null || m_target.debugDeck.cards.Count == 0)
		{
			if (GUILayout.Button("Initialize Test Deck"))
			{
				m_target.InitializeDebugDeck();
			}
		}
		else
		{
			if (GUILayout.Button("Reset Test Deck"))
			{
				m_target.InitializeDebugDeck();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
			InspectorGUIForStudySession();
		}
	}

	private void InspectorGUIForStudySession()
	{
		if (m_target.currentStudySession == null || m_target.currentStudySession.reviewCards == null)
		{
			if (GUILayout.Button("Start Study Session"))
			{
				m_target.StartStudySession();
			}
		}
		else
		{
			GUILayout.Label("Study Session");

			EditorGUILayout.PropertyField(serializedObject.FindProperty("currentStudySession"));


			if (m_target.isFrontOrBack == SRSCardFace.Front)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("currentCardFront"));

				if (GUILayout.Button("Flip"))
				{
					m_target.SetCardBack();
				}
			}
			else if (m_target.isFrontOrBack == SRSCardFace.Back)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("currentCardFront"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("currentCardBack"));

				GUILayout.BeginHorizontal();

				if (GUILayout.Button("Forgot"))
				{
					m_target.ManageCardAfterFeedback(SRSAnswerRating.Forgotten);
				}

				if (GUILayout.Button("Hard"))
				{
					m_target.ManageCardAfterFeedback(SRSAnswerRating.Hard);
				}

				if (GUILayout.Button("Correct"))
				{
					m_target.ManageCardAfterFeedback(SRSAnswerRating.Correct);
				}

				if (GUILayout.Button("Easy"))
				{
					m_target.ManageCardAfterFeedback(SRSAnswerRating.Easy);
				}

				GUILayout.EndHorizontal();
			}

			if (GUILayout.Button("Stop Study Session"))
			{
				m_target.StopStudySession();
			}
		}
	}
}