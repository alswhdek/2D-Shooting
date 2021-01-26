using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum eBossType
{
    None = -1,
    OneLevel,
    TwoLevel,
    ThreeLevel,
    FourLevel,
    FiveLevel,
    Max
}
public class GokenBoss : MonoBehaviour
{  
    [Header("Hp")]
    [SerializeField]
    protected int m_hp;
    protected int m_currhp;

    protected eBossType m_type;   
    protected bool m_attackMode = false;
    protected bool m_isDie;

    [Header("속도,좌표")]
    [SerializeField]
    protected float mappley, mstartSeped, mautoSpeed;//적용 속도,등장 속도,자동 움직이는 속도
    [SerializeField]
    protected float myPosMax;

    [Header("Pool")]
    [SerializeField]
    protected EnemyBoltPool menemyBoltPool;   

    [SerializeField]
    private Transform[] mfirePos; //총알을 생성 위치

    [Header("Component")]
    protected CapsuleCollider2D m_collider;
    protected Rigidbody2D m_RB;
    protected SpriteRenderer m_sprite;

    private int m_dropExp;

    protected virtual void Awake()
    {
        m_type = eBossType.ThreeLevel;
        m_isDie = false;
        m_sprite = GetComponent<SpriteRenderer>();
        m_collider = GetComponent<CapsuleCollider2D>();
        m_RB = GetComponent<Rigidbody2D>();        
        m_currhp = m_hp;
        transform.position = new Vector3(0f, 25f, 0f); //몬스터 초기 위치
    }

    private void OnEnable()
    {                
        //controller.SetBoss(true);
        StartCoroutine(MovePatten());
        StartCoroutine(AttackPatten());
    }

    #region 자동이동 코루틴
    protected virtual IEnumerator MovePatten()
    {       
        mappley = mstartSeped;       
        Vector3 rayDir;
        float rayDistance;
        bool isMove = true;
   
        m_RB.velocity = Vector3.down * mappley;

        while(transform.position.y > myPosMax)
        {
            yield return new WaitForFixedUpdate();
        }
        m_attackMode = true;
        while (m_attackMode)
        {          
            mappley = mautoSpeed;
            if (isMove)
            {
                rayDir = Vector2.left;
                rayDistance = -3f;
                m_RB.velocity = Vector2.left * mappley;
            }
            else
            {
                rayDir = Vector2.right;
                rayDistance = 3f;
                m_RB.velocity = Vector2.right * mappley;
            }
            if(Physics2D.Raycast(new Vector3(transform.position.x + (rayDistance), transform.position.y),rayDir, m_collider.bounds.extents.x,1<<LayerMask.NameToLayer("Collider")))
            {
                isMove = !isMove;
            }          
            yield return null;
        }
    }
    #endregion

    #region 자동 공격 코루틴
    private IEnumerator AttackPatten()
    {
        WaitForSeconds attackTime = new WaitForSeconds(Random.Range(0.5f, 1f));
        WaitForFixedUpdate waiting = new WaitForFixedUpdate();
        var type = EnemyBoltPool.eEnemyBoltType.ThreeBoss;
        while(!m_attackMode)
        {
            yield return waiting;
        }

        while(m_attackMode)
        {
            yield return attackTime;
            
            for(int i=0; i< mfirePos.Length; i++)
            {
                EnemyBolt newBolt = menemyBoltPool.GetFromPool((int)EnemyBoltPool.eEnemyBoltType.ThreeBoss);
                newBolt.SetBolt((int)type, "Player", (int)type * 1);
                newBolt.transform.position = mfirePos[i].position;             
            }
            SoundController.Instance.SfxBossSound(SoundController.eBossSfxType.Boss03_Shoot);
        }
    }
    #endregion

