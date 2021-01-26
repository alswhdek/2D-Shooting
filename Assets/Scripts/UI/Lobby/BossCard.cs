using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCard : MonoBehaviour, ILobbyMenu
{
    [SerializeField]
    private GameObject m_cardObj;
    private TweenAlpha[] m_tween;
    List<string> m_nameList = new List<string>() { "보스01", "보스02", "보스03", "보스04", "보스05" };
    void Awake()
    {
        m_tween = m_cardObj.GetComponentsInChildren<TweenAlpha>();       
        LoadCard();
    }

    public void LoadCard()
    {
        for(int i=0; i<m_nameList.Count; i++)
        {
            var boss = PlayerDataManager.Instance.isOpenBoss(i);
            if(boss)
            {
                m_tween[i].to = 1;
            }
        }
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
}
