using System;
using TMPro;
using UnityEngine;

public class DailyWordUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_furiganaLabel;
	[SerializeField] private TextMeshProUGUI m_kanjiLabel;
	[SerializeField] private TextMeshProUGUI m_levelLabel;
	[SerializeField] private TextMeshProUGUI m_traductionLabel;

	public void OnClick()
	{
		Debug.Log("Click DailyWorldUI !");
	}

	public void Refresh(WordData data)
	{
		m_furiganaLabel.text = data.furigana;
		m_kanjiLabel.text = data.kanji;
		m_levelLabel.text = data.level;
		m_traductionLabel.text = data.traduction;
	}
}