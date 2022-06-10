using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Protocol.Dto;
using UnityEngine;
using UnityEngine.UI;

public class RankListPanel : MonoBehaviour
{
    public GameObject go_ItemPrefab;
    private Button btn_Close;
    private Transform m_Parent;
    public void Awake()
    {
        EventCenter.AddListener(EventDefine.ShowRankListPanel,Show);
        EventCenter.AddListener<RankListDto>(EventDefine.SendRankListDto,GetRankListDto);
        btn_Close = transform.Find("btn_Close").GetComponent<Button>();
        btn_Close.onClick.AddListener(OnCloseButtonClick);
        m_Parent = transform.Find("List/ScrollRect/Parent");
        transform.DOScale(Vector3.zero, 0.3f);

        // gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankListPanel,Show);
    }

    private void OnCloseButtonClick()
    {
        transform.DOScale(Vector3.zero, 0.3f);
        // gameObject.SetActive(false);
    }

    public void Show()
    {
        transform.DOScale(Vector3.one, 0.3f);
        // gameObject.SetActive(true);
    }

    /// <summary>
    /// 得到排行榜数据传输模型
    /// </summary>
    /// <param name="dto"></param>
    private void GetRankListDto(RankListDto dto)
    {
        if (dto == null) return;
        for (int i = 0; i < dto.rankList.Count; i++)
        {
            GameObject go = Instantiate(go_ItemPrefab, m_Parent);
            go.transform.Find("index/txt_Index").GetComponent<Text>().text = (i +1).ToString();
            go.transform.Find("txt_CoinCount").GetComponent<Text>().text = dto.rankList[i].CoinCount.ToString();
            if (dto.rankList[i].UserName == Models.GameModel.userDto.UserName)
            {
                go.transform.Find("index/txt_Index").GetComponent<Text>().color = Color.red;
                go.transform.Find("txt_UserName").GetComponent<Text>().text = "我";
                go.transform.Find("txt_UserName").GetComponent<Text>().color = Color.red;
                go.transform.Find("txt_CoinCount").GetComponent<Text>().color = Color.red;
            }
            else
            {
                go.transform.Find("txt_UserName").GetComponent<Text>().text = dto.rankList[i].UserName;

            }
        }
    }
}
