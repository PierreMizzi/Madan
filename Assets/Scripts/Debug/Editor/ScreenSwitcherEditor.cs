using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenSwitcher))]
public class ScreenSwitcherEditor : Editor
{

	private ScreenSwitcher m_target;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		m_target = target as ScreenSwitcher;

		foreach (Canvas canvas in m_target.canvases)
		{
			if (GUILayout.Button($"Display {canvas.gameObject.name}"))
				m_target.DisplayScreen(canvas);
		}

	}
}