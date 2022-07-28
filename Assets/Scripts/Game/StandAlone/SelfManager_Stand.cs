using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Protocol.Code;
using UnityEngine;
using UnityEngine.UI;

public class SelfManager_Stand : MonoBehaviour
{
    private GameObject go_BottomButton;
    private Image img_HeadIcon;
    private Text txt_UserName;
    private Text txt_CoinCount;
    private Image img_Banker;
    private GameObject go_CountDown;
    private Text txt_CountDown;
    private Text txt_StakeSum;
    private Button btn_Ready;
    private Text txt_GiveUp;
    private Transform CardPoints;
    private ZjhManager_Stand m_ZjhManager;
    private StakeCountHint m_StakeCountHint;

    private Button btn_LookCard;
    private Button btn_FollowStakes;
    private Button btn_AddStakes;
    private Button btn_CompareCard;
    private Button btn_GiveUp;
    private Toggle tog_2;
    private Toggle tog_5;
    private Toggle tog_10;

    private int m_StakeSum = 0;

    private List<Card> m_CardList = new List<Card>();
    public GameObject go_CardPre;
    private int m_CardPointX = -40;
    private CardType m_CardType;

    private List<GameObject> go_SpawnCardList = new List<GameObject>();

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
        EventCenter.AddListener(EventDefine.UpdateCoinCount, UpdateCoinCount);
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
                OnFollowStakesButtonClick();
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
        m_StakeCountHint = transform.Find("StakeCountHint").GetComponent<StakeCountHint>();
        m_ZjhManager = gameObject.GetComponentInParent<ZjhManager_Stand>();
        go_BottomButton = transform.Find("BottomButton").gameObject;
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("Coin/txt_CoinCount").GetComponent<Text>();
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        txt_StakeSum = transform.Find("StakeSum/txt_StakeSum").GetComponent<Text>();
        btn_Ready = transform.Find("btn_Ready").GetComponent<Button>();
        btn_Ready.onClick.AddListener(ReadyButtonClick);
        txt_GiveUp = transform.Find("txt_GiveUp").GetComponent<Text>();
        CardPoints = transform.Find("CardPoints");

        btn_LookCard = transform.Find("BottomButton/btn_LookCard").GetComponent<Button>();
        btn_LookCard.onClick.AddListener(OnLookCardButtonClick);
        btn_FollowStakes = transform.Find("BottomButton/btn_FollowStakes").GetComponent<Button>();
        btn_FollowStakes.onClick.AddListener(OnFollowStakesButtonClick);
        btn_AddStakes = transform.Find("BottomButton/btn_AddStakes").GetComponent<Button>();
        btn_CompareCard = transform.Find("BottomButton/btn_CompareCard").GetComponent<Button>();
        btn_GiveUp = transform.Find("BottomButton/btn_GiveUp").GetComponent<Button>();
        tog_2 = transform.Find("BottomButton/tog_2").GetComponent<Toggle>();
        tog_5 = transform.Find("BottomButton/tog_5").GetComponent<Toggle>();
        tog_10 = transform.Find("BottomButton/tog_10").GetComponent<Toggle>();

        go_BottomButton.SetActive(false);
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        txt_GiveUp.gameObject.SetActive(false);

        if (Models.GameModel.userDto != null)
        {
            img_HeadIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
            txt_UserName.text = Models.GameModel.userDto.UserName;
            txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
            txt_StakeSum.text = "0";
        }

    }

    private void OnFollowStakesButtonClick()
    {
        int stakes = m_ZjhManager.Stakes(0);
        m_ZjhManager.SetNextPlayerStakes();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        SetBottomButtonInteractable(false);
        UpdateCoin(stakes, "不看");
    }
    private void OnLookCardButtonClick()
    {
        btn_LookCard.interactable = false;
        for (int i = 0; i < m_CardList.Count; i++)
        {
            go_SpawnCardList[i].GetComponent<Image>().sprite =
                ResourcesManager.LoadCardSprite("card_" + m_CardList[i].Color + "_" + m_CardList[i].Weight);
        }
    }

    private void UpdateCoin(int count, string str)
    {
        m_StakeCountHint.Show(count + str);
        m_StakeSum += count;
        txt_StakeSum.text = m_StakeSum.ToString();
        if(NetMsgCenter.Instance!=null)
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, -count);
    }
    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.UpdateCoinCount, UpdateCoinCount);
    }

    public void ReadyButtonClick()
    {
        m_StakeSum += Models.GameModel.botStacks;
        txt_StakeSum.text = m_StakeSum.ToString();
        if (NetMsgCenter.Instance != null)
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ,
                -Models.GameModel.botStacks);

        m_ZjhManager.ChooseBanker();
        btn_Ready.gameObject.SetActive(false);
    }

    private void SetBottomButtonInteractable(bool value)
    {
        // btn_LookCard.interactable = value;
        btn_FollowStakes.interactable = value;
        btn_AddStakes.interactable = value;
        btn_CompareCard.interactable = value;
        btn_GiveUp.interactable = value;
        tog_2.interactable = value;
        tog_5.interactable = value;
        tog_10.interactable = value;
    }

    private void UpdateCoinCount()
    {
        txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
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
        go_SpawnCardList.Add(go);
        
        m_CardPointX += 40;
    }

    public void DealCardFinished()
    {
        go_BottomButton.SetActive(true);
        SetBottomButtonInteractable(false);
        SortCards();
        GetCardType();
        print("self牌型：" + m_CardType);
    }

    public void StartStakes()
    {
        SetBottomButtonInteractable(true);
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