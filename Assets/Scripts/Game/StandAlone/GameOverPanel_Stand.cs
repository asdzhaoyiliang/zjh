using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using Protocol.Code;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel_Stand : MonoBehaviour
{
    [Serializable]
    public class Player
    {
        public Text txt_CoinCount;
        public Image img_Win;
        public Image img_Lose;
    }

    public AudioClip clip_Win;
    public AudioClip clip_Lose;
    public Player m_LeftPlayer;
    public Player m_SelfPlayer;
    public Player m_RightPlayer;

    private Button btn_Again;
    private Button btn_MainMenu;
    private AudioSource m_AudioSource;

    public void Awake()
    {
        EventCenter.AddListener<int, int, int>(EventDefine.GameOver, GameOver);
        Init();
    }

    public void Init()
    {
        m_AudioSource = GetComponent<AudioSource>();
        
        m_LeftPlayer.txt_CoinCount = transform.Find("Left/txt_CoinCount").GetComponent<Text>();
        m_LeftPlayer.img_Win = transform.Find("Left/img_Win").GetComponent<Image>();
        m_LeftPlayer.img_Lose = transform.Find("Left/img_Lose").GetComponent<Image>();

        m_SelfPlayer.txt_CoinCount = transform.Find("Self/txt_CoinCount").GetComponent<Text>();
        m_SelfPlayer.img_Win = transform.Find("Self/img_Win").GetComponent<Image>();
        m_SelfPlayer.img_Lose = transform.Find("Self/img_Lose").GetComponent<Image>();

        m_RightPlayer.txt_CoinCount = transform.Find("Right/txt_CoinCount").GetComponent<Text>();
        m_RightPlayer.img_Win = transform.Find("Right/img_Win").GetComponent<Image>();
        m_RightPlayer.img_Lose = transform.Find("Right/img_Lose").GetComponent<Image>();

        btn_Again = transform.Find("btn_Again").GetComponent<Button>();
        btn_Again.onClick.AddListener(OnAgainButtonClick);
        btn_MainMenu = transform.Find("btn_MainMenu").GetComponent<Button>();
        btn_MainMenu.onClick.AddListener(OnMainMenuButtonClick);
    }

    public void OnDestroy()
    {
        EventCenter.RemoveListener<int, int, int>(EventDefine.GameOver, GameOver);
    }

    private void GameOver(int leftCoinCount, int selfCoinCount, int rightCoinCount)
    {
        transform.DOScale(Vector3.one, 0.3f);
        
        m_LeftPlayer.img_Win.gameObject.SetActive(false);
        m_LeftPlayer.img_Lose.gameObject.SetActive(false);
        m_SelfPlayer.img_Win.gameObject.SetActive(false);
        m_SelfPlayer.img_Lose.gameObject.SetActive(false);
        m_RightPlayer.img_Win.gameObject.SetActive(false);
        m_RightPlayer.img_Lose.gameObject.SetActive(false);
        //左边
        if (leftCoinCount < 0)
        {
            m_LeftPlayer.img_Lose.gameObject.SetActive(true);
            m_LeftPlayer.txt_CoinCount.text = leftCoinCount.ToString();
        }
        else
        {
            m_LeftPlayer.img_Win.gameObject.SetActive(true);
            m_LeftPlayer.txt_CoinCount.text = (Mathf.Abs(selfCoinCount + rightCoinCount) + leftCoinCount).ToString();
        }
        //自身
        if (selfCoinCount < 0)
        {
            m_AudioSource.clip = clip_Lose;
            m_AudioSource.Play();
            m_SelfPlayer.img_Lose.gameObject.SetActive(true);
            m_SelfPlayer.txt_CoinCount.text = selfCoinCount.ToString();
        }
        else
        {
            m_AudioSource.clip = clip_Win;
            m_AudioSource.Play();
            var winCoin = Mathf.Abs(leftCoinCount + rightCoinCount) + selfCoinCount;
            if (NetMsgCenter.Instance != null)
            {
                NetMsgCenter.Instance.SendMsg(OpCode.Account, AccountCode.UpdateCoinCount_CREQ, winCoin);
            }
            m_SelfPlayer.img_Win.gameObject.SetActive(true);
            m_SelfPlayer.txt_CoinCount.text = winCoin.ToString();
        }
        //右边
        if (rightCoinCount < 0)
        {
            m_RightPlayer.img_Lose.gameObject.SetActive(true);
            m_RightPlayer.txt_CoinCount.text = rightCoinCount.ToString();
        }
        else
        {
            m_RightPlayer.img_Win.gameObject.SetActive(true);
            m_RightPlayer.txt_CoinCount.text = (Mathf.Abs(selfCoinCount + leftCoinCount) + rightCoinCount).ToString();
        }
    }

    /// <summary>
    /// 再来一局
    /// </summary>
    private void OnAgainButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 回到主界面
    /// </summary>
    private void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("2.Main");
    }
}
