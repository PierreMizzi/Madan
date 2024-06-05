using System;
using UnityEngine;

public class Application : MonoBehaviour
{
	private void Start()
	{
		Load();
	}

	[ContextMenu("Load")]
	private void Load()
	{
		Parser.Load();
	}
}