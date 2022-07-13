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
    private Text txt_Ready;
    private Transform CardPonits;

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
        go_BottomButton = transform.Find("BottomButton").gameObject;
        img_HeadIcon = transform.Find("img_HeadIcon").GetComponent<Image>();
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("Coin/txt_CoinCount").GetComponent<Text>();
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        txt_StakeSum = transform.Find("StakeSum/txt_StakeSum").GetComponent<Text>();
        btn_Ready = transform.Find("btn_Ready").GetComponent<Button>();
        txt_Ready = transform.Find("txt_Ready").GetComponent<Text>();
        CardPonits = transform.Find("CardPoints");
        
        go_BottomButton.SetActive(false);
        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        txt_Ready.gameObject.SetActive(false);
    }
}
