using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using Protocol.Code;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomChoosePanel : MonoBehaviour
{
    private Button btn_EnterRoom1;
    private Button btn_EnterRoom2;
    private Button btn_EnterRoom3;
    private Button btn_Close;
    private GameType m_gameType;

    public void Awake()
    {
        EventCenter.AddListener<GameType>(EventDefine.ShowRoomChoosePanel, Show);
        Init();
    }

    public void Init()
    {
        btn_EnterRoom1 = transform.Find("Room_1/btn_EnterRoom").GetComponent<Button>();
        btn_EnterRoom1.onClick.AddListener(() => { EnterRoom(10, 100); });
        btn_EnterRoom2 = transform.Find("Room_2/btn_EnterRoom").GetComponent<Button>();
        btn_EnterRoom2.onClick.AddListener(() => { EnterRoom(100, 200); });
        btn_EnterRoom3 = transform.Find("Room_3/btn_EnterRoom").GetComponent<Button>();
        btn_EnterRoom3.onClick.AddListener(() => { EnterRoom(200, 400); });
        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(() => { Close(); });
    }

    public void OnDestroy()
    {
        EventCenter.RemoveListener<GameType>(EventDefine.ShowRoomChoosePanel, Show);
    }

    public void Show(GameType gameType)
    {
        m_gameType = gameType;
        transform.DOScale(Vector3.one, 0.3f);
    }

    public void Close()
    {
        transform.DOScale(Vector3.zero, 0.3f);
    }

    public void EnterRoom(int botStacks, int topStacks)
    {
        Models.GameModel.botStacks = botStacks;
        Models.GameModel.topStacks = topStacks;
        switch (m_gameType)
        {
            case GameType.Net:
                // NetMsgCenter.Instance.SendMsg(OpCode.Match, AccountCode.Login_CREQ,);
                break;
            case GameType.StandAlone:
                SceneManager.LoadScene("3.StandAlone");
                break;
            default:
                break;
        }
    }
}
