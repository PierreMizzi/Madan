using System;
using UnityEngine;

public class DailyWordHistory : ApplicationScreen
{
    [SerializeField] private RectTransform m_container;
    [SerializeField] private WordDataRect m_wordDataRectPrefab;
    [SerializeField] private DailyWordManager m_manager;

    #region MonoBehaviour

    private void Awake()
    {
        m_wordDataRectPrefab.gameObject.SetActive(false);

        if (m_applicationChannel != null)
            m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
    }

    private void OnDestroy()
    {
        if (m_applicationChannel != null)
            m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;
    }

    #endregion

    private void CallbackAppDataLoaded()
    {
        PopulateHistory();
    }

    private void PopulateHistory()
    {
        int index = 1;

        foreach (WordData dailyWord in m_manager.dailyWords)
        {
            WordDataRect wordDataRect = Instantiate(m_wordDataRectPrefab, m_container);
            wordDataRect.gameObject.SetActive(true);
            wordDataRect.SetWordData(dailyWord, index);
            index++;
        }
    }

    public override void Display(params string[] options)
    {
        base.Display(options);
        if (options.Length != 0)
        {
            if (options[0] == "DailyCheck")
                Debug.Log("");
        }
    }

    public void OnClickBackButton()
    {
        m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
    }

}