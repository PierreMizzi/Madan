using UnityEngine;
using UnityEditor;

namespace PierreMizzi.Extensions.SRS.EditorScripts
{



	[CustomEditor(typeof(SRSDebugger))]
	public class SRSDebuggerEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			SRSDebugger m_target = target as SRSDebugger;

			if (GUILayout.Button("Test SRS Algorythm"))
			{
				m_target.TestSRSAlgorythm();
			}

			if (GUILayout.Button("Initialize Test Deck"))
			{
				m_target.InitializeDebugDeck();
			}

			GUILayout.Space(EditorGUIUtility.singleLineHeight);
			GUILayout.Label("Answer Rating");

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Forgot"))
			{
			}

			if (GUILayout.Button("Hard"))
			{
			}

			if (GUILayout.Button("Correct"))
			{
			}

			if (GUILayout.Button("Easy"))
			{
			}

			GUILayout.EndHorizontal();

		}
	}
}