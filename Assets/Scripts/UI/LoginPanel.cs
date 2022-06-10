using System;
using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviour
{

    private InputField input_UserName;
    private InputField input_PassWord;
    private Button btn_Login;
    private Button btn_Register;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowLoginPanel,Show);
        Init();
    }

    private void Init()
    {
        input_UserName = transform.Find("input_UserName").GetComponent<InputField>();
        input_PassWord = transform.Find("input_PassWord").GetComponent<InputField>();
        btn_Login = transform.Find("btn_Login").GetComponent<Button>();
        btn_Login.onClick.AddListener(OnLoginButtonClick);
        btn_Register = transform.Find("btn_Register").GetComponent<Button>();
        btn_Register.onClick.AddListener(OnRegisterButtonClick);
    }
    private void OnRegisterButtonClick()
    {
        EventCenter.Broadcast(EventDefine.ShowRegisterPanel);
    }
    private void OnLoginButtonClick()
    {
        if (input_UserName.text == null || input_UserName.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint,"请输入用户名");
            // Debug.Log("请输入用户名");
            return;
        }
        if (input_PassWord.text == null || input_PassWord.text == "")
        {
            EventCenter.Broadcast(EventDefine.Hint,"请输入密码");
            // Debug.Log("请输入密码");
            return;
        }
        //向服务器发送数据，用户登录
        AccountDto dto = new AccountDto(input_UserName.text,input_PassWord.text);
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.Login_CREQ, dto);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowLoginPanel,Show);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}
