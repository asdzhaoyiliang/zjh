using System.Collections;
using System.Collections.Generic;
using Protocol.Dto;
using UnityEngine;

/// <summary>
/// 游戏数据
/// </summary>
public class GameModel
{
    //用户信息
    public UserDto userDto { get; set; }

    //底注
    public int botStacks { get; set; }

    //顶注
    public int topStacks { get; set; }
}