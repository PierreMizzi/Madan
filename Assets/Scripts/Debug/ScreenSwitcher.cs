using System;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSwitcher : MonoBehaviour
{
	[SerializeField] private List<Canvas> m_canvases;

	public List<Canvas> canvases => m_canvases;

	public void DisplayScreen(Canvas chosenCanvas)
	{
		foreach (Canvas canvas in m_canvases)
			canvas.enabled = chosenCanvas == canvas;
	}
}