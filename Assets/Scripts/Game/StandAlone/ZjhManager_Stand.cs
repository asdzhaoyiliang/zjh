using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZjhManager_Stand : MonoBehaviour
{
    private Text txt_BottomStakes;
    private Text txt_TopStakes;
    private Button btn_Back;
    private LeftManager_Stand m_LeftManager;
    private RightManager_Stand m_RightManager;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_LeftManager = GetComponentInChildren<LeftManager_Stand>();
        m_RightManager = GetComponentInChildren<RightManager_Stand>();
        txt_BottomStakes = transform.Find("Main/txt_BottomStakes").GetComponent<Text>();
        txt_TopStakes = transform.Find("Main/txt_TopStakes").GetComponent<Text>();
        btn_Back = transform.Find("Main/btn_Back").GetComponent<Button>();

        txt_BottomStakes.text = Models.GameModel.botStacks.ToString();
        txt_TopStakes.text = Models.GameModel.topStacks.ToString();
        btn_Back.onClick.AddListener(() => { SceneManager.LoadScene("2.Main"); });
    }

    public void ChooseBanker()
    {
        m_LeftManager.StartChooseBanker();
        m_RightManager.StartChooseBanker();
    }
}
