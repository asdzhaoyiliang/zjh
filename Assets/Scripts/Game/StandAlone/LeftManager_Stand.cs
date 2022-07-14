using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class LeftManager_Stand : MonoBehaviour
{
    private Image img_Banker;
    private Text txt_StakeSum;
    private Text txt_Ready;
    private GameObject go_CountDown;
    private Text txt_CountDown;
    private Transform CardPoints;

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
        img_Banker = transform.Find("img_Banker").GetComponent<Image>();
        txt_StakeSum = transform.Find("StakeSum/txt_StakeSum").GetComponent<Text>();
        txt_Ready = transform.Find("txt_Ready").GetComponent<Text>();
        go_CountDown = transform.Find("CountDown").gameObject;
        txt_CountDown = transform.Find("CountDown/txt_CountDown").GetComponent<Text>();
        CardPoints = transform.Find("CardPoints");

        img_Banker.gameObject.SetActive(false);
        go_CountDown.SetActive(false);
        txt_StakeSum.text = "0";
    }

    public void StartChooseBanker()
    {
        txt_Ready.gameObject.SetActive(false);
    }
}
