using System;
using System.Collections.Generic;
using UnityEngine;

/*

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

    protected override void Awake()
    {
        base.Awake();
        m_wordDataRectPrefab.gameObject.SetActive(false);

        if (m_applicationChannel != null)
            m_applicationChannel.onRefreshDailyCheck += CallbackRefreshDailyCheck;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (m_applicationChannel != null)
            m_applicationChannel.onRefreshDailyCheck -= CallbackRefreshDailyCheck;
    }

    #endregion

    protected override void CallbackAppDataLoaded()
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
                SetState(DailyWordHistoryState.Unlocking);
                m_dailyCheckType = (DailyCheckType)int.Parse(options[1]);
            }
        }
    }

    public void OnClickBackButton()
    {
        if (m_state == DailyWordHistoryState.Unlocking)
            SetState(DailyWordHistoryState.Normal);

        m_applicationChannel.onDisplayScreen.Invoke(ApplicationScreenType.MainMenu);
    }

    #region State

    private DailyWordHistoryState m_state;

    private void SetState(DailyWordHistoryState state)
    {
        m_state = state;

        switch (state)
        {
            case DailyWordHistoryState.Normal:
                SetStateNormal();
                break;
            case DailyWordHistoryState.Unlocking:
                SetStateUnlocking();
                break;
        }
    }

    private void SetStateUnlocking()
    {
        foreach (WordDataRect rect in m_wordDataRects)
        {
            if (rect.state == WordDataRectState.Locked)
                rect.SetUnlockable();
        }
    }

    private void SetStateNormal()
    {
        foreach (WordDataRect rect in m_wordDataRects)
        {
            if (rect.state == WordDataRectState.Unlockable)
                rect.SetLocked();
        }
    }

    #endregion

    #region Lock / Unlocked

    /// <summary> 
    /// Type of the DailyCheck from which the DailyWordHistory entered its unlockable state
    /// When unlocking a WordDataRect, it checks the related DailyCheck on MainMenu
    /// </summary>
    private DailyCheckType m_dailyCheckType;

    public void ManageAfterUnlocking()
    {
        SetState(DailyWordHistoryState.Normal);

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