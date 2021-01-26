using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : SingletonMonoBehaviour<StageController>
{
    [SerializeField]
    private UILabel m_progressValue;
    [SerializeField]
    private UISprite m_progressGauge;

    private float m_time;
    private float m_duration = 100f;

    private bool m_isReady;
    public bool IsReady { get { return m_isReady; } set { m_isReady = value; } }
    void Start()
    {       
        gameObject.SetActive(false);        
    }
    public void OnStageProgress()
    {
        m_isReady = false;
        m_progressValue.text = string.Empty;
        m_progressGauge.fillAmount = 0f;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        if(!m_isReady)
        {
            m_time += Time.deltaTime * 10;
            m_progressValue.text = string.Format("{0}% / {1}%", Mathf.FloorToInt(m_time), Mathf.FloorToInt(m_duration));
            m_progressGauge.fillAmount = m_time / m_duration;
            if(m_time >= m_duration)
            {
                m_isReady = true;
                m_time = 0f;
                m_progressValue.text = string.Empty;
                m_progressGauge.fillAmount = 1f;
                GameController.Instance.LoadStage();
                gameObject.SetActive(false);
            }
        }
    }
}
