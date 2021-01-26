using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    public Status m_status;
    public int m_currentExp = 0;
    private int m_currentHp;

    private SpriteRenderer m_spriteRenderer;

    private Rigidbody2D mRB;    
    
    private float m_currentDamage;
  
    [Header("총알 관련 객체,변수")]
    [SerializeField]
    private Transform m_boltPos;
    private bool m_isFire = true;  
      
    [Header("GoldObject")]
    private int m_currGold;

    [Header("ShootTime")]
    [SerializeField]
    private float mShootTime;
    private float mCurrentShootTime;
    
    [Header("Pool")]
    [SerializeField]
    private BoltPool m_boltPool;
    [SerializeField]
    private EffectPool mEffectPool;
    [SerializeField]
    private BombPool mBombPool;

    [Header("PlayerValue")]
    [SerializeField]
    private float mTiltValue;   
    private Vector3 m_dir;
    private bool m_isDrag = false;

    [Header("Player_Ray_Value")]
    [SerializeField]
    private GameObject m_selectObj;
    private Vector3 m_startPos, m_endPos;
    RaycastHit2D m_hit;
    Ray m_ray;

    [Header("MinMax Value")]
    [SerializeField]
    private float mMinXpos, mMaxXpos, mMinYpos, mMaxYpos;

    [SerializeField]
    private bool misDie;
    [SerializeField]
    private GameObject m_levelUpEffect;

    //아이템 관련 변수
    [Header("그림자 모드")]
    [SerializeField]
    private Transform[] m_shadowPos;
    [SerializeField]    
    private PlayerShadow[] m_shadow;
    private bool m_isShadow;

    [Header("무적 모드 아이템")]
    [SerializeField]
    private GameObject m_invincibilityObj;
    private Invincibility[] m_invincibility;
    [SerializeField]
    private GameObject[] m_booster;
    private bool m_isInvin = false;

    [Header("자석 모드 아이템")]
    [SerializeField]
    private GameObject m_magnet;
    private bool m_isMagnet;
    #region Unity
    // Start is called before the first frame update
    protected override void OnAwake()
    {        
        mRB = GetComponent<Rigidbody2D>();      
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        LoadPlayer();

        m_levelUpEffect.gameObject.SetActive(false);

        mCurrentShootTime = 0;     
        transform.position = new Vector3(0, -1.5f);     
    }
    protected override void OnStart()
    {
        InitStatus();
        UIController.Instance.ShowHp(m_currentHp, m_status.m_hp);
        UIController.Instance.ShowExp(m_currentExp,m_status.m_exp);
        //그림자 모드 비활성화
         
        for (int i=0; i<m_shadow.Length; i++)
        {            
            m_shadow[i].gameObject.SetActive(false);
        }
        //무적 모드 비활성화     
        m_invincibility = m_invincibilityObj.GetComponentsInChildren<Invincibility>();
        for (int i = 0; i < m_invincibility.Length; i++)
        {
            m_invincibility[i].gameObject.SetActive(false);
        }
        //부스터 오브젝트 비활성화
        for(int i=0; i<m_booster.Length; i++)
        {
            m_booster[i].gameObject.SetActive(false);
        }
        //자석 모드 비활성화
        m_magnet.gameObject.SetActive(false);
    }
    void Update()
    {
        if (!m_isDrag)
        {
            Move(); //이동
        }
        if(Input.GetMouseButtonDown(0))
        {
            m_isDrag = true;
            var touchObj = GetTouchMove();
            if(touchObj != null)
            {
                Debug.LogError("PlayerObject");                
                m_selectObj = touchObj;
                m_startPos = Input.mousePosition;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_isDrag = false;
            m_dir = Vector3.zero;
            m_startPos = m_endPos = Vector3.zero;           
            m_selectObj = null;
        }
        if (m_isDrag)
        {
            m_endPos = Input.mousePosition;
            m_dir = m_endPos - m_startPos;
            m_dir.y = 0f;
            m_dir.z = 0f;
            if(GetLayer() && m_selectObj != null)
            {
                transform.position += m_dir.normalized * m_status.m_moveSpeed * Time.deltaTime;
            }
            m_endPos = m_startPos;
        }        
        //기본 총알 발싸 기능
        if (mCurrentShootTime <= 0 && m_isFire)
        {
            mCurrentShootTime = mShootTime; //CollTime 을 다시 mShootTimer값으로 초기화
            Fire(); //총알 발싸            
        }
        mCurrentShootTime -= Time.deltaTime;                              
    }
    #endregion

    #region 플레이어 상태 구조체 프로퍼티
    public Status GetStatus { get { return m_status; } set { m_status = value; } }
    public int Defence { get { return m_status.m_defence; } set { m_status.m_defence = value; } }
    public int Attack { get { return m_status.m_attack; } set { m_status.m_attack = value; } }
    public int Speed { get { return (int)m_status.m_moveSpeed; } set { m_status.m_moveSpeed = value; } }
    public int Health { get { return m_status.m_hp; } set { m_status.m_hp = value; } }
    #endregion

    #region 플레이어 상태 초기화
    private void InitStatus()
    {
        m_status = new Status(1, 1000, PlayerDataManager.Instance.GetHp(PlayerDataManager.Instance.GetPlayerIndex()), PlayerDataManager.Instance.GetAttack(PlayerDataManager.Instance.GetPlayerIndex()),
            PlayerDataManager.Instance.GetSpeed(PlayerDataManager.Instance.GetPlayerIndex()), PlayerDataManager.Instance.GetDefence(PlayerDataManager.Instance.GetPlayerIndex()));        
        m_currentHp = m_status.m_hp;
    }
    #endregion

    #region 플레이어 Load
    private Sprite GetChaecterSprite(int index)
    {
        var spr = Resources.Load<Sprite>(string.Format("Images/Player/Player_{0:00}", index + 1));
        return spr;
    }
    public void LoadPlayer()
    {
        m_spriteRenderer.sprite = GetChaecterSprite(PlayerDataManager.Instance.GetPlayerIndex());
        if(PlayerDataManager.Instance.GetPlayerIndex() == 1)
        {
            m_spriteRenderer.flipY = true;
        }
    } 
    #endregion

    #region Player Layer 검출
    private bool GetLayer()
    {
        Vector3 dir = Vector3.zero;
        //float value = 0f;
        if(m_dir == Vector3.zero)
        {
            dir = Vector3.zero;
            //value = 0f;
        }
        if(m_dir.x < 0f)//왼쪽
        {
            dir = Vector3.left;
            //value = 0f;
        }
        if(m_dir.x > 0f)
        {
            dir = Vector3.right;
            //value = 1.2f;
        }
        if(Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y),dir,1.2f,1<<LayerMask.NameToLayer("Collider")))
        {
            return false;
        }
        Debug.DrawRay(new Vector3(transform.position.x,transform.position.y,0f), dir, Color.yellow);
        return true;
    }
    #endregion

    #region Player Touch Move
    private GameObject GetTouchMove()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Physics2D.Raycast(mousePos, Vector2.zero,10f,1<<LayerMask.NameToLayer("Player")))
        {
            return gameObject;
        }
        return null;
    }
    #endregion

    #region Player Move
    public void Move()
    {
        m_dir.Set(Input.GetAxisRaw("Horizontal"),0f,0f);

        transform.position += m_dir.normalized * m_status.m_moveSpeed * Time.deltaTime;
       
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, mMinXpos, mMaxXpos), Mathf.Clamp(transform.position.y, mMinYpos, mMaxYpos)); //맵 이탈을위한 x,y축에 경계선을 정해준다.
        if(m_dir.x < 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 5f);
        }
        if(m_dir.x > 0f)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, -5f);
        }
        if(m_dir == Vector3.zero)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }       
    }
    #endregion
 
    #region Player Fire
    private void Fire()
    {       
        var Index = PlayerDataManager.Instance.GetPlayerIndex();
        BoltPool.eBoltType boltType = BoltPool.eBoltType.None;
        SoundController.ePlayerSfxType soundType = SoundController.ePlayerSfxType.None;
        switch(Index)
        {
            case (int)PlayerDataManager.PlayerType.OneCharcter:
                if (m_status.m_level == 1)
                {
                    boltType = BoltPool.eBoltType.Player01_01;
                    soundType = SoundController.ePlayerSfxType.Player01_01;
                }
                else if (m_status.m_level == 2)
                {
                    boltType = BoltPool.eBoltType.Player01_02;
                    soundType = SoundController.ePlayerSfxType.Player01_02;
                }
                else if (m_status.m_level == 3)
                {
                    boltType = BoltPool.eBoltType.Player01_03;
                    soundType = SoundController.ePlayerSfxType.Player01_03;
                }
                else if (m_status.m_level == 4)
                {
                    boltType = BoltPool.eBoltType.Player01_04;
                    soundType = SoundController.ePlayerSfxType.Player01_03;
                }
                else if (m_status.m_level == 5)
                {
                    boltType = BoltPool.eBoltType.Player01_05;
                    soundType = SoundController.ePlayerSfxType.Player01_03;
                }
                break;
            case (int)PlayerDataManager.PlayerType.TwoCharcter:
                if (m_status.m_level == 1)
                {
                    boltType = BoltPool.eBoltType.Player02_01;
                    soundType = SoundController.ePlayerSfxType.Player02_01;
                }
                else if (m_status.m_level == 2)
                {
                    boltType = BoltPool.eBoltType.Player02_02;
                    soundType = SoundController.ePlayerSfxType.Player02_02;
                }
                else if (m_status.m_level == 3)
                {
                    boltType = BoltPool.eBoltType.Player02_03;
                    soundType = SoundController.ePlayerSfxType.Player02_03;
                }
                else if (m_status.m_level == 4)
                {
                    boltType = BoltPool.eBoltType.Player02_04;
                    soundType = SoundController.ePlayerSfxType.Player02_03;
                }
                else if (m_status.m_level == 5)
                {
                    boltType = BoltPool.eBoltType.Player02_05;
                    soundType = SoundController.ePlayerSfxType.Player02_03;
                }
                break;
            case (int)PlayerDataManager.PlayerType.ThreeCharcter:
                if (m_status.m_level == 1)
                {
                    boltType = BoltPool.eBoltType.Player03_01;
                    soundType = SoundController.ePlayerSfxType.Player03_01;
                }
                else if (m_status.m_level == 2)
                {
                    boltType = BoltPool.eBoltType.Player03_02;
                    soundType = SoundController.ePlayerSfxType.Player03_02;
                }
                else if (m_status.m_level == 3)
                {
                    boltType = BoltPool.eBoltType.Player03_03;
                    soundType = SoundController.ePlayerSfxType.Player03_03;
                }
                else if (m_status.m_level == 4)
                {
                    boltType = BoltPool.eBoltType.Player03_04;
                    soundType = SoundController.ePlayerSfxType.Player03_03;
                }
                else if (m_status.m_level == 5)
                {
                    boltType = BoltPool.eBoltType.Player03_05;
                    soundType = SoundController.ePlayerSfxType.Player03_03;
                }
                break;           
        }        
        Bolt newBolt = m_boltPool.GetFromPool((int)boltType);
        newBolt.SetBolt(boltType, m_status.m_level + 1f * 20f);
        m_boltPool.SetParent(newBolt);       
        newBolt.transform.position = m_boltPos.position;     
        newBolt.Target("Enemy");
        SoundController.Instance.SfxPlaySound(soundType);//사운드
        if (m_isShadow)
        {
            for (int i = 0; i < m_shadowPos.Length; i++)
            {
                newBolt = m_boltPool.GetFromPool((int)boltType);
                newBolt.SetBolt(boltType, m_status.m_level + 1f * 20f);
                m_boltPool.SetParent(newBolt);
                newBolt.Target("Enemy");
                newBolt.transform.position = m_shadowPos[i].transform.position;
            }
        }
    }

    public bool GetFire { get { return m_isFire; } set { m_isFire = value; } }
    #endregion

    #region 현재 레벨을 반환
    public int GetLevel { get { return m_status.m_level; } }
    #endregion

    #region HP
    public int CurrentHp { get { return m_currentHp; } set { m_currentHp = value; } }
    public int GetCurrentHp()
    {
        return m_status.m_hp;
    } 
    #endregion

    #region Player Exp
    public void AddExp(int value)
    {        
        if (value < 0) return;
        if(m_currentExp < m_status.m_exp)
        {
            m_currentExp += value;
            UIController.Instance.ShowExp(m_currentExp, m_status.m_exp);
            if(m_currentExp >= m_status.m_exp)
            {
                if(m_status.m_level < 5)
                {
                    m_isInvin = true;
                    m_status.m_level++;
                    UIController.Instance.ShowLevel(m_status.m_level);
                    if(value > m_currentExp)
                    {
                        m_currentExp = value - m_currentExp;
                    }
                    else
                    {
                        m_currentExp = 0;
                    }
                    m_status.m_exp += 2000;
                    m_status.m_hp += 100;
                    m_currentHp = m_status.m_hp;
                    UIController.Instance.ShowExp(m_currentExp, m_status.m_exp);
                    UIController.Instance.ShowHp(m_currentHp, m_status.m_hp);
                    m_levelUpEffect.gameObject.SetActive(true);
                    Invoke("EndLevelUpEffect", 5f);
                }

            }
        }
    }
    private void EndLevelUpEffect()
    {
        m_levelUpEffect.gameObject.SetActive(false);
        m_isInvin = false;
    }
    public int GetCurrentExp()
    {
        return m_currentExp;
    }
    public int GetMaxExp()
    {
        return m_status.m_exp;
    }
    #endregion

    #region Player Gold
    public void AddGold(int gold)
    {
        if (gold >= 0)
        {
            m_currGold += gold;
            Debug.Log("현재 획득 골드는 : " + m_currGold);
        }
    }
    #endregion

    #region Player Hit
    public void Hit(int damage)
    {             
        if (damage < 0 || m_isInvin) return;
        if(m_currentHp > 0)
        {           
            Color hitColor = Color.green;
            m_spriteRenderer.color = hitColor;
            Invoke("ResetColor", 0.2f);
            m_currentHp -= damage;
            UIController.Instance.ShowHp(m_currentHp, m_status.m_hp);
            Debug.Log("Hp : " + m_currentHp);            
        }
        else
        {
            SetDie();
            m_currentHp = 0;          
        }       
        //SoundController.Instance.SfxPlaySound(SoundController.ePlayerSfxType.Die);     
        //GameController.Instance.ReStart();
    }
    private void ResetColor()
    {
        m_spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
    private void SetDie()
    {
        GameController.Instance.GameEnd();
        var effectType = EffectPool.eEffectType.player;
        Timer newEffect = mEffectPool.GetFromPool((int)effectType); //임시 이펙트
        newEffect.transform.position = transform.position;
        var soundType = SoundController.ePlayerSfxType.PlayerDie;
        SoundController.Instance.SfxPlaySound(soundType);       
        gameObject.SetActive(false);
    }
    public bool IsAlive()
    {
        return m_currentHp < 0;
    }
    #endregion

    
    //아이템 효과
    #region 그림자 아이템
    public void SetShadow()
    {
        m_isShadow = true;
        for (int i=0; i<m_shadow.Length; i++)
        {
            m_shadow[i].gameObject.SetActive(true);
        }     
    }
    public void EndShadow()
    {
        m_isShadow = false;
        for (int i = 0; i < m_shadow.Length; i++)
        {
            m_shadow[i].gameObject.SetActive(false);
        }
    }
    #endregion

    #region 무적 아이템
    public void Setinvincibility()
    {
        m_invincibility[0].gameObject.SetActive(true);
        for(int i=0; i< m_booster.Length; i++)
        {
            if(m_isShadow)
            {
                m_booster[i].gameObject.SetActive(true);
            }
            else
            {
                m_booster[0].gameObject.SetActive(true);
                break;
            }
        }
    }
    public void Endinvincibility()
    {
        m_invincibility[0].gameObject.SetActive(false);
        m_invincibility[1].gameObject.SetActive(true);
        m_invincibility[1].SetSpeed();
        for (int i = 0; i < m_booster.Length; i++)
        {
            if (m_isShadow)
            {
                m_booster[i].gameObject.SetActive(false);
            }
            else
            {
                m_booster[0].gameObject.SetActive(false);
                break;
            }
        }
    }
    #endregion

    #region 자석 아이템
    public bool IsMagnet { get { return m_isMagnet; }set { m_isMagnet = value; } }
    public void SetMarent()
    {
        m_isMagnet = true;
        m_magnet.gameObject.SetActive(true);
    }
    public void EndMarent()
    {
        m_isMagnet = false;
        m_magnet.gameObject.SetActive(false);
    }
    #endregion

    #region 속도 아이템
    public void SetSpeed(float speed)
    {
        float maxSpeed = 20f;
        for (int i = 0; i < m_booster.Length; i++)
        {
            if (m_isShadow)
            {
                m_booster[i].gameObject.SetActive(true);
            }
            else
            {
                m_booster[0].gameObject.SetActive(true);
                break;
            }
        }
        if (m_status.m_moveSpeed < maxSpeed)
        {
            m_status.m_moveSpeed = m_status.m_moveSpeed * speed;
        }
        else
            return;
    }
    public void EndSpeed(float speed)
    {
        for (int i = 0; i < m_booster.Length; i++)
        {
            if (m_isShadow)
            {
                m_booster[i].gameObject.SetActive(false);
            }
            else
            {
                m_booster[0].gameObject.SetActive(false);
                break;
            }
        }
        m_status.m_moveSpeed = m_status.m_moveSpeed / speed;
    }
    #endregion

    #region HP 아이템
    public void SetHp()
    {
        int resetHp = m_status.m_hp;
        m_currentHp = resetHp;
        UIController.Instance.ShowHp(m_currentHp, m_status.m_hp);
    }
    #endregion

    #region 공격 증가 아이템
    public void SetAttack(int attack)
    {
        int maxAttack = 6;
        if(m_status.m_attack < maxAttack)
        {
            m_status.m_attack += attack;
        }
    }
    public void endAttack(int attack)
    {
        m_status.m_attack = m_status.m_attack / attack;
    }
    #endregion

}
