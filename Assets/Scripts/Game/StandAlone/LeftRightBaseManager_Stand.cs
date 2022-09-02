using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LeftRightBaseManager_Stand : BaseManager_Stand
{
    private Text txt_Ready;

    private float m_RandomWaitStakesTime = 0;
    private bool m_IsHasStakesTime = false;

    private int m_StakeNum = 0;
    protected bool m_IsCompareing = false;

    public void Awake()
    {
        Init();
    }

    public void FixedUpdate()
    {
        if (m_IsStartStakes)
        {
            if (IsWin())
            {
                m_IsStartStakes = false;
                return;
            }

            if (m_RandomWaitStakesTime <= 0)
            {
                //开始下注
                PutStakes();
                m_IsStartStakes = false;
                if (m_IsCompareing == false)
                {
                    go_CountDown.SetActive(false);
                    m_ZjhManager.SetNextPlayerStakes();
                }
                return;
            }

            m_Timer += Time.deltaTime;
            if (m_Timer >= 1)
            {
                m_RandomWaitStakesTime--;
                m_Timer = 0;
                m_Time--;
                txt_CountDown.text = m_Time.ToString();
            }
        }
    }

    public override void CompareWin()
    {
        
    }

    public override void CompareLose()
    {
        GiveUpCard();
    }

    private void Init()
    {
        m_StakeCountHint = transform.Find("StakeCountHint").GetComponent<StakeCountHint>();
        // m_ZjhManager = GetComponent<ZjhManager_Stand>();
        m_ZjhManager = gameObject.GetComponentInParent<ZjhManager_Stand>();
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        txt_StakeSum = transform.Find("StakeSum/txt_StakeSum").GetComponent<Text>();
        txt_Ready = transform.Find("txt_Ready").GetComponent<Text>();
        txt_GiveUp = transform.Find("txt_GiveUp").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        CardPoints = transform.Find("CardPoints");

        txt_GiveUp.gameObject.SetActive(false);
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        txt_StakeSum.text = "0";
        img_HeadIcon.sprite = ResourcesManager.GetSprite("headIcon_" + Random.Range(0, 19));
    }

    public void StartChooseBanker()
    {
        m_StakeSum += Models.GameModel.botStacks;
        txt_StakeSum.text = m_StakeSum.ToString();
        txt_Ready.gameObject.SetActive(false);
    }

    public override void StartStakes()
    {
        base.StartStakes();
        m_RandomWaitStakesTime = Random.Range(3, 6);

    }

    public abstract void Compare();

    private void PutStakes()
    {
        if (m_IsHasStakesTime)
        {
            m_StakeNum--;
            if (m_StakeNum <= 0) //下注次数用完
            {
                GetPutStakesNum();

                //比牌
                m_IsCompareing = true;
                Compare();
                StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "看看");
                return;
            }

            int stakes = m_ZjhManager.Stakes(Random.Range(3, 6));

            StakesAfter(stakes, "不看");
            print("1");
        }
        else if (m_CardType == CardType.Duizi)
        {
            int ran = Random.Range(0, 10);
            if (ran < 5) //跟注
            {
                StakesAfter(m_ZjhManager.Stakes(Random.Range(3, 6)), "不看");
                print("2");
            }
            else
            {
                //比牌
                m_IsCompareing = true;
                Compare();
                StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "看看");
            }
        }
        else if (m_CardType == CardType.Min)
        {
            int ran = Random.Range(0, 15);
            if (ran < 5) //跟注
            {
                StakesAfter(m_ZjhManager.Stakes(Random.Range(3, 6)), "不看");
                print("3");
            }
            else if (ran >= 5 && ran < 10)
            {
                //比牌
                m_IsCompareing = true;
                Compare();
                StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "看看");
            }
            else
            {
                //弃牌
                GiveUpCard();
            }
        }
        else //if (m_CardType == CardType.Baozi || m_CardType == CardType.Max)
        {
            StakesAfter(m_ZjhManager.Stakes(Random.Range(4, 6)), "不看");
            print("4");
        }
    }

    public abstract bool IsWin();

    /// <summary>
    /// 弃牌
    /// </summary>
    private void GiveUpCard()
    {
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        txt_GiveUp.gameObject.SetActive(true);
        m_ZjhManager.SetNextPlayerStakes();
        m_IsGiveUpCard = true;

        foreach (var item in go_SpawnCardList)
        {
            Destroy(item);
        }
    }

    private void GetPutStakesNum()
    {
        if ((int)m_CardType >= 2 && (int)m_CardType <= 6)
        {
            m_IsHasStakesTime = true;
            m_StakeNum = (int)m_CardType * 6;
        }
    }

}