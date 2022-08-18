using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseManager_Stand : MonoBehaviour
{
    public List<Card> m_CardList = new List<Card>();
    public CardType m_CardType;
    protected int m_StakeSum = 0;

    public bool m_IsGiveUpCard = false;
    protected bool m_IsStartStakes = false;
    protected GameObject go_CountDown;
    protected Image img_Banker;
    protected Image img_HeadIcon;
    protected StakeCountHint m_StakeCountHint;
    protected Text txt_StakeSum;
    protected Text txt_GiveUp;
    protected ZjhManager_Stand m_ZjhManager;

    /// <summary>
    /// 倒计时
    /// </summary>
    protected float m_Time = 60f;

    /// <summary>
    /// 计时器
    /// </summary>
    protected float m_Timer = 0.0f;
    protected int m_CardPointX = -40;
    protected List<GameObject> go_SpawnCardList = new List<GameObject>();
    public GameObject go_CardPre;
    protected Text txt_CountDown;
    protected Transform CardPoints;


    public abstract void Win();
    public abstract void Lose();
    public virtual void StakesAfter(int count, string str)
    {
        m_StakeCountHint.Show(count + str);
        m_StakeSum += count;
        txt_StakeSum.text = m_StakeSum.ToString();
    }
    
    public void BecomeBanker()
    {
        img_Banker.gameObject.SetActive(true);
    }
    
    public virtual void StartStakes()
    {
        m_IsStartStakes = true;
        go_CountDown.SetActive(true);
        txt_CountDown.text = "60";
        m_Time = 60;
    }
    
    public void DealCard(Card card, float duration, Vector3 initPos)
    {
        m_CardList.Add(card);
        GameObject go = Instantiate(go_CardPre, CardPoints);
        go.GetComponent<RectTransform>().localPosition = initPos;
        go.GetComponent<RectTransform>().DOLocalMove(new Vector3(m_CardPointX, 0, 0), duration);
        
        go_SpawnCardList.Add(go);
        m_CardPointX += 40;
    }
    public virtual void DealCardFinished()
    {
        SortCards();
        GetCardType();
        print("牌型：" + m_CardType);
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
    protected void GetCardType()
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
