using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RightManager_Stand : LeftRightBaseManager_Stand
{
    public override void Compare()
    {
        m_ZjhManager.RightPlayerCompare();
    }

    public override void CompareWin()
    {
        base.CompareWin();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZjhManager.m_CurrentStakesIndex = 2;
        m_ZjhManager.SetNextPlayerStakes();
        m_IsCompareing = false;
    }

    public override bool IsWin()
    {
        if (m_ZjhManager.IsRightWin())
        {
            m_ZjhManager.RightWin();
            return true;
        }

        return false;
    }
}