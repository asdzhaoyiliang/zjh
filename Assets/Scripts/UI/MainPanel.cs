using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    private Text txt_UserName;
    private Text txt_CoinCount;
    private Image headIcon;
    private Button btn_Rank;
    private Button btn_Bank;
    private Button btn_Stand;
    private Button btn_Online;

    public void Awake()
    {
        Init();
    }

    private void Init()
    {
        txt_UserName = transform.Find("txt_UserName").GetComponent<Text>();
        txt_CoinCount = transform.Find("txt_CoinCount").GetComponent<Text>();
        headIcon = transform.Find("mask/headIcon").GetComponent<Image>();
        btn_Rank = transform.Find("btn_Rank").GetComponent<Button>();
        btn_Bank = transform.Find("btn_Bank").GetComponent<Button>();
        btn_Stand = transform.Find("btn_Stand").GetComponent<Button>();
        btn_Online = transform.Find("btn_Online").GetComponent<Button>();

        txt_UserName.text = Models.GameModel.userDto.UserName;
        txt_CoinCount.text = Models.GameModel.userDto.CoinCount.ToString();
        headIcon.sprite = ResourcesManager.GetSprite(Models.GameModel.userDto.IconName);
    }
}
