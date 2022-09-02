using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class VSPanel_Stand : MonoBehaviour
{
    [Serializable]
    public class Player
    {
        public Text txt_Name;
        public Image[] cardsArr;
        public Image img_Lose;
        public Image img_Win;
    }

    public Player m_ComparePlayer;
    public Player m_ComparedPlayer;
    private BaseManager_Stand compare;
    private BaseManager_Stand compared;

    public void Awake()
    {
        EventCenter.AddListener<BaseManager_Stand,BaseManager_Stand>(EventDefine.VSAI,CompareCard);
        EventCenter.AddListener<BaseManager_Stand,BaseManager_Stand,string,string>(EventDefine.VSWithSelf,VSWithSelf);
        // Init();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<BaseManager_Stand,BaseManager_Stand>(EventDefine.VSAI,CompareCard);
        EventCenter.RemoveListener<BaseManager_Stand,BaseManager_Stand,string,string>(EventDefine.VSWithSelf,VSWithSelf);
    }

    private void Init()
    {
        m_ComparePlayer.cardsArr[0] = transform.Find("compare/cards/card_1").GetComponent<Image>();
        m_ComparePlayer.cardsArr[1] = transform.Find("compare/cards/card_2").GetComponent<Image>();
        m_ComparePlayer.cardsArr[2] = transform.Find("compare/cards/card_3").GetComponent<Image>();
        m_ComparePlayer.txt_Name = transform.Find("compare/txt_Name").GetComponent<Text>();
        m_ComparePlayer.img_Lose = transform.Find("compare/img_Lose").GetComponent<Image>();
        m_ComparePlayer.img_Win = transform.Find("compare/img_Win").GetComponent<Image>();
        
        m_ComparedPlayer.cardsArr[0] = transform.Find("compared/cards/card_1").GetComponent<Image>();
        m_ComparedPlayer.cardsArr[1] = transform.Find("compared/cards/card_2").GetComponent<Image>();
        m_ComparedPlayer.cardsArr[2] = transform.Find("compared/cards/card_3").GetComponent<Image>();
        m_ComparedPlayer.txt_Name = transform.Find("compared/txt_Name").GetComponent<Text>();
        m_ComparedPlayer.img_Lose = transform.Find("compared/img_Lose").GetComponent<Image>();
        m_ComparedPlayer.img_Win = transform.Find("compared/img_Win").GetComponent<Image>();

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);
        transform.DOScale(Vector3.zero, 0.3f);
    }

    IEnumerator CompareWin()
    {
        yield return new WaitForSeconds(2f);
        compare.CompareWin();
        compared.CompareLose();
    }

    IEnumerator CompareLose()
    {
        yield return new WaitForSeconds(2f);
        compare.CompareLose();
        compared.CompareWin();
    }
    public void VSWithSelf(BaseManager_Stand compare, BaseManager_Stand compared, string compareName, string comparedName)
    {
        transform.DOScale(Vector3.one, 0.3f).OnComplete(() => { StartCoroutine(Delay());});
        
        m_ComparePlayer.img_Lose.gameObject.SetActive(false);
        m_ComparePlayer.img_Win.gameObject.SetActive(false);
        m_ComparedPlayer.img_Lose.gameObject.SetActive(false);
        m_ComparedPlayer.img_Win.gameObject.SetActive(false);

        m_ComparePlayer.txt_Name.text = compareName;
        m_ComparedPlayer.txt_Name.text = comparedName;

        for (int i = 0; i < compare.m_CardList.Count; i++)
        {
            string cardName = "card_" + compare.m_CardList[i].Color + "_" + compare.m_CardList[i].Weight;
            m_ComparePlayer.cardsArr[i].sprite = ResourcesManager.LoadCardSprite(cardName);
        }

        for (int i = 0; i < compared.m_CardList.Count; i++)
        {
            string cardName = "card_" + compared.m_CardList[i].Color + "_" + compared.m_CardList[i].Weight;
            m_ComparedPlayer.cardsArr[i].sprite = ResourcesManager.LoadCardSprite(cardName);
        }

        CompareCard(compare, compared);
    }

    private void CompareCard(BaseManager_Stand compare, BaseManager_Stand compared)
    {
        this.compare = compare;
        this.compared = compared;
        
        if (compare.m_CardType > compared.m_CardType)
        {
            ComparePlayerWin();
        }
        else if (compare.m_CardType < compared.m_CardType)
        {
            ComparedPlayerWin();
        }
        else
        {
            if (compare.m_CardType == CardType.Min || compare.m_CardType == CardType.Jinhua)
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (compare.m_CardList[i].Weight > compared.m_CardList[i].Weight)
                    {
                        ComparePlayerWin();
                        return;
                    }
                    if (compare.m_CardList[i].Weight < compared.m_CardList[i].Weight)
                    {
                        ComparedPlayerWin();
                        return;
                    }
                }

                ComparedPlayerWin();
                return;
            }

            if (compare.m_CardType == CardType.Duizi)
            {
                if (compare.m_CardList[1].Weight > compared.m_CardList[1].Weight)
                {
                    ComparePlayerWin();
                }
                else if (compare.m_CardList[1].Weight < compared.m_CardList[1].Weight)
                {
                    ComparedPlayerWin();
                }
                else
                {
                    int compareNum = compare.m_CardList[1].Weight == compare.m_CardList[0].Weight
                        ? compare.m_CardList[2].Weight
                        : compare.m_CardList[0].Weight;
                    int comparedNum = compared.m_CardList[1].Weight == compared.m_CardList[0].Weight
                        ? compared.m_CardList[2].Weight
                        : compared.m_CardList[0].Weight;
                    if (compareNum > comparedNum)
                    {
                        ComparePlayerWin();
                    }
                    else
                    {
                        ComparedPlayerWin();
                    }
                }
            }

            if (compare.m_CardType == CardType.Shunzi || compare.m_CardType == CardType.Shunjin || compare.m_CardType == CardType.Baozi)
            {
                if (compare.m_CardList[0].Weight > compared.m_CardList[0].Weight)
                {
                    ComparePlayerWin();
                }
                else
                {
                    ComparedPlayerWin();
                }
            }

            if (compare.m_CardType == CardType.Max)
            {
                ComparedPlayerWin();
            }
                
        }
    }

    /// <summary>
    /// 比较者胜利
    /// </summary>
    public void ComparePlayerWin()
    {
        StartCoroutine(CompareWin());
        m_ComparePlayer.img_Win.gameObject.SetActive(true);
        m_ComparedPlayer.img_Lose.gameObject.SetActive(true);
    }

    /// <summary>
    /// 被比较者胜利
    /// </summary>
    public void ComparedPlayerWin()
    {
        StartCoroutine(CompareLose());
        m_ComparePlayer.img_Lose.gameObject.SetActive(true);
        m_ComparedPlayer.img_Win.gameObject.SetActive(true);
    }
}
