using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : SingletonMonoBehaviour<UIController>
{
    [SerializeField]
    private UILabel m_score;
    [SerializeField]
    private UILabel m_stage;
    [SerializeField]
    private UILabel m_gold;
    [SerializeField]
    private UILabel m_level;
    [SerializeField]
    private UI2DSprite m_exp;
    [SerializeField]
    private UILabel m_expLabel;
    [SerializeField]
    private UISprite m_hp;
    [SerializeField]
    private UILabel m_hpText;

    public void ShowScore(int score)
    {
        m_score.text = string.Format("Score : {0:00}", score);
    }
    public void ShowStage(int stage)
    {
        m_stage.text = string.Format("Stage : {0:00}", stage);
    }
    public void ShowGold(int gold)
    {
        m_gold.text = string.Format("{0:00}", gold);
    }
    public void ShowHp(float current,float max)
    {
        m_hp.fillAmount = current / max;
        if(current <=0)
        {
            m_hpText.text = string.Empty;
        }
        else
        {
            m_hpText.text = string.Format("{0:00} / {1}", current, max);
        }
    }
    public void ShowLevel(int level)
    {
        m_level.text = string.Format("LEVEL_" + level);
    }
    public void ShowExp(int currentExp,float maxExp)
    {
        m_exp.fillAmount = currentExp / maxExp;
        m_expLabel.text = string.Format("{0:00} / {1}", currentExp, maxExp);
    }
}
