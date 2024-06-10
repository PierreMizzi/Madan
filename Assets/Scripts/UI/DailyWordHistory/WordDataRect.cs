using TMPro;
using UnityEngine;

public class WordDataRect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI k_indexLabel;
    [SerializeField] private TextMeshProUGUI m_kanjiLabel;
    [SerializeField] private TextMeshProUGUI m_furiganaLabel;
    [SerializeField] private TextMeshProUGUI m_traductionLabel;

    private WordData m_wordData;

    public void SetWordData(WordData wordData, int index)
    {
        m_wordData = wordData;

        k_indexLabel.text = index.ToString();

        m_kanjiLabel.text = m_wordData.kanji;
        m_furiganaLabel.text = m_wordData.furigana;
        m_traductionLabel.text = m_wordData.traduction;
    }

}
