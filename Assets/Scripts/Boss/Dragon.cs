using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : GokenBoss
{
    [SerializeField]
    private float m_disUpPos;

    //private Rigidbody2D mRB;   
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private float m_speed;
    [SerializeField]
    private float m_maxYpos;

    [SerializeField]
    private float m_attackMinPos, m_attackMaxPos;
    private bool m_attacking = false;

    private Coroutine m_coroutineMovePatten;
    private Coroutine m_coroutinAttackPatten;
    private Coroutine m_m_coroutSetAttack;

    [SerializeField]
    private BgController m_bgController;

    protected override void Awake()
    {
        m_type = eBossType.FourLevel;
        m_currhp = m_hp;
        transform.position = new Vector3(0, 21f, 0);       

        m_sprite = GetComponent<SpriteRenderer>();
        m_RB = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CapsuleCollider2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        m_coroutinAttackPatten = StartCoroutine(Coroutine_SetAttack());
        m_coroutineMovePatten = StartCoroutine(Coroutine_MovePatten());
    }

    #region 공격 모드 시간 체크
    private IEnumerator Coroutine_SetAttack()
    {
        float timer = Random.Range(5f, 10f);
        yield return new WaitForSeconds(timer);
        m_attacking = true;
    }
    #endregion

    #region 자동이동 코루틴
    private IEnumerator Coroutine_MovePatten()
    {
        float nomalSpeed = 5f;
        m_speed = nomalSpeed;
        bool moveing = true;
        WaitForFixedUpdate waiting = new WaitForFixedUpdate();
        float raydir;
        Vector3 rayDistance;
        //m_attacking = false;
        if (transform.position.y > m_maxYpos)
        {
            m_RB.velocity = Vector2.down * m_speed;
        }
        while (transform.position.y > m_maxYpos)
        {
            yield return waiting;
        }
        while (true)
        {
            //m_attackMode.gameObject.SetActive(false);
            m_speed = 5f;
            if (moveing)
            {
                m_RB.velocity = Vector2.left * m_speed;
                raydir = -1f;
                rayDistance = Vector2.left;
            }
            else
            {
                m_RB.velocity = Vector2.right * m_speed;
                raydir = 1f;
                rayDistance = Vector2.right;
            }
            Debug.DrawRay(new Vector3(transform.position.x + (raydir), transform.position.y), rayDistance, Color.red);            
            bool hit = Physics2D.Raycast(new Vector3(transform.position.x + (raydir), transform.position.y), rayDistance, m_collider.bounds.extents.x,1<<LayerMask.NameToLayer("Collider"));
            if (hit)
            {
                moveing = !moveing;
            }
            if (m_attacking) //공격 상태일때 이동 코루틴 중단
            {
                m_coroutinAttackPatten = StartCoroutine(Coroutine_AttackPatten());
                yield break;
            }
            yield return null;
        }
    }
    #endregion

    #region 자동공격 코루틴
    private IEnumerator Coroutine_AttackPatten()
    {
        //WaitForSeconds attackTime = new WaitForSeconds(Random.Range(2f, 10f));
        WaitForFixedUpdate waiting = new WaitForFixedUpdate();
        while (m_attacking)
        {
            m_bgController.SetSpeedScale(20f);
            //m_attackMode.gameObject.SetActive(true);
            m_speed = 10f;
            m_RB.velocity = Vector3.down * m_speed;
            m_spriteRenderer.flipY = true;
            while (transform.position.y > m_attackMinPos)
            {
                yield return waiting;
            }
            m_RB.velocity = Vector2.up * m_speed;
            m_spriteRenderer.flipY = false;
            while (transform.position.y < m_attackMaxPos)
            {
                yield return waiting;
            }
            m_attacking = false;
            m_spriteRenderer.flipY = true;
            m_RB.velocity = Vector2.zero;
            m_coroutineMovePatten = StartCoroutine(Coroutine_MovePatten());
            m_m_coroutSetAttack = StartCoroutine(Coroutine_SetAttack());
            m_bgController.SetSpeedScale(20f);
            yield return null;            
        }
    }
    #endregion
}