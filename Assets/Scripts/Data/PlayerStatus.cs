using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Status
{
    public int m_level;
    public int m_exp;
    public int m_hp;
    public int m_attack;
    public float m_moveSpeed;
    public int m_defence;

    public Status(int level,int exp,int hp,int attack, float moveSpeed,int defence)
    {
        m_level = level;
        m_exp = exp;
        m_hp = hp;
        m_attack = attack;
        m_moveSpeed = moveSpeed;
        m_defence = defence;
    }
}
