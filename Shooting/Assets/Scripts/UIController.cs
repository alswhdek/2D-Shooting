﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mLifeIcon;
    [SerializeField]
    private Text mScoreText,mRestartText,mStateText,mPlayerHPText;
    private Coroutine mAIphaAnimRoutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void LooseLife(int curr)
    {
        mLifeIcon[curr].SetActive(false);
    }

    public void ShowPlayerHP(float current,float max)
    {
        mPlayerHPText.text = string.Format("HP : {0}/{1}", current.ToString(), max.ToString());
    }

    public void ShowRestartText(bool isActive)
    {
        mRestartText.gameObject.SetActive(isActive);
        if (isActive)
        {
            mAIphaAnimRoutine = StartCoroutine(RestartTextRoutine());
        }
        else
        {
            StopCoroutine(mAIphaAnimRoutine);
        }
    }

    private IEnumerator RestartTextRoutine()
    {
        WaitForSeconds pointOne = new WaitForSeconds(.1f);
        mRestartText.color = Color.white;
        Color colorGap = Color.black * 0.1f;
        float timer = 1;
                
        bool bDown = true;
        while(true)
        {
            yield return pointOne;
            if (bDown)
            {
                mRestartText.color -= colorGap;
            }
            else
            {
                mRestartText.color += colorGap;
            }
            timer -= .1f;

            if(timer <= 0)
            {
                bDown =!bDown;//false 대입이 아닌 값을 바꿔라
                timer = 1;
            }            
        }
    }

    public void ShowState(string value)
    {
        mStateText.text = value;
    }

    public void ShowScore(float Score)
    {
        mScoreText.text = "Score: " + Score.ToString();
    }
}
