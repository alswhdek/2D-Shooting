using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TankBoss : GokenBoss
{
    [SerializeField]
    private Transform m_tankShootEq;
    [SerializeField]
    private float myPos;

    [SerializeField]
    private float meffectTime;

    [Header("Pool")]
    [SerializeField]
    private EnemyBoltPool m_boltPool;

    [Header("속도,좌표")]
    [SerializeField]
    private float mSpeed , mMoveSpeed;
    [SerializeField]
    private float mleftXpos, mrightXpos;

   
    [SerializeField]
    private TankRotate mTankRotate;
    [SerializeField]
    private TankBossHit m_tankHit;
    
    [Header("총알 생성 위치")]
    [SerializeField]
    private Transform[] mFirePos;
    [SerializeField]
    private Transform mRotatePos;

    private Coroutine mstartEffect;

    private SoundController mSoundController;

    //private eBossType m_type;
    //private Rigidbody2D m_RB;

    // Start is called before the first frame update
    protected override void Awake()
    {
        m_type = eBossType.TwoLevel;
        m_RB = GetComponent<Rigidbody2D>();              
        mTankRotate.gameObject.SetActive(false);
        m_attackMode = false;
    }
    private void OnEnable()
    {
        transform.position = new Vector3(0, 21f, 0);
        mstartEffect = StartCoroutine(MovePatten());        
    }
    public void SetPos(Vector3 pos)
    {
        Vector3 tankRotateDistance = transform.position - pos;
        transform.position = tankRotateDistance;
    }
    #region 자동 이동 코루틴
    protected override IEnumerator MovePatten()
    {
        WaitForFixedUpdate Move = new WaitForFixedUpdate();

        m_RB.velocity = Vector2.down * mSpeed;
        while(transform.position.y > myPos)
        {
            yield return Move;
        }
        m_RB.velocity = Vector2.zero;
        m_attackMode = true;      
        mTankRotate.gameObject.SetActive(true);
        StartCoroutine(AttackPatten());
        StartCoroutine(Coroutine_AttackModeCheck());
    }
    #endregion

    #region 공격 모드 실시간 확인
    private IEnumerator Coroutine_AttackModeCheck()
    {
        while(true)
        {
            if(m_tankHit.GetCurrentHp <= 0)
            {
                m_attackMode = false;
                StopAllCoroutines();
                SoundController.Instance.EndLoopSound(); // 클리어 시 에는 반복 SFX Shoot Sound 종료
            }
            yield return null;
        }
    }
    #endregion

    #region 자동 공격 코루틴
    private IEnumerator AttackPatten()
    {
        WaitForSeconds Step = new WaitForSeconds(Random.Range(0.05f, 0.1f));
        var type = EnemyBoltPool.eEnemyBoltType.TwoBoss;
        //var soundType = SoundController.eBossSfxType.Boss02_Shoot;
        while (m_attackMode)
        {
            yield return Step;
            for (int i = 0; i < mFirePos.Length; i++)
            {
                EnemyBolt NewBolt = m_boltPool.GetFromPool((int)EnemyBoltPool.eEnemyBoltType.TwoBoss);
                m_boltPool.SetParentPos(NewBolt);
                NewBolt.transform.position = mFirePos[i].transform.position;
                var distance = m_tankShootEq.transform.position - NewBolt.transform.position;               
                NewBolt.transform.rotation = mFirePos[i].transform.rotation;
                NewBolt.SetShoot((int)type, "Player", 3, distance);
                SoundController.Instance.LoopSound(SoundController.eLoopSfxType.Boss02_Shoot);
                //NewBolt.transform.rotation = Quaternion.Euler(mRotatePos.rotation.eulerAngles.x, mRotatePos.rotation.eulerAngles.y, mRotatePos.rotation.eulerAngles.z + 90f);               
            }
        }       
    }
    #endregion
}


