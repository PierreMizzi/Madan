using System;
using TMPro;
using UnityEngine;

public class WordDataRect : MonoBehaviour
{

    private DailyWordHistory m_manager;
    private WordData m_wordData;

    private WordDataRectState m_state;
    public WordDataRectState state => m_state;

    [SerializeField] private TextMeshProUGUI k_indexLabel;
    [SerializeField] private TextMeshProUGUI m_kanjiLabel;
    [SerializeField] private TextMeshProUGUI m_furiganaLabel;
    [SerializeField] private TextMeshProUGUI m_traductionLabel;

    [SerializeField] private GameObject m_lockedImage;
    [SerializeField] private GameObject m_unlockableImage;


    public void Initialize(DailyWordHistory manager, WordData wordData, int index)
    {
        m_manager = manager;
        m_wordData = wordData;

        k_indexLabel.text = index.ToString();

        m_kanjiLabel.text = m_wordData.kanji;
        m_furiganaLabel.text = m_wordData.furigana;
        m_traductionLabel.text = m_wordData.traduction;

        ManageLockedUnlocked();
    }

    public void OnClick()
    {
        if (m_state == WordDataRectState.Unlockable)
        {
            SetUnlocked();
            m_manager.UnsetUnlockable();
        }
    }

    [ContextMenu("SetLocked")]
    public void SetLocked()
    {
        m_state = WordDataRectState.Locked;

        m_lockedImage.SetActive(true);
        m_unlockableImage.SetActive(false);

        m_wordData.dateUnlocked = new DateTime();
    }

    [ContextMenu("SetUnlocked")]
    public void SetUnlocked()
    {
        m_state = WordDataRectState.Unlocked;

        m_lockedImage.SetActive(false);
        m_unlockableImage.SetActive(false);

        m_wordData.dateUnlocked = DateTime.Now;
    }

    [ContextMenu("SetUnlockable")]
    public void SetUnlockable()
    {
        m_state = WordDataRectState.Unlockable;

        m_unlockableImage.SetActive(true);
    }

    public void ManageLockedUnlocked()
    {
        if (m_wordData.isUnlocked)
            SetUnlocked();
        else
            SetLocked();
    }


}
