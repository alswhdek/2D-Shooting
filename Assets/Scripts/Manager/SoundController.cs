using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class SoundController : DontDestroy<SoundController> //싱글턴
{
    public enum eBGMType
    {
        None=-1,
        Lobby,
        Characters,
        Stage1,
        Stage1Boss,
        Stage2,
        Stage2Boss,
        Stage3,
        Stage3Boss,
        Stage4,
        Stage4Boss,
        Stage5,
        Stage5Boss,
        Max
    }
    public enum ePlayerSfxType
    {
        None = -1,
        Player01_01,
        Player01_02,
        Player01_03,
        Player02_01,
        Player02_02,
        Player02_03,
        Player03_01,
        Player03_02,
        Player03_03,
        PlayerDie,
        Max
    }
    public enum eEnemySfxType
    {
        None=-1,
        Enemy01,
        Enemy02,
        Enemy03,
        Enemy04,
        Enemy05,
        Enemy06,
        Enemy07,
        Enemy08,
        Enemy09,
        Enemy010,
        EnemyDie01_02,
        EnemyDie03_04,
        EnemyDie05_06,
        EnemyDie07_08,
        EnemyDie09_10,
        EnemyHit,
        Max
    }
    public enum eBossSfxType
    {
        None=-1,
        Boss01_Shoot01,
        Boss01_Shoot02,
        Boss01_Hit,        
        Boss02_Shoot,
        Boss02_Hit,        
        Boss03_Shoot,
        Boss03_Hit,        
        Boss04_Move,
        Boss04_Attack,
        Boss04_Hit,        
        Boss05_Shoot01,
        Boss05_Shoot02,
        Boss05_Hit,       
        Max
    }
    public enum eLoopSfxType
    {
        None = -1,
        Boss02_Shoot,
        Boss01_Die,
        Boss02_Die,
        Boss03_Die,
        Boss04_Die,
        Boss05_Die,
        Max
    }
    public enum eItemCoinSound
    {
        None=-1,
        item,
        Max
    }
    [SerializeField]
    private AudioSource m_backGroundSound;
    [SerializeField]
    private AudioSource m_sfxSound;
    [SerializeField]
    private AudioSource m_itemSound;

    [SerializeField]
    private AudioClip[] m_bgmArr;
    [SerializeField]
    private AudioClip[] m_playerSfxArr;
    [SerializeField]
    private AudioClip[] m_enemySfxArr;
    [SerializeField]
    private AudioClip[] m_bossSfxArr;
    [SerializeField]
    private AudioClip[] m_itemArr;
    [SerializeField]
    private AudioClip[] m_loopSfxArr;

    private Dictionary<ePlayerSfxType, int> m_dicPlayerSfxBuff = new Dictionary<ePlayerSfxType, int>();
    private Dictionary<eEnemySfxType, int> m_dicEnemySfxBuff = new Dictionary<eEnemySfxType, int>();
    private Dictionary<eBossSfxType, int> m_dicBossSfxBuff = new Dictionary<eBossSfxType, int>();
    private Dictionary<eItemCoinSound, int> m_dicItemSfxBuff = new Dictionary<eItemCoinSound, int>();

    #region 배경,이펙트 사운드 조절
    public void SetBgmVolume(int volume)
    {
        m_backGroundSound.volume = volume;
    }
    public void SetSfxVolume(int volume)
    {
        m_sfxSound.volume = volume;
    }
    #endregion

    #region 배경,이펙트 사운드 음소거
    public void SetBgmZeroVolume()
    {
        m_backGroundSound.volume = 0;
        m_backGroundSound.Stop();
    }
    public void SetSfxZeroVolume()
    {
        m_sfxSound.volume = 0;
        m_sfxSound.Stop();
    }
    #endregion

    #region 반복 사운드 설정
    public void LoopSound(eLoopSfxType type)
    {
        m_sfxSound.loop = true;
        m_sfxSound.clip = m_loopSfxArr[(int)type];
        m_sfxSound.Play();
        Debug.Log("Die Sound!!");
    }
    public void EndLoopSound()
    {
        m_sfxSound.loop = false;
    }
    #endregion

    #region 배경 사운드 재생
    public void PlayBackgroundSound(eBGMType type)
    {
        m_backGroundSound.clip = m_bgmArr[(int)type];
        m_backGroundSound.Play();
    }
    #endregion

    #region 플레이어 이펙트 사운드 재생
    public void SfxPlaySound(ePlayerSfxType type)
    {
        if (!m_dicPlayerSfxBuff.ContainsKey(type)) //해당 타입의 사운드가 재생된적이없다면
        {
            m_dicPlayerSfxBuff.Add(type, 1); //해당 타입의 사운드 추가
        }
        else //해당 타입의 사운드가 재생된적이있다면
        {
            m_dicPlayerSfxBuff[type]++;
            if (m_dicPlayerSfxBuff[type] > 1)
            {
                m_dicPlayerSfxBuff[type] = 1;
                return; //한번 재생이되고 해당 문은 빠져나간다.
            }
        }
        StartCoroutine(Coroutine_SfxPlaySound(type, m_playerSfxArr[(int)type].length));
        m_sfxSound.PlayOneShot(m_playerSfxArr[(int)type]);
    }
    private IEnumerator Coroutine_SfxPlaySound(ePlayerSfxType type, float length)
    {
        yield return new WaitForSeconds(length);
        m_dicPlayerSfxBuff[type]--;
    }
    #endregion

    #region  적 이펙트 사운드 재생
    public void SfxEnemySound(eEnemySfxType type)
    {
        if (!m_dicEnemySfxBuff.ContainsKey(type)) //해당 타입의 사운드가 재생된적이없다면
        {
            m_dicEnemySfxBuff.Add(type, 1); //해당 타입의 사운드 추가
        }
        else //해당 타입의 사운드가 재생된적이있다면
        {
            m_dicEnemySfxBuff[type]++;
            if (m_dicEnemySfxBuff[type] > 3)
            {
                m_dicEnemySfxBuff[type] = 3;
                return; //한번 재생이되고 해당 문은 빠져나간다.
            }
        }
        StartCoroutine(Coroutine_SfxPlaySound(type, m_enemySfxArr[(int)type].length));
        m_sfxSound.PlayOneShot(m_enemySfxArr[(int)type]);
    }
    private IEnumerator Coroutine_SfxPlaySound(eEnemySfxType type, float length)
    {
        yield return new WaitForSeconds(length);
        m_dicEnemySfxBuff[type]--;
    }
    #endregion

    #region 보스 이펙트 사운드 재생
    public void SfxBossSound(eBossSfxType type)
    {
        if (!m_dicBossSfxBuff.ContainsKey(type)) //해당 타입의 사운드가 재생된적이없다면
        {
            m_dicBossSfxBuff.Add(type, 1); //해당 타입의 사운드 추가
        }
        else //해당 타입의 사운드가 재생된적이있다면
        {
            m_dicBossSfxBuff[type]++;
            if (m_dicBossSfxBuff[type] > 2)
            {
                m_dicBossSfxBuff[type] = 2;
                return; //한번 재생이되고 해당 문은 빠져나간다.
            }
        }
        StartCoroutine(Coroutine_SfxPlaySound(type, m_bossSfxArr[(int)type].length));
        m_sfxSound.PlayOneShot(m_bossSfxArr[(int)type]);
    }
    private IEnumerator Coroutine_SfxPlaySound(eBossSfxType type, float length)
    {
        yield return new WaitForSeconds(length);
        m_dicBossSfxBuff[type]--;
    }
    #endregion

    #region 아이템 사운드 재생
    public void PlayItemSound(eItemCoinSound type)
    {
        if(m_dicItemSfxBuff.ContainsKey(type))
        {
            m_dicItemSfxBuff[type]++;
            if(m_dicItemSfxBuff[type] > 2)
            {
                m_dicItemSfxBuff[type] = 2;
            }
        }
        else
        {
            m_dicItemSfxBuff.Add(type,1);
            return;
        }
        m_itemSound.PlayOneShot(m_itemArr[(int)type]);
        StartCoroutine(Coroutine_PlayItemSound(type, m_itemArr[(int)type].length));
    }
    private IEnumerator Coroutine_PlayItemSound(eItemCoinSound type,float length)
    {
        yield return new WaitForSeconds(length);
        m_dicItemSfxBuff[type]--;
    }
    #endregion
}
