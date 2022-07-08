using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Protocol.Code;
using UnityEngine;
using UnityEngine.UI;

public class RechargePanel : MonoBehaviour
{
    private GameObject goods;
    public Button[] goodsBtnArr;
    private Button btn_Close;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.UpdateCoinCount, RechargeSuccess);
        Init();
    }

    private void Init()
    {
        EventCenter.AddListener(EventDefine.ShowRechargePanel, Show);
        goods = transform.Find("goods").gameObject;
        goodsBtnArr = new Button[goods.transform.childCount];
        for (int i = 0; i < goods.transform.childCount; i++)
        {
            goodsBtnArr[i] = goods.transform.GetChild(i).GetComponentInChildren<Button>();
        }

        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        goodsBtnArr[0].onClick.AddListener(delegate { Recharge(10); });
        goodsBtnArr[1].onClick.AddListener(delegate { Recharge(20); });
        goodsBtnArr[2].onClick.AddListener(delegate { Recharge(50); });
        goodsBtnArr[3].onClick.AddListener(delegate { Recharge(100); });
        goodsBtnArr[4].onClick.AddListener(delegate { Recharge(200); });
        goodsBtnArr[5].onClick.AddListener(delegate { Recharge(500); });
    }

    private void Recharge(int coinCount)
    {
        NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, coinCount);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRechargePanel, Show);
    }

    private void OnCloseButtonClick()
    {
        transform.DOScale(Vector3.zero, 0.3f);
    }

    private void Show()
    {
        transform.DOScale(Vector3.one, 0.3f);
    }

    private void RechargeSuccess()
    {
        EventCenter.Broadcast(EventDefine.Hint,"充值成功");
    }
}