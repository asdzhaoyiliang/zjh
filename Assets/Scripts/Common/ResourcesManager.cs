﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    private static Dictionary<string, Sprite> nameSpriteDic = new Dictionary<string, Sprite>();
    /// <summary>
    /// 获取图集
    /// </summary>
    /// <param name="iconName"></param>
    /// <returns></returns>
    public static Sprite GetSprite(string iconName)
    {
        if (nameSpriteDic.ContainsKey(iconName))
        {
            return nameSpriteDic[iconName];
        }
        else
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("headIcon");
            string[] nameArr = iconName.Split('_');
            Sprite temp = sprites[int.Parse(nameArr[1])];
            nameSpriteDic.Add(iconName, temp);
            return temp;
        }
    }
}
