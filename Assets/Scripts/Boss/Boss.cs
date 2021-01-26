using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : GokenBoss
{
    [SerializeField]
    private float mSpeed,mattackMoveSpeed;

    [SerializeField]
    private GameObject m_firePosObj;
    private Transform[] m_firepos;
    
    /*[Header("Min/Max")]
    [SerializeField]
    private float mYMaxPos,mXMinPos, mXMaxPos;*/

    /*[Header("BossFirePos")]
    [SerializeField]
    private Transform mFirePos;*/

    [Header("Pool")]
    [SerializeField]
    private EnemyBoltPool mEnemyBoltPool;
    [SerializeField]
    private EffectPool mEffectPool;
   
    //private SpriteRenderer mSprite;  
    // Start is called before the first frame update
    protected override void Awake()
    {
        m_type = eBossType.OneLevel;
        m_collider = GetComponent<CapsuleCollider2D>();
        m_RB = GetComponent<Rigidbody2D>();
        m_sprite = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {       
        m_firepos = m_firePosObj.GetComponentsInChildren<Transform>();
    }
    private void OnEnable()
    {
        transform.position = new Vector3(0, 20f, 0);       
        m_currhp = m_hp;
        StartCoroutine(MovePatten());
        StartCoroutine(AutoFire());
        StartCoroutine(Coroutine_AutoFireShoot());
    }

    #region 자동이동 코루틴
    protected override IEnumerator MovePatten()
    {
        return base.MovePatten();
    }
    #endregion

    #region 자동 공격(Nomal Bolt)
    private IEnumerator AutoFire()
    {
        WaitForSeconds Step = new WaitForSeconds(Random.Range(0.5f, 1f));
        WaitForFixedUpdate Waiting = new WaitForFixedUpdate();
        var type = EnemyBoltPool.eEnemyBoltType.OneBoss;
        var damage = (int)type * 2;
        while(!m_attackMode)
        {
            yield return Waiting;
        }

        while(m_attackMode) //Player가 사망하지 않았을 경우
        {
            yield return Step;
            for(int i=0; i<m_firepos.Length-1; i++)
            {
                EnemyBolt newBolt = mEnemyBoltPool.GetFromPool((int)type);
                newBolt.transform.position = m_firepos[i].position;
                newBolt.SetBolt(damage, "Player", (int)type * 2);                
            }
            SoundController.Instance.SfxBossSound(SoundController.eBossSfxType.Boss01_Shoot01);
        }
    }
    #endregion

    #region 자동 공격(Skill Bolt)

    IEnumerator Coroutine_AutoFireShoot()
    {
        WaitForSeconds createTime = new WaitForSeconds(Random.Range(1f, 5f));
        var type = EnemyBoltPool.eEnemyBoltType.OneBossSkill;
        while (!m_attackMode)
        {
            yield return new WaitForFixedUpdate();
        }
        while (m_attackMode)
        {
            yield return createTime;
            EnemyBolt newBolt = mEnemyBoltPool.GetFromPool((int)type);
            newBolt.transform.position = m_firepos[0].position; // 가운데 Pos에서 총알 생성           
            newBolt.SetBolt((int)type * 2, "Player", (int)type*2);
            SoundController.Instance.SfxBossSound(SoundController.eBossSfxType.Boss01_Shoot02);
        }
    }
    #endregion

}
