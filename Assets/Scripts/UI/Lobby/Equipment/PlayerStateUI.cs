using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUI : SingletonMonoBehaviour<PlayerStateUI>
{
    [SerializeField]
    private UILabel m_attackLabel;
    [SerializeField]
    private UILabel m_defenceLabel;
    [SerializeField]
    private UILabel m_speedLabel;
    [SerializeField]
    private UILabel m_hpLabel;

    public void SetPlayerState(int attack, int defence, int speed, int hp)
    {
        m_attackLabel.text = string.Format("공격력 : {0:00}", attack);
        m_defenceLabel.text = string.Format("방어력 : {0:00}", defence);
        m_speedLabel.text = string.Format("스피드 : {0:00}", speed);
        m_hpLabel.text = string.Format("체력 : {0:00}", hp);
    }
}
