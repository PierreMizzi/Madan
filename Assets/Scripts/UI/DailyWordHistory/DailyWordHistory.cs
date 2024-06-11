using System;
using System.Collections.Generic;
using UnityEngine;

/*

    TODO : Locked / Unlocked on WordDataRect
    TODO : DailyWordHistory updates its WordDataRect

    Cliquer sur un DailyCheck

    if(1 dailyWord.isLocked == true)
    DailyWordHistory en mode "Pick a word"

    -> Click on WordData
    - WordData is unlocked
    - SaveManager saves
*/

public class DailyWordHistory : ApplicationScreen
{
    [SerializeField] private RectTransform m_container;
    [SerializeField] private WordDataRect m_wordDataRectPrefab;
    [SerializeField] private DailyWordManager m_manager;
    private List<WordDataRect> m_wordDataRects = new List<WordDataRect>();

    #region MonoBehaviour

    private void Awake()
    {
        m_wordDataRectPrefab.gameObject.SetActive(false);

        if (m_applicationChannel != null)
        {
            m_applicationChannel.onAppDataLoaded += CallbackAppDataLoaded;
            m_applicationChannel.onRefreshDailyCheck += CallbackRefreshDailyCheck;
        }
    }

    private void OnDestroy()
    {
        if (m_applicationChannel != null)
        {
            m_applicationChannel.onAppDataLoaded -= CallbackAppDataLoaded;
            m_applicationChannel.onRefreshDailyCheck -= CallbackRefreshDailyCheck;
        }
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
            wordDataRect.Initialize(this, dailyWord, index);
            m_wordDataRects.Add(wordDataRect);
            index++;
        }
    }

    public override void Display(params string[] options)
    {
        base.Display(options);
        if (options.Length != 0)
        {
            if (options[0] == DailyCheck.k_dailyCheckOption)
            {
                SetUnlockable();
                m_dailyCheckType = (DailyCheckType)int.Parse(options[1]);
            }
        }
    }

    public void OnClickBackButton()
    {
        m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
    }

    #region Lock / Unlocked

    /// <summary> 
    /// Type of the DailyCheck from which the DailyWordHistory entered its unlockable state
    /// When unlocking a WordDataRect, it checks the related DailyCheck on MainMenu
    /// </summary>
    private DailyCheckType m_dailyCheckType;

    private void SetUnlockable()
    {
        foreach (WordDataRect rect in m_wordDataRects)
        {
            if (rect.state == WordDataRectState.Locked)
                rect.SetUnlockable();
        }
    }

    public void UnsetUnlockable()
    {
        foreach (WordDataRect rect in m_wordDataRects)
        {
            if (rect.state == WordDataRectState.Unlockable)
                rect.SetLocked();
        }
        m_applicationChannel.onCheckDailyCheck.Invoke(m_dailyCheckType);
        m_dailyCheckType = DailyCheckType.None;

        // Saves : 
        //  - The newly unlocked status of WordData
        //  - The checked status on appropriate DailyCheck
        SaveManager.Save();
    }

    /// <summary> 
    /// Called when DailyCheck are reset, which means all unlocked WordDataRect are now locked
    /// </summary>
    private void CallbackRefreshDailyCheck()
    {
        foreach (WordDataRect rect in m_wordDataRects)
            rect.ManageLockedUnlocked();
    }


    #endregion

}