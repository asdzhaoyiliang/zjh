using System.Collections;
using System.Collections.Generic;
using Protocol.Code;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AccountHandler : BaseHandler
{
    public override void OnReceive(int subCode, object value)
    {
        switch (subCode)
        {
            case AccountCode.Register_SRES:
                Register_SRES((int)value);
                break;
            case AccountCode.Login_SRES:
                Login_SRES((int)value);
                break;
            case AccountCode.GetUserInfo_SRES:
                GetUserInfo_SRES((UserDto)value);
                break;
            case AccountCode.GetRankList_SRES:
                GetRankList_SRES((RankListDto)value);
                break;
            case AccountCode.UpdateCoinCount_SRES:
                UpdateCoinCount_SRES((int)value);
                break;
            default:
                break;
        }
    }

    private void UpdateCoinCount_SRES(int value)
    {
        Models.GameModel.userDto.CoinCount = value;
        EventCenter.Broadcast(EventDefine.UpdateCoinCount);
    }

    private void GetRankList_SRES(RankListDto value)
    {
        EventCenter.Broadcast(EventDefine.SendRankListDto, value);
    }

    private void GetUserInfo_SRES(UserDto value)
    {
        Models.GameModel.userDto = value;
        //跳转到主场景
        SceneManager.LoadScene("2.Main");
    }

    /// <summary>
    /// 登录服务器的响应
    /// </summary>
    /// <param name="value"></param>
    private void Login_SRES(int value)
    {
        if (value == -1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "用户不存在");
            return;
        }

        if (value == -2)
        {
            EventCenter.Broadcast(EventDefine.Hint, "密码不正确");
            return;
        }

        if (value == -3)
        {
            EventCenter.Broadcast(EventDefine.Hint, "账号已在线");
            return;
        }

        if (value == 0)
        {
            NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.GetUserInfo_CREQ, null);
            EventCenter.Broadcast(EventDefine.Hint, "登录成功");
            return;
        }
    }

    private void Register_SRES(int value)
    {
        if (value == -1)
        {
            EventCenter.Broadcast(EventDefine.Hint, "用户名已被注册");
            return;
        }

        if (value == 0)
        {
            EventCenter.Broadcast(EventDefine.Hint, "注册成功");
            return;
        }
    }
}