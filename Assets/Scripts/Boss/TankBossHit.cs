using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBossHit : GokenBoss
{   
    protected override void Awake()
    {
        m_isDie = false;
        m_type = eBossType.TwoLevel;
        m_currhp = m_hp;
        m_RB = GetComponent<Rigidbody2D>();
        m_sprite = GetComponent<SpriteRenderer>();
    }
    public int GetCurrentHp { get { return m_currhp; } }
}
