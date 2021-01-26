using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private MonsterManager.eEnemyType m_type = MonsterManager.eEnemyType.None;
    public int m_hp;
    public int m_currHp;
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private int mlevel;

    private int m_selectIndex;   

    private Rigidbody2D mRB;   
    private PlayerController m_playerCtr;
    
    [SerializeField]
    private float mSpeed;
    [SerializeField]
    private Transform m_firePos;

    [Header("몬스터 공격 모드 좌표")]
    [SerializeField]
    private float m_attackPos;
       
    [Header("Pool")]
    [SerializeField]
    private EnemyBoltPool m_boltPool;
    [SerializeField]
    private EffectPool m_effectPool;
    [SerializeField]
    private ItemPool m_itemPool;
    [SerializeField]
    private GoldPool m_goldPool;

    private Coroutine m_coroutineMovePatten;
    private bool m_isInvinc = false;
    private PlayerController m_player;

    private int m_dropScore;
    private int m_dropExp;

    public Transform m_dummyHud;    
    // Start is called before the first frame update
    protected virtual void Awake()
    {        
        mRB = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        var obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null)
        {
            m_player = obj.GetComponent<PlayerController>();
        }
    }

    protected virtual void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");              
        if (obj != null)
        {
            m_playerCtr = obj.GetComponent<PlayerController>();

        }
    }

    private void OnEnable()
    {
        mRB.velocity = Vector2.down * mSpeed;
        m_coroutineMovePatten = StartCoroutine(MovePatten());
        StartCoroutine(AutoShoot());

    }
    public void SetEnemy(MonsterManager.eEnemyType type)
    {
        m_type = type;        
        m_hp = (int)m_type + 1;
        m_currHp = m_hp;
        m_spriteRenderer.sprite = Resources.Load<Sprite>(string.Format("Images/Enemy/Enemy_{0:00}",(int)m_type + 1));              
    }

    #region 현재 몬스터 스프라이트 상태
    public Sprite GetEnemySprite(int index)
    {
        var sprite = Resources.Load<Sprite>(string.Format("Images/Enemy/Enemy_{0:00}", index + 1));
        return sprite;
    }
    #endregion

    #region 몬스터 스프라이트 로드
    public void LoadEnemySprite()
    {
        m_spriteRenderer.sprite = GetEnemySprite(m_selectIndex);

    }
    #endregion

    #region Hierarchy 정리를위한 EnemyPool자식으로 넣어준다.
    public void SetPos(EnemyPool pool)
    {
        pool.SetParent(this);
    }
    #endregion

    #region BoltPool 접근
    public void SetBoltPool(EnemyBoltPool pool)
    {
        m_boltPool = pool;
    }
    #endregion

    #region ItemPool 접근
    public void SetItemPool(ItemPool pool)
    {
        m_itemPool = pool;
    }
    #endregion

    #region EffectPool 접근
    public void SetEffectPool(EffectPool pool)
    {
        m_effectPool = pool;
    }
    #endregion

    #region GoldPool 접근
    public void SetGoldPool(GoldPool pool)
    {
        m_goldPool = pool;
    }
    #endregion

    #region 총알 자동 생성 코루틴
    private IEnumerator AutoShoot()
    {
        WaitForFixedUpdate AttackStep = new WaitForFixedUpdate();
        WaitForSeconds PointStep;     
        while(transform.position.y > m_attackPos)
        {
            yield return AttackStep;
        }
        
        while (true)
        {
            PointStep = new WaitForSeconds(Random.Range(1f, 2f));
            yield return PointStep;
            Fire();
        }
    }
    #endregion

    #region 자동 이동 
    private IEnumerator MovePatten()
    {       
        WaitForSeconds PointStep = new WaitForSeconds(Random.Range(0.1f, 0.5f));
        WaitForSeconds PointStop = new WaitForSeconds(Random.Range(0.5f, 1f));
        
        while (true)
        {         
            yield return PointStep;
            if(m_playerCtr == null)
            {
                yield break;
            }
            if (!m_isInvinc)
            {
                if (transform.position.x < 0) //왼쪽에 있으면 오른쪽으로 이동
                {
                    mRB.velocity += Vector2.right * mSpeed;
                }
                else //오른쪽에 있으면 왼쪽으로 이동
                {
                    mRB.velocity += Vector2.left * mSpeed;
                }
                yield return PointStop;
                Vector2 StopX = mRB.velocity;
                StopX.x = 0;
                mRB.velocity = StopX;
            }
            yield return null;
        }
    }
    #endregion

    #region 총알 생성
    private void Fire()
    {
        EnemyBoltPool.eEnemyBoltType bolttype = EnemyBoltPool.eEnemyBoltType.None;
        SoundController.eEnemySfxType sfxType = SoundController.eEnemySfxType.None;
        switch (m_type)
        {
            case MonsterManager.eEnemyType.One:
                bolttype = EnemyBoltPool.eEnemyBoltType.One;
                sfxType = SoundController.eEnemySfxType.Enemy01;
                break;
            case MonsterManager.eEnemyType.Two:
                bolttype = EnemyBoltPool.eEnemyBoltType.Two;
                sfxType = SoundController.eEnemySfxType.Enemy02;
                break;
            case MonsterManager.eEnemyType.Three:
                bolttype = EnemyBoltPool.eEnemyBoltType.Three;
                sfxType = SoundController.eEnemySfxType.Enemy03;
                break;
            case MonsterManager.eEnemyType.Four:
                bolttype = EnemyBoltPool.eEnemyBoltType.Four;
                sfxType = SoundController.eEnemySfxType.Enemy04;
                break;
            case MonsterManager.eEnemyType.Five:
                bolttype = EnemyBoltPool.eEnemyBoltType.Five;
                sfxType = SoundController.eEnemySfxType.Enemy05;
                break;
            case MonsterManager.eEnemyType.Six:
                bolttype = EnemyBoltPool.eEnemyBoltType.Six;
                sfxType = SoundController.eEnemySfxType.Enemy06;
                break;
            case MonsterManager.eEnemyType.Seven:
                bolttype = EnemyBoltPool.eEnemyBoltType.Seven;
                sfxType = SoundController.eEnemySfxType.Enemy07;
                break;
            case MonsterManager.eEnemyType.Eight:
                bolttype = EnemyBoltPool.eEnemyBoltType.Eight;
                sfxType = SoundController.eEnemySfxType.Enemy08;
                break;
            case MonsterManager.eEnemyType.Nine:
                bolttype = EnemyBoltPool.eEnemyBoltType.Nine;
                sfxType = SoundController.eEnemySfxType.Enemy09;
                break;
            case MonsterManager.eEnemyType.Ten:
                bolttype = EnemyBoltPool.eEnemyBoltType.Ten;
                sfxType = SoundController.eEnemySfxType.Enemy010;
                break;
        }
        if (transform.position.y <= m_attackPos)
        {
            
            EnemyBolt newBolt = m_boltPool.GetFromPool((int)bolttype);
            m_boltPool.SetParentPos(newBolt);
            newBolt.SetBolt((int)bolttype+1, "Player", (int)bolttype+1 * 5+5);         
            newBolt.transform.position = m_firePos.position;            
            SoundController.Instance.SfxEnemySound(sfxType); //사운드                                 
        }
    }
    #endregion

    #region 피격
    public void Hit(int damage)
    {
        if (transform.position.y < m_attackPos)
        {
            if (m_currHp > 0)
            {
                m_currHp -= damage;              
                m_spriteRenderer.color = Color.red * new Color(1, 1, 1, 0.4f);
                Invoke("ReStore", 0.2f);
                Debug.Log(m_currHp);
                SoundController.Instance.SfxEnemySound(SoundController.eEnemySfxType.EnemyHit);
            }
            else
            {
                m_currHp = 0;
                SetDie();
            }
        }
    }
    protected virtual void ReStore()
    {
        m_spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    #endregion

    #region 몬스터 사망 처리
    public void SetDie()
    {       
        EffectPool.eEffectType effectType = EffectPool.eEffectType.None;
        SoundController.eEnemySfxType dieSfxType = SoundController.eEnemySfxType.None;       
        switch (m_type)
        {
            case MonsterManager.eEnemyType.One: //1 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0102;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie01_02;
                m_dropScore = Random.Range(10, 20);
                m_dropExp = Random.Range(5, 10);
                break;
            case MonsterManager.eEnemyType.Two: //2 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0304;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie01_02;
                m_dropScore = Random.Range(30, 40);
                m_dropExp = Random.Range(15, 20);
                break;
            case MonsterManager.eEnemyType.Three: //3 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0506;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie03_04;
                m_dropScore = Random.Range(50, 60);
                m_dropExp = Random.Range(25, 30);
                break;
            case MonsterManager.eEnemyType.Four: //4 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0708;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie03_04;
                m_dropScore = Random.Range(70, 80);
                m_dropExp = Random.Range(35, 40);
                break;
            case MonsterManager.eEnemyType.Five: //5 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0910;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie05_06;
                m_dropScore = Random.Range(90, 100);
                m_dropExp = Random.Range(45, 50);
                break;
            case MonsterManager.eEnemyType.Six: //5 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0708;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie05_06;
                m_dropScore = Random.Range(90, 100);
                m_dropExp = Random.Range(45, 50);
                break;
            case MonsterManager.eEnemyType.Seven: //5 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0506;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie03_04;
                m_dropScore = Random.Range(90, 100);
                m_dropExp = Random.Range(45, 50);
                break;
            case MonsterManager.eEnemyType.Eight: //5 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0708;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie05_06;
                m_dropScore = Random.Range(90, 100);
                m_dropExp = Random.Range(45, 50);
                break;
            case MonsterManager.eEnemyType.Nine: //5 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0708;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie03_04;
                m_dropScore = Random.Range(90, 100);
                m_dropExp = Random.Range(45, 50);
                break;
            case MonsterManager.eEnemyType.Ten: //5 레벨 몬스터 
                effectType = EffectPool.eEffectType.Enemy_0708;
                dieSfxType = SoundController.eEnemySfxType.EnemyDie03_04;
                m_dropScore = Random.Range(90, 100);
                m_dropExp = Random.Range(45, 50);
                break;

        }
        Timer newEffect = m_effectPool.GetFromPool((int)effectType);
        m_effectPool.SetParent(newEffect);
        newEffect.transform.position = transform.position;      
        SoundController.Instance.SfxEnemySound(dieSfxType); //사운드
        ItemManager.Instance.CreateItem(transform.position,this);
        GameController.Instance.AddScore(m_dropScore);
        m_player.AddExp(m_dropExp);
        m_playerCtr.AddExp((int)m_type + 10); //경험치 추가      
        gameObject.SetActive(false);       
    }
    #endregion

    #region 충돌
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("Hit",(int)m_type+1);                    
        }       
    }
    #endregion
}