    #region 피격
    protected virtual void Hit(int damage)
    {      
        if (transform.position.y <= myPosMax) // 일정거리 높이에있을때는 피격당하지않는다.
        {         
            if (!m_isDie)
            {
                if (m_currhp > 0)
                {
                    m_currhp -= damage;
                    Debug.LogError("Hp : " + m_currhp);
                    m_sprite.color = Color.red * new Color(1, 1, 1, 0.4f);
                    Invoke("ReStore", 0.05f);
                    HitSound();
                }
                else
                {
                    m_isDie = true;
                    StopAllCoroutines();
                    SetDie();
                }
            }
        }
    }
    private void HitSound()
    {
        var hitSoundType = SoundController.eBossSfxType.None;
        switch(m_type)
        {
            case eBossType.OneLevel:
                hitSoundType = SoundController.eBossSfxType.Boss01_Hit;
                break;
            case eBossType.TwoLevel:
                hitSoundType = SoundController.eBossSfxType.Boss02_Hit;
                break;
            case eBossType.ThreeLevel:
                hitSoundType = SoundController.eBossSfxType.Boss03_Hit;
                break;
            case eBossType.FourLevel:
                hitSoundType = SoundController.eBossSfxType.Boss04_Hit;
                break;
            case eBossType.FiveLevel:
                hitSoundType = SoundController.eBossSfxType.Boss05_Hit;
                break;
        }
        SoundController.Instance.EndLoopSound();
        SoundController.Instance.SfxBossSound(hitSoundType);
    }
    private void SetDie()
    {
        m_attackMode = false;
        switch (m_type)
        {
            case eBossType.OneLevel:
                PlayerDataManager.Instance.OpenBossCard((int)eBossType.OneLevel);
                m_dropExp = Random.Range(100, 200);
                break;
            case eBossType.TwoLevel:
                PlayerDataManager.Instance.OpenBossCard((int)eBossType.TwoLevel);
                m_dropExp = Random.Range(300, 400);
                break;
            case eBossType.ThreeLevel:
                PlayerDataManager.Instance.OpenBossCard((int)eBossType.ThreeLevel);
                m_dropExp = Random.Range(500, 600);
                break;
            case eBossType.FourLevel:
                PlayerDataManager.Instance.OpenBossCard((int)eBossType.FourLevel);
                m_dropExp = Random.Range(700, 800);
                break;
            case eBossType.FiveLevel:
                PlayerDataManager.Instance.OpenBossCard((int)eBossType.FiveLevel);
                m_dropExp = Random.Range(900, 1000);
                break;
        }
        StopAllCoroutines();
        DieSound();
        GameController.Instance.AddExp(m_dropExp);
        m_RB.velocity = Vector2.zero; //멈춘다.                            
        GameController.Instance.BossClear(); // 클리어 이펙트 노출
        m_currhp = 0;                     
        float itemDropGap = 0f;
        int itemDropCount = 10;
        for (int i = 0; i < itemDropCount; i++)
        {
            ItemManager.Instance.CreateItem(new Vector3(transform.position.x + itemDropGap, transform.position.y, transform.position.z));
            itemDropGap += 1.2f;
            if (i == 5)
            {
                itemDropGap = -1.2f;
            }
            if (i >= 6)
            {
                itemDropGap -= 1.2f;
            }
        }       
    }

    private void DieSound()
    {
        var dieSoundType = SoundController.eLoopSfxType.None;
        switch (m_type)
        {
            case eBossType.OneLevel:
                dieSoundType = SoundController.eLoopSfxType.Boss01_Die;              
                break;
            case eBossType.TwoLevel:
                dieSoundType = SoundController.eLoopSfxType.Boss02_Die;               
                break;
            case eBossType.ThreeLevel:
                dieSoundType = SoundController.eLoopSfxType.Boss03_Die;               
                break;
            case eBossType.FourLevel:
                dieSoundType = SoundController.eLoopSfxType.Boss04_Die;               
                break;
            case eBossType.FiveLevel:
                dieSoundType = SoundController.eLoopSfxType.Boss05_Die;                
                break;
        }  
        SoundController.Instance.LoopSound(dieSoundType);
    }
    protected virtual void ReStore()
    {
        m_sprite.color = new Color(1, 1, 1, 1);
    }
    #endregion
    private void Update()
    {
        if(!gameObject.activeSelf)
        {
            SoundController.Instance.EndLoopSound();
        }
    }
}
