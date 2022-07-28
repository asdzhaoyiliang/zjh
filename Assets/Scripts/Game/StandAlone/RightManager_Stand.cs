using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RightManager_Stand : MonoBehaviour
{
    private Image img_HeadIcon;
    private Image img_Banker;
    private Text txt_StakeSum;
    private Text txt_Ready;
    private GameObject go_CountDown;
    private Text txt_CountDown;
    private Transform CardPoints;

    private int m_StakeSum = 0;

    private List<Card> m_CardList = new List<Card>();
    public GameObject go_CardPre;
    private int m_CardPointX = -40;
    private CardType m_CardType;

    public bool m_IsGiveUpCard = false;

    private bool m_IsStartStakes = false;
    /// <summary>
    /// 倒计时
    /// </summary>
    private float m_Time = 60f;
    /// <summary>
    /// 计时器
    /// </summary>
    private float m_Timer = 0.0f;
    public void Awake()
    {
        Init();
    }
    public void FixedUpdate()
    {
        if (m_IsStartStakes)
        {
            if (m_Time <= 0)
            {
                //倒计时结束
                //默认当做跟注处理
                m_IsStartStakes = false;
                m_Time = 60;
            }
            m_Timer += Time.deltaTime;
            if (m_Timer >= 1)
            {
                m_Timer = 0;
                m_Time--;
                txt_CountDown.text = m_Time.ToString();
            }
        }
    }
    private void Init()
    {
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        txt_StakeSum = transform.Find("StakeSum/txt_StakeSum").GetComponent<Text>();
        txt_Ready = transform.Find("txt_Ready").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        CardPoints = transform.Find("CardPoints");

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

    public void BecomeBanker()
    {
        img_Banker.gameObject.SetActive(true);
    }

    public void DealCard(Card card, float duration, Vector3 initPos)
    {
        m_CardList.Add(card);
        GameObject go = Instantiate(go_CardPre, CardPoints);
        go.GetComponent<RectTransform>().localPosition = initPos;
        go.GetComponent<RectTransform>().DOLocalMove(new Vector3(m_CardPointX, 0, 0), duration);
        m_CardPointX += 40;
    }

    public void DealCardFinished()
    {
        SortCards();
        GetCardType();
        print("left牌型：" + m_CardType);
    }

    public void StartStakes()
    {
        m_IsStartStakes = true;
        go_CountDown.SetActive(true);
        txt_CountDown.text = "60";
        m_Time = 60;
    }
    
    public void SortCards()
    {
        for (int i = 0; i < m_CardList.Count; i++)
        {
            for (int j = i; j < m_CardList.Count; j++)
            {
                if (m_CardList[j].Weight < m_CardList[i].Weight)
                {
                    Card temp = m_CardList[j];
                    m_CardList[j] = m_CardList[i];
                    m_CardList[i] = temp;
                }
            }
        }
    }

    private void GetCardType()
    {
        if (m_CardList[0].Weight == 5 && m_CardList[1].Weight == 3 && m_CardList[2].Weight == 2)
        {
            m_CardType = CardType.Max;
        }
        else if (m_CardList[0].Weight == m_CardList[1].Weight && m_CardList[1].Weight == m_CardList[2].Weight)
        {
            m_CardType = CardType.Baozi;
        }
        else if (m_CardList[0].Color == m_CardList[1].Color && m_CardList[1].Color == m_CardList[2].Color &&
                 m_CardList[0].Weight == m_CardList[1].Weight + 1 && m_CardList[1].Weight == m_CardList[2].Weight + 1)
        {
            m_CardType = CardType.Shunjin;
        }
        else if (m_CardList[0].Color == m_CardList[1].Color && m_CardList[1].Color == m_CardList[2].Color)
        {
            m_CardType = CardType.Jinhua;
        }
        else if (m_CardList[0].Weight == m_CardList[1].Weight + 1 && m_CardList[1].Weight == m_CardList[2].Weight + 1)
        {
            m_CardType = CardType.Shunzi;
        }
        else if (m_CardList[0].Weight == m_CardList[1].Weight || m_CardList[1].Weight == m_CardList[2].Weight)
        {
            m_CardType = CardType.Duizi;
        }
        else
        {
            m_CardType = CardType.Min;
        }
    }
}