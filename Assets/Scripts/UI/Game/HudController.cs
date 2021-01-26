using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField]
    UILabel m_damageLabel;
    [SerializeField]
    UIProgressBar m_hpBar;
    private float m_duration = 0.5f;
    private float m_time;
    Enemy m_enemy;

    public void SetHp(int currentHp,int hpMax)
    {
        m_hpBar.value = currentHp / hpMax;
    }
    private IEnumerator Coroutine_AciveEnd()
    {
        while(true)
        {
            m_time += Time.deltaTime;
            if(m_time >= m_duration)
            {
                m_time = 0f;
                gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(Coroutine_AciveEnd());
    }
    private void Update()
    {
        transform.position = m_enemy.transform.position;
    }
}
