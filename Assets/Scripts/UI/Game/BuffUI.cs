using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : MonoBehaviour
{
    private UI2DSprite m_iconSprite;
    [SerializeField]
    private UI2DSprite m_coolTimeGauge;
    [SerializeField]
    private UILabel m_coolTimeLabel;

    private ItemType m_type;

    private float m_duration;
    private float m_time;
    private int m_iconIndex;
    private bool m_isBuff;
    public bool IsBuff { get { return m_isBuff; }set { m_isBuff = value; } }
    public ItemType GetItemType { get { return m_type; }}
    public void SetUI(ItemType type,float duration,int iconIndex)
    {
        m_type = type;
        m_iconIndex = iconIndex;
        m_duration = duration;
        m_iconSprite.sprite2D = Resources.Load<Sprite>(string.Format("Images/BuffIcon/ActionIcon_{0:00}", m_iconIndex));
        m_coolTimeGauge.fillAmount = 1f;
        m_coolTimeLabel.text = m_duration.ToString();
        m_isBuff = true;
    }
    public void ResetTime()
    {
        m_time = 0f;
    }
    private void Awake()
    {
        m_iconSprite = GetComponent<UI2DSprite>();
    }
    private void Update()
    {
        if (m_isBuff)
        {
            m_time += Time.deltaTime;
            m_coolTimeGauge.fillAmount = m_time / m_duration;
            m_coolTimeLabel.text = Mathf.FloorToInt(m_duration - m_time).ToString();
            
            if(m_time > m_duration)
            {
                m_isBuff = false;
                m_time = 0f;
                m_coolTimeGauge.fillAmount = 0f;
                m_coolTimeLabel.text = string.Empty;             
            }
        }
    }
}
