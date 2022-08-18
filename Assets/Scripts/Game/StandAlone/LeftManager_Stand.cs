using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LeftManager_Stand : LeftRightBaseManager_Stand
{
    public override void Compare()
    {
        m_ZjhManager.LeftPlayerCompare();
    }

    public override void Win()
    {
        base.Win();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZjhManager.m_CurrentStakesIndex = 1;
        m_ZjhManager.SetNextPlayerStakes();
    }
}