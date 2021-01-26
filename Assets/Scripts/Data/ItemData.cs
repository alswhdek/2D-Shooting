using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    None = -1,
    Shadow,
    invincibility,
    Magnet,
    Speed,
    ResetHp,
    Gold,
    Attack,
    Max
}
public class ItemData : MonoBehaviour
{
    public int m_icon;
    public int m_attack;
    public float m_duration;
    public float m_speed;
    public int m_gold;
}
