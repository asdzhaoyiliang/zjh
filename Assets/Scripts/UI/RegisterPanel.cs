using System;
using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegisterPanel : MonoBehaviour
{
    private InputField input_UserName;
    private InputField input_PassWord;
    private Button btn_Back;
    private Button btn_Register;
    private Button btn_Pwd;
    //是否显示密码
    private bool isShowPassWord = false;

    public void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRegisterPanel, Show);
        Init();
        gameObject.SetActive(false);
    }

    private void Init()
    {
        input_UserName = transform.Find("UserName/input_UserName").GetComponent<InputField>();
        input_PassWord = transform.Find("PassWord/input_PassWord").GetComponent<InputField>();
        btn_Back = transform.Find("btn_Back").GetComponent<Button>();
        btn_Back.onClick.AddListener(OnBackButtonClick);
        btn_Register = transform.Find("btn_Register").GetComponent<Button>();
        btn_Register.onClick.AddListener(OnRegisterButtonClick);
        btn_Pwd = transform.Find("btn_Pwd").GetComponent<Button>();
        btn_Pwd.onClick.AddListener(OnPwdButtonClick);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRegisterPanel,Show);
    }

    private void OnBackButtonClick()
    {
        gameObject.SetActive(false);
        EventCenter.Broadcast(EventDefine.ShowLoginPanel);
    }

    private void OnPwdButtonClick()
    {
        isShowPassWord = !isShowPassWord;
        if (isShowPassWord)
        {
            input_PassWord.contentType = InputField.ContentType.Standard;
            btn_Pwd.GetComponentInChildren<Text>().text = "隐藏";
        }
        else
        {
            input_PassWord.contentType = InputField.ContentType.Password;
            btn_Pwd.GetComponentInChildren<Text>().text = "显示";
        }
        EventSystem.current.SetSelectedGameObject(input_PassWord.gameObject);
    }

    private void OnRegisterButtonClick()
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
        //向服务器发送数据，注册用户
        AccountDto dto = new AccountDto(input_UserName.text, input_PassWord.text);
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.Register_CREQ, dto);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

}
