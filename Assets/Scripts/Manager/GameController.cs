using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : SingletonMonoBehaviour<GameController>
{

    
    [SerializeField]
    private TweenAlpha m_tweenAlpha;
   
    [SerializeField]
    private Boss m_boss01;
    [SerializeField]
    private TankBoss m_boss02;
    [SerializeField]
    private GokenBoss m_boss03;
    [SerializeField]
    private Dragon m_boss04;
    [SerializeField]
    private Boss05 m_boss05;

    private bool m_bosscomback = false;    

    [SerializeField]
    private BgController m_bgcontroller;

    private Coroutine m_createEnemy;
    private Coroutine m_gameTime;

    private float m_timer;
    private float m_bossTime = 5f;

    [SerializeField]  
    private GameObject[] m_bossEffect;

    private eBossType m_bossType = eBossType.None;

    //플레이어 관련 변수
    private PlayerController m_player;

    //UI 관련 맴버 변수들
    private int m_gold = 0;
    private int m_stage = 1;
    private int m_score = 0;

    //각 스테이지 몬스터 생성 시간
    float m_minValue, m_maxValue;

    protected override void OnAwake()
    {
        m_tweenAlpha.enabled = false;
        //m_tweenAlpha.gameObject.SetActive(false);
        m_boss01.gameObject.SetActive(false);
        m_boss02.gameObject.SetActive(false);
        m_boss03.gameObject.SetActive(false);
        m_boss04.gameObject.SetActive(false);
        m_boss05.gameObject.SetActive(false);

        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();      
    }
    protected override void OnStart()
    {      
        m_createEnemy = StartCoroutine(Coroutine_CreateEnemy());
        m_gameTime = StartCoroutine(Coroutine_GameTime());
        for (int i = 0; i < m_bossEffect.Length; i++)
        {
            m_bossEffect[i].gameObject.SetActive(false);
        }
        InitState();      
    }
    public void InitState()
    {                
        var maxExp = m_player.GetMaxExp();
        UIController.Instance.ShowScore(m_score);
        UIController.Instance.ShowStage(m_stage);
        UIController.Instance.ShowGold(m_gold);
        UIController.Instance.ShowHp(m_player.GetCurrentHp(), m_player.GetStatus.m_hp);
        UIController.Instance.ShowLevel(1);        
    }
    public int GetStage()
    {
        return m_stage;
    }
    public void NextStage()
    {
        m_tweenAlpha.from = 1;
        m_tweenAlpha.to = 0;
        m_tweenAlpha.duration = 5f;
        m_tweenAlpha.gameObject.SetActive(true);        
        m_tweenAlpha.enabled = true;                         
    }
    public void AddExp(int exp)
    {
        m_player.AddExp(exp);
    }
    public void LoadStage()
    {
        if (m_stage < 5)
        {
            m_stage++;
        }
        else
        {
            GameEnd();
            return;
        }
        m_player.IsMagnet = false;
        UIController.Instance.ShowStage(m_stage);        
        m_bossTime += 5f;
        m_bgcontroller.SetBgSprite(m_stage);
        m_gameTime = StartCoroutine(Coroutine_GameTime());//다시 보스 생성시간 체크
        m_createEnemy = StartCoroutine(Coroutine_CreateEnemy());//다시 몬스터 생성        
    }
    #region Stage Bgm 사운드
    public void StageBgmSound()
    {
        var type = SoundController.eBGMType.None;
        switch(m_stage)
        {
            case 1:
                type = SoundController.eBGMType.Stage1;
                break;
            case 2:
                type = SoundController.eBGMType.Stage2;
                break;
            case 3:
                type = SoundController.eBGMType.Stage3;
                break;
            case 4:
                type = SoundController.eBGMType.Stage4;
                break;
            case 5:
                type = SoundController.eBGMType.Stage5;
                break;
        }
        SoundController.Instance.PlayBackgroundSound(type);
    }
    #endregion

    #region 보스 활성화
    public void OpenBoss()
    {
        switch (m_stage)
        {
            case 1:
                m_bossType = eBossType.OneLevel;
                m_boss01.gameObject.SetActive(true);
                SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Stage1Boss);
                break;
            case 2:
                m_bossType = eBossType.TwoLevel;
                m_boss02.gameObject.SetActive(true);
                SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Stage2Boss);
                break;
            case 3:
                m_bossType = eBossType.ThreeLevel;
                m_boss03.gameObject.SetActive(true);
                SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Stage3Boss);
                break;
            case 4:
                m_bossType = eBossType.FourLevel;
                m_boss04.gameObject.SetActive(true);              
                SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Stage4Boss);
                break;
            case 5:
                m_bossType = eBossType.FiveLevel;
                m_boss05.gameObject.SetActive(true);                            
                SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Stage5Boss);
                break;       
        }
    }
    #endregion

    #region 단계별 보스 클리어 이펙트
    public void BossClear()
    {        
        switch (m_bossType)
        {
            case eBossType.OneLevel:
                m_bossEffect[0].gameObject.SetActive(true);
                m_bossEffect[0].transform.position = m_boss01.transform.position;              
                break;
            case eBossType.TwoLevel:
                m_bossEffect[1].gameObject.SetActive(true);
                m_bossEffect[1].transform.position = m_boss02.transform.position;
                
                break;
            case eBossType.ThreeLevel:
                m_bossEffect[2].gameObject.SetActive(true);
                m_bossEffect[2].transform.position = m_boss03.transform.position;
                
                break;
            case eBossType.FourLevel:
                m_bossEffect[3].gameObject.SetActive(true);
                m_bossEffect[3].transform.position = m_boss04.transform.position;
                
                break;
            case eBossType.FiveLevel:
                m_bossEffect[4].gameObject.SetActive(true);
                m_bossEffect[4].transform.position = m_boss05.transform.position;
                
                break;
        }
        m_player.IsMagnet = true;
        m_player.GetFire = false;
        StartCoroutine(Coroutine_DieEffectEnd());
    }
    IEnumerator Coroutine_DieEffectEnd()
    {
        float timer = 0f;
        float effectEndTime = 5f;      
        while(!m_player.IsAlive())
        {
            timer += Time.deltaTime;
            if(timer >= effectEndTime)
            {                
                for (int i = 0; i < m_bossEffect.Length; i++)
                {
                    if (m_bossEffect[i].gameObject.activeSelf)
                    {
                        m_bossEffect[i].gameObject.SetActive(false);
                        break;
                    }
                }
                m_player.GetFire = true;
                switch (m_bossType)
                {
                    case eBossType.OneLevel:
                        m_boss01.gameObject.SetActive(false);                       
                        break;
                    case eBossType.TwoLevel:
                        m_boss02.gameObject.SetActive(false);                      
                        break;
                    case eBossType.ThreeLevel:
                        m_boss03.gameObject.SetActive(false);                       
                        break;
                    case eBossType.FourLevel:
                        m_boss04.gameObject.SetActive(false);                       
                        break;
                    case eBossType.FiveLevel:
                        m_boss05.gameObject.SetActive(false);                       
                        break;
                }
                SoundController.Instance.EndLoopSound();
                StageController.Instance.OnStageProgress();
                //NextStage();
                yield break;
            }           
            yield return null;
        }            
    }
    #endregion

    #region 적 비행기 생성
    private IEnumerator Coroutine_CreateEnemy()
    {        
        MonsterManager.eEnemyType type = MonsterManager.eEnemyType.None;        
        float result;

        StageBgmSound();
        
        while (true)
        {          
            switch (m_stage)
            {              
                case 1:
                    type = (MonsterManager.eEnemyType)Random.Range((int)MonsterManager.eEnemyType.One, (int)MonsterManager.eEnemyType.Two+1);
                    m_minValue = 0.5f;
                    m_maxValue = 1f;
                    break;
                case 2:
                    type = (MonsterManager.eEnemyType)Random.Range((int)MonsterManager.eEnemyType.Three, (int)MonsterManager.eEnemyType.Four+1);
                    m_minValue = 0.4f;
                    m_maxValue = 0.9f;                  
                    break;
                case 3:
                    type = (MonsterManager.eEnemyType)Random.Range((int)MonsterManager.eEnemyType.Five, (int)MonsterManager.eEnemyType.Six+1);
                    m_minValue = 0.3f;
                    m_maxValue = 0.8f;                   
                    break;
                case 4:
                    type = (MonsterManager.eEnemyType)Random.Range((int)MonsterManager.eEnemyType.Seven, (int)MonsterManager.eEnemyType.Eight+1);
                    m_minValue = 0.2f;
                    m_maxValue = 0.7f;                   
                    break;
                case 5:
                    type = (MonsterManager.eEnemyType)Random.Range((int)MonsterManager.eEnemyType.Nine, (int)MonsterManager.eEnemyType.Max);
                    m_minValue = 0.1f;
                    m_maxValue = 0.6f;                   
                    break;
            }
            result = Random.Range(m_minValue, m_maxValue);
            yield return new WaitForSeconds(result);
            MonsterManager.Instance.CreateEnemy(type);
        }
    }
    #endregion

    #region 스테이지별 시간
    private IEnumerator Coroutine_GameTime()
    {       
        while(true)
        {
            m_timer += Time.deltaTime;
            if(m_timer >= m_bossTime)
            {               
                OpenBoss();                
                m_timer = 0f;
                StopAllCoroutines();
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region 골드 획득
    public void AddGold(int gold)
    {
        m_gold += gold;
        UIController.Instance.ShowGold(m_gold);
    }
    #endregion

    #region 경험치 획득
    public void AddScore(int score)
    {
        m_score += score;
        UIController.Instance.ShowScore(m_score);
    }
    #endregion

    #region GameEnd
    public void GameEnd()
    {
        ResultManager.Instance.SetResult(m_score, m_gold);
        PlayerDataManager.Instance.IncreaseGold(m_gold);        
    }
    #endregion
}








