using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ZjhManager_Stand : MonoBehaviour
{
    private Text txt_BottomStakes;
    private Text txt_TopStakes;
    private Button btn_Back;
    private SelfManager_Stand m_SelfManager;
    private LeftManager_Stand m_LeftManager;
    private RightManager_Stand m_RightManager;

    /// <summary>
    /// 左边玩家是否弃牌
    /// </summary>
    public bool LeftIsGiveUp
    {
        get { return m_LeftManager.m_IsGiveUpCard; }
    }
    /// <summary>
    /// 右边玩家是否弃牌
    /// </summary>
    public bool RightIsGiveUp
    {
        get { return m_RightManager.m_IsGiveUpCard; }
    }
    /// <summary>
    /// 当前发牌的游标
    /// </summary>
    private int m_CurrentDealCardIndex = 0;
    /// <summary>
    /// 当前下注的游标
    /// </summary>
    public int m_CurrentStakesIndex = 0;
    

    /// <summary>
    /// 牌库
    /// </summary>
    private List<Card> m_CardList = new List<Card>();
    /// <summary>
    /// 发牌的下标
    /// </summary>
    private int m_DealCardIndex = 0;

    private float m_DealCardDurationTime = 0.1f;

    /// <summary>
    /// 是否开始下注
    /// </summary>
    private bool m_IsStartStakes = false;
    
    private bool m_IsNextPlayerCanStake = true;

    /// <summary>
    /// 上一位玩家下注的数量
    /// </summary>
    private int m_LastPlayerStakesCount = 0;

    public void SetNextPlayerStakes()
    {
        m_IsNextPlayerCanStake = true;
    }

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_SelfManager = GetComponentInChildren<SelfManager_Stand>();
        m_LeftManager = GetComponentInChildren<LeftManager_Stand>();
        m_RightManager = GetComponentInChildren<RightManager_Stand>();
        txt_BottomStakes = transform.Find("Main/txt_BottomStakes").GetComponent<Text>();
        txt_TopStakes = transform.Find("Main/txt_TopStakes").GetComponent<Text>();
        btn_Back = transform.Find("Main/btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(() => { SceneManager.LoadScene("2.Main"); });

        txt_BottomStakes.text = Models.GameModel.botStacks.ToString();
        txt_TopStakes.text = Models.GameModel.topStacks.ToString();
        m_LastPlayerStakesCount = Models.GameModel.botStacks;
    }

    public void FixedUpdate()
    {
        if (m_IsStartStakes)
        {
            if (m_IsNextPlayerCanStake)
            {
                if (m_CurrentStakesIndex % 3 == 0)
                {
                    if (m_SelfManager.m_IsGiveUpCard == false)
                    {
                        m_SelfManager.StartStakes();
                        m_IsNextPlayerCanStake = false;
                    }
                }

                if (m_CurrentStakesIndex % 3 == 1)
                {
                    if (m_LeftManager.m_IsGiveUpCard == false)
                    {
                        m_LeftManager.StartStakes();
                        m_IsNextPlayerCanStake = false;
                    }
                }

                if (m_CurrentStakesIndex % 3 == 2)
                {
                    if (m_RightManager.m_IsGiveUpCard == false)
                    {
                        m_RightManager.StartStakes();
                        m_IsNextPlayerCanStake = false;
                    }
                }

                m_CurrentStakesIndex++;
            }
        }
    }

    /// <summary>
    /// 和右边玩家比牌
    /// </summary>
    public void RightPlayerCompare()
    {
        if (m_SelfManager.m_IsGiveUpCard)
        {
            //和左边玩家比牌
            EventCenter.Broadcast(EventDefine.VSAI, (BaseManager_Stand)m_RightManager,
                (BaseManager_Stand)m_LeftManager);
        }
        else
        {
            //和self玩家比牌
            EventCenter.Broadcast(EventDefine.VSWithSelf, (BaseManager_Stand)m_RightManager,
                (BaseManager_Stand)m_SelfManager, "右边玩家", "我");
        }
    }

    /// <summary>
    /// 和左边玩家比牌
    /// </summary>
    public void LeftPlayerCompare()
    {
        if (m_SelfManager.m_IsGiveUpCard)
        {
            //和右边玩家比牌
            EventCenter.Broadcast(EventDefine.VSAI, (BaseManager_Stand)m_LeftManager,
                (BaseManager_Stand)m_RightManager);
        }
        else
        {
            //和self玩家比牌
            EventCenter.Broadcast(EventDefine.VSWithSelf, (BaseManager_Stand)m_LeftManager,
                (BaseManager_Stand)m_SelfManager, "左边玩家", "我");
        }
    }

    public int Stakes(int count)
    {
        m_LastPlayerStakesCount += count;
        if (m_LastPlayerStakesCount > Models.GameModel.topStacks)
        {
            m_LastPlayerStakesCount = Models.GameModel.topStacks;
        }

        return m_LastPlayerStakesCount;
    }

    public void ChooseBanker()
    {
        m_LeftManager.StartChooseBanker();
        m_RightManager.StartChooseBanker();

        int ran = Random.Range(0, 3);
        switch (ran)
        {
            case 0:
                m_SelfManager.BecomeBanker();
                m_CurrentDealCardIndex = 0;
                m_CurrentStakesIndex = 1;
                break;
            case 1:
                m_LeftManager.BecomeBanker();
                m_CurrentDealCardIndex = 1;
                m_CurrentStakesIndex = 2;
                break;
            case 2:
                m_RightManager.BecomeBanker();
                m_CurrentDealCardIndex = 2;
                m_CurrentStakesIndex = 0;
                break;
            default:
                break;
        }
 
        //发牌
        EventCenter.Broadcast(EventDefine.Hint, "开始发牌");
        StartCoroutine(DealCard());
    }

    private IEnumerator DealCard()
    {
        //初始化牌
        if (m_CardList.Count == 0 || m_CardList == null || m_CardList.Count < 9)
        {
            InitCard();
            // 洗牌
            ClearCard();
        }

        // 发牌
        for (int i = 0; i < 9; i++)
        {
            if (m_CurrentDealCardIndex % 3 == 0)
            {
                //自身发牌
                m_SelfManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(0, 215, 0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            else if (m_CurrentDealCardIndex % 3 == 1)
            {
                //左边发牌
                m_LeftManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(556, 0, 0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }
            else
            {
                //右边发牌
                m_RightManager.DealCard(m_CardList[m_DealCardIndex], m_DealCardDurationTime, new Vector3(-523, 0, 0));
                m_CardList.RemoveAt(m_DealCardIndex);
            }

            m_DealCardIndex++;
            m_CurrentDealCardIndex++;
            yield return new WaitForSeconds(m_DealCardDurationTime);
        }

        //发牌结束
        m_SelfManager.DealCardFinished();
        m_LeftManager.DealCardFinished();
        m_RightManager.DealCardFinished();
        m_IsStartStakes = true;
    }

    private void InitCard()
    {
        for (int weight = 2; weight <= 14; weight++)
        {
            for (int color = 0; color <= 3; color++)
            {
                Card card = new Card(weight, color);
                m_CardList.Add(card);
            }
        }
    }

    private void ClearCard()
    {
        for (int i = 0; i < m_CardList.Count; i++)
        {
            int ran = Random.Range(0, m_CardList.Count);
            Card tmp = m_CardList[i];
            m_CardList[i] = m_CardList[ran];
            m_CardList[ran] = tmp;
        }
    }
}
