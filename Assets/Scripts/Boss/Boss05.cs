using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss05 : GokenBoss
{   
    [SerializeField]
    private GameObject m_firePosObj;
    private Transform[] m_firePos;
    [SerializeField]
    private GameObject m_combackEffect;
    private bool m_isAttack = false;
    protected override void Awake()
    {       
        m_type = eBossType.FiveLevel;
        m_isDie = false;
        m_hp = 5000;
        m_sprite = GetComponent<SpriteRenderer>();        
        m_RB = GetComponent<Rigidbody2D>();
        m_firePos = m_firePosObj.GetComponentsInChildren<Transform>();
        m_collider = GetComponent<CapsuleCollider2D>();
        m_currhp = m_hp;
    }  
    private void OnEnable()
    {
        transform.position = new Vector3(0f, 5f, 0f);        
        StartCoroutine(Coroutine_CombackEffectEnd());
    }
    public bool IsAttack { get { return m_isAttack; } }
    private IEnumerator Coroutine_CombackEffectEnd()
    {
        yield return new WaitForSeconds(10f);
        m_combackEffect.gameObject.SetActive(false);
        m_isAttack = true;
        StartCoroutine(AttackPatten());
    }
    IEnumerator AttackPatten()
    {
        bool isFire = true;
        WaitForSeconds createTime = new WaitForSeconds(Random.Range(0.5f, 1f));
        var boltType = EnemyBoltPool.eEnemyBoltType.None;
        var soundType = SoundController.eBossSfxType.None;
        while(m_isAttack)
        {
            yield return createTime;
            if(isFire)
            {
                boltType = EnemyBoltPool.eEnemyBoltType.FiveBossFire;
                soundType = SoundController.eBossSfxType.Boss05_Shoot02;
            }
            else
            {
                boltType = EnemyBoltPool.eEnemyBoltType.FiveBossIce;
                soundType = SoundController.eBossSfxType.Boss05_Shoot01;
            }
            for(int i=0; i<m_firePos.Length; i++)
            {
                EnemyBolt newBolt = menemyBoltPool.GetFromPool((int)boltType);
                menemyBoltPool.SetParentPos(newBolt);
                newBolt.transform.position = m_firePos[i].transform.position;
                var distance = transform.position - m_firePos[i].transform.position;
                newBolt.transform.rotation = m_firePos[i].transform.rotation;           
                newBolt.SetShoot((int)boltType, "Player", 3, distance);                                             
            }
            SoundController.Instance.SfxBossSound(soundType);
            yield return new WaitForSeconds(1f);
            isFire = !isFire;
        }
    }
    protected override void Hit(int damage)
    {
        if (m_isAttack)
        {
            base.Hit(damage);
        }
    }
}
