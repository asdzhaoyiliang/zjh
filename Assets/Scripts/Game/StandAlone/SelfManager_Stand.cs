using System;
using System.Collections;
using System.Collections.Generic;
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
    private Transform CardPonits;
    private ZjhManager_Stand m_ZjhManager;

    private Button btn_LookCard;
    private Button btn_FollowStakes;
    private Button btn_AddStakes;
    private Button btn_CompareCard;
    private Button btn_GiveUp;
    private Toggle tog_2;
    private Toggle tog_5;
    private Toggle tog_10;

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
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
        CardPonits = transform.Find("CardPoints");

        btn_LookCard = transform.Find("BottomButton/btn_LookCard").GetComponent<Button>();
        btn_FollowStakes = transform.Find("BottomButton/btn_FollowStakes").GetComponent<Button>();
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

        img_HeadIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
        txt_UserName.text = Models.GameModel.userDto.UserName;
        txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
        txt_StakeSum.text = "0";
    }

    public void ReadyButtonClick()
    {
        m_ZjhManager.ChooseBanker();
        go_BottomButton.SetActive(true);
        SetBottomButtonInteractable(false);
        btn_Ready.gameObject.SetActive(false);
    }

    private void SetBottomButtonInteractable(bool value)
    {
        btn_LookCard.interactable = value;
        btn_FollowStakes.interactable = value;
        btn_AddStakes.interactable = value;
        btn_CompareCard.interactable = value;
        btn_GiveUp.interactable = value;
        tog_2.interactable = value;
        tog_5.interactable = value;
        tog_10.interactable = value;
    }
}