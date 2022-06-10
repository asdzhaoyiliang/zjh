using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    private float hintShowTime = 2f;
    private Text hintText;
    private Color color;

    private void Awake()
    {
        EventCenter.AddListener<string>(EventDefine.Hint,Show);
        hintText = GetComponent<Text>();
        color = hintText.color;
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<string>(EventDefine.Hint,Show);
    }

    private void Show(string text)
    {
        hintText.text = text;
        hintText.DOColor(new Color(color.r, color.g, color.b, 1), 0.3f).OnComplete(() =>
        {
            StartCoroutine(Dealy());
        });
    }

    private IEnumerator Dealy()
    {
        yield return new WaitForSeconds(hintShowTime);
        hintText.DOColor(new Color(color.r, color.g, color.b, 0), 0.3f);
    }
}
