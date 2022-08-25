using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Protocol.Code;
using UnityEngine;
using UnityEngine.UI;

public class SelfManager_Stand : BaseManager_Stand
{
    private GameObject go_BottomButton;
    private Text txt_UserName;
    private Text txt_CoinCount;
    private Button btn_Ready;

    private Button btn_LookCard;
    private Button btn_FollowStakes;
    private Button btn_AddStakes;
    private Button btn_CompareCard;
    private Button btn_GiveUp;
    private Toggle tog_2;
    private Toggle tog_5;
    private Toggle tog_10;
    private GameObject go_CompareBtns;
    private Button btn_CompareLeft;
    private Button btn_CompareRight;

    public void Awake()
    {
        EventCenter.AddListener(EventDefine.UpdateCoinCount, UpdateCoinCount);
        Init();
    }

    public void FixedUpdate()
    {
        if (tog_2.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.gray;
            tog_5.GetComponent<Image>().color = Color.white;
            tog_10.GetComponent<Image>().color = Color.white;
        }

        if (tog_5.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.white;
            tog_5.GetComponent<Image>().color = Color.gray;
            tog_10.GetComponent<Image>().color = Color.white;
        }

        if (tog_10.isOn)
        {
            tog_2.GetComponent<Image>().color = Color.white;
            tog_5.GetComponent<Image>().color = Color.white;
            tog_10.GetComponent<Image>().color = Color.gray;
        }

        if (m_IsStartStakes)
        {
            if (m_ZjhManager.IsSelfWin())
            {
                m_ZjhManager.SelfWin();
                m_IsStartStakes = false;
                return;
            }

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

    public override void Win()
    {
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        m_ZjhManager.m_CurrentStakesIndex = 0;
        m_ZjhManager.SetNextPlayerStakes();
    }

    public override void Lose()
    {
        OnGiveUpCardButtonClick();
    }

    private void OnGiveUpCardButtonClick()
    {
        m_IsStartStakes = false;
        go_BottomButton.SetActive(false);
        go_CountDown.SetActive(false);
        m_IsGiveUpCard = true;
        txt_GiveUp.gameObject.SetActive(true);
        go_CompareBtns.SetActive(false);

        foreach (var item in go_SpawnCardList)
        {
            Destroy(item);
        }

        m_ZjhManager.SetNextPlayerStakes();
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
        btn_AddStakes.onClick.AddListener(OnAddStakeButtonClick);
        btn_CompareCard = transform.Find("BottomButton/btn_CompareCard").GetComponent<Button>();
        btn_CompareCard.onClick.AddListener(OnCompareButtonClick);
        btn_GiveUp = transform.Find("BottomButton/btn_GiveUp").GetComponent<Button>();
        btn_GiveUp.onClick.AddListener(OnGiveUpCardButtonClick);
        tog_2 = transform.Find("BottomButton/tog_2").GetComponent<Toggle>();
        tog_5 = transform.Find("BottomButton/tog_5").GetComponent<Toggle>();
        tog_10 = transform.Find("BottomButton/tog_10").GetComponent<Toggle>();
        go_CompareBtns = transform.Find("CompareBtns").gameObject;
        btn_CompareLeft = transform.Find("CompareBtns/btn_CompareLeft").GetComponent<Button>();
        btn_CompareLeft.onClick.AddListener(OnCompareLeftButtonClick);
        btn_CompareRight = transform.Find("CompareBtns/btn_CompareRight").GetComponent<Button>();
        btn_CompareLeft.onClick.AddListener(OnCompareRightButtonClick);

        btn_LookCard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_FollowStakes.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_AddStakes.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_CompareCard.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        btn_GiveUp.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_2.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_5.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        tog_10.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;

        go_BottomButton.SetActive(false);
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        txt_GiveUp.gameObject.SetActive(false);
        go_CompareBtns.SetActive(false);

        if (Models.GameModel.userDto != null)
        {
            img_HeadIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
            txt_UserName.text = Models.GameModel.userDto.UserName;
            txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
            txt_StakeSum.text = "0";
        }

    }

    private void OnCompareLeftButtonClick()
    {
        m_ZjhManager.SelfCompareLeft();
        SetBottomButtonInteractable(false);
    }

    private void OnCompareRightButtonClick()
    {
        m_ZjhManager.SelfCompareRight();
        SetBottomButtonInteractable(false);
    }

    private void OnCompareButtonClick()
    {
        go_CompareBtns.SetActive(true);
        if (m_ZjhManager.LeftIsGiveUp)
        {
            btn_CompareLeft.gameObject.SetActive(false);
        }

        if (m_ZjhManager.RightIsGiveUp)
        {
            btn_CompareRight.gameObject.SetActive(false);
        }
    }

    private void OnAddStakeButtonClick()
    {
        if (tog_2.isOn)
        {
            StakesAfter(m_ZjhManager.Stakes(m_ZjhManager.Stakes(0) * 1), "不看");
        }

        if (tog_5.isOn)
        {
            StakesAfter(m_ZjhManager.Stakes(m_ZjhManager.Stakes(0) * 4), "不看");
        }

        if (tog_10.isOn)
        {
            StakesAfter(m_ZjhManager.Stakes(m_ZjhManager.Stakes(0) * 9), "不看");
        }

        m_IsStartStakes = false;
        go_CountDown.gameObject.SetActive(false);
        SetBottomButtonInteractable(false);
        m_ZjhManager.SetNextPlayerStakes();
        go_CompareBtns.SetActive(false);
    }

    private void OnFollowStakesButtonClick()
    {
        int stakes = m_ZjhManager.Stakes(0);
        m_ZjhManager.SetNextPlayerStakes();
        m_IsStartStakes = false;
        go_CountDown.SetActive(false);
        SetBottomButtonInteractable(false);
        StakesAfter(stakes, "不看");
        go_CompareBtns.SetActive(false);
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

    public override void StakesAfter(int count, string str)
    {
        base.StakesAfter(count, str);
        if (NetMsgCenter.Instance != null)
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

    override public void DealCardFinished()
    {
        go_BottomButton.SetActive(true);
        SetBottomButtonInteractable(false);

        base.DealCardFinished();
    }

    public override void StartStakes()
    {
        base.StartStakes();
        SetBottomButtonInteractable(true);
    }

}