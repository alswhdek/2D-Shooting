using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx;
public class PlayerDataManager : DontDestroy<PlayerDataManager>
{
    public enum PlayerType
    {
        None = -1,
        OneCharcter,
        TwoCharcter,
        ThreeCharcter,
        Max
    }

    int BasicCoin = 70000; //초기 골드
    int BasicGem = 700; //초기 잼
    int MaxHeroCount = 13; //초기 선택,구매 할수있는 캐릭터 갯수  
    PlayerData m_playerData;
    //PlayerController m_player;
    public int GetGold() //현재 골드
    {
        return m_playerData.m_goldOwned;
    }
    public void SetGold(int value)
    {
        m_playerData.m_goldOwned -= value;
    }
    public void IncreaseGold(int gold) //획득 골드
    {
        m_playerData.m_gemOwned += gold;
        Save();
    }
    public bool DecreaseGold(int value)
    {
        if (m_playerData.m_goldOwned - value >= 0)
        {
            m_playerData.m_goldOwned -= value;
            return true;
        }
        return false;
    }

    public int GetGem()
    {
        return m_playerData.m_gemOwned;
    }
    public void IncreaseGem(int gem)
    {
        m_playerData.m_gemOwned += gem;
    }
    public bool DecreaseGem(int value)
    {
        if (m_playerData.m_gemOwned - value >= 0)
        {
            m_playerData.m_gemOwned -= value;
            Save();
            return true;
        }
        return false;
    }

    public void SetPlayerIndex(int index)
    {
        m_playerData.m_playerIndex = index;
        if(InventoryController.Instance != null)
        {
            InventoryController.Instance.LoadCharcterSprite(m_playerData.m_playerIndex);
        }
        Save();
    }
    public int GetPlayerIndex()
    {
        return m_playerData.m_playerIndex;
    }
    public bool isOpenBoss(int index)
    {
        return m_playerData.m_bossList[index].m_isOpen;
    }
    public void OpenBossCard(int index)
    {
        m_playerData.m_bossList[index].m_isOpen = true;
        Save();
    }
    public bool isOpenHero(int index)
    {
        return m_playerData.m_heroList[index].m_isOpen;
    }
    public void OpenHero(int index)
    {
        m_playerData.m_heroList[index].m_isOpen = true;
        Save();
    }

    public int GetBestScore()
    {
        return m_playerData.m_bestScore;
    }
    public void SetBestScore(int score)
    {
        if (score > m_playerData.m_bestScore)
            m_playerData.m_bestScore = score;
        Save();
    }
    
    //플레이어 스텟
    public int GetAttack(int index)
    {
        return m_playerData.m_heroList[index].m_attack;
    }
    public void SetAttack(int index,int value,bool isMounting)
    {
        if (isMounting)
        {
            m_playerData.m_heroList[index].m_attack += value;
        }
        else
        {
            m_playerData.m_heroList[index].m_attack -= value;
        }
        Save();
    }
    public int GetDefence(int index)
    {
        return m_playerData.m_heroList[index].m_defence;
    }
    public void SetDefence(int index, int value,bool isMounting)
    {
        if (isMounting)
        {
            m_playerData.m_heroList[index].m_defence += value;
        }
        else
        {
            m_playerData.m_heroList[index].m_defence -= value;
        }
        Save();
    }
    public int GetSpeed(int index)
    {
        return m_playerData.m_heroList[index].m_speed;
    }
    public void SetSpeed(int index, int value,bool isMounting)
    {
        if (isMounting)
        {
            m_playerData.m_heroList[index].m_speed += value;
        }
        else
        {
            m_playerData.m_heroList[index].m_speed -= value;
        }
        Save();
    }
    public int GetHp(int index)
    {
        return m_playerData.m_heroList[index].m_hp;
    }
    public void SetHp(int index, int value, bool isMounting)
    {
        if (isMounting)
        {
            m_playerData.m_heroList[index].m_hp += value;
        }
        else
        {
            m_playerData.m_heroList[index].m_hp -= value;
        }
        Save();
    }
    public void Save()
    {
        var result = JsonFx.Json.JsonWriter.Serialize(m_playerData); //저장할 데이터를 가져온다.
        PlayerPrefs.SetString("PlayerData", result); //저장한 데이터를 문자열로 변경한다.
        PlayerPrefs.Save(); // 저장
    }
    public bool Load()
    {
        var result = PlayerPrefs.GetString("PlayerData", string.Empty); //로드할 데이터를 가져와서 가져온 데이터가 비었으면 빈공간으로 유지한다.
        if (!string.IsNullOrEmpty(result)) //로드할 데이터가 있으면
        {
            m_playerData = JsonFx.Json.JsonReader.Deserialize<PlayerData>(result); // 로드한 데이터를 복호화 시켜준다.
            return true;
        }
        return false;
    }

    protected override void OnAwake()
    {      
        //PlayerPrefs.DeleteAll(); //로드랑 같이 빌드후 실행을 해줘야지 초기화된다.       
        if (!Load())
        {                       
            m_playerData = new PlayerData()
            {
                m_bestScore = 0,
                m_goldOwned = BasicCoin,
                m_gemOwned = BasicGem,
                m_heroList = new List<HeroData>(3)
                {
                    new HeroData() { m_isOpen = true, m_level = 1,m_attack=1,m_defence=0,m_speed=10,m_hp=200 },
                    new HeroData() { m_isOpen = false, m_level = 1,m_attack=2,m_defence=1,m_speed=15,m_hp=250 },
                    new HeroData() { m_isOpen = false, m_level = 1,m_attack=3,m_defence=2,m_speed=20,m_hp=300 },
                },
                m_bossList = new List<BossCardData>(5)
                {
                    new BossCardData() { m_isOpen = false, m_level = 10 },
                    new BossCardData() { m_isOpen = false, m_level = 20 },
                    new BossCardData() { m_isOpen = false, m_level = 30 },
                    new BossCardData() { m_isOpen = false, m_level = 40 },
                    new BossCardData() { m_isOpen = false, m_level = 50 },
                },
                m_playerIndex = 0,
            };         
            Save();
        }
    }
}

