using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HeroData
{
    public bool m_isOpen;
    public int m_level;
    public int m_attack;
    public int m_defence;
    public int m_speed;
    public int m_hp;
}
[System.Serializable]
public class BossCardData
{
    public bool m_isOpen;
    public int m_level;
}
public class PlayerData
{
    public int m_bestScore;
    public int m_goldOwned;
    public int m_gemOwned;
    public int m_playerIndex;
    public List<HeroData> m_heroList; //내가 캐릭터가 하나 이상일수도있으니까 리스트에 보관해놓습니다.
    public List<BossCardData> m_bossList;    
}
