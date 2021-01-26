using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private Rigidbody2D mRB;
   
    private float mSpeed;

    private int m_damage;
   
    private string mTargetTag;
    private string mNoTargetTag;

    private BoltPool.eBoltType m_type = BoltPool.eBoltType.None;

    // Start is called before the first frame update
    private void Awake()
    {       
        mRB = GetComponent<Rigidbody2D>();        
    }

    /*private void OnEnable()
    {
        mRB.velocity = transform.up * mSpeed;
    }*/

    public void SetBolt(BoltPool.eBoltType type, float speed)
    {
        m_type = type;
        mSpeed = speed;
        m_damage = (int)m_type + 1;        
        mRB.velocity = Vector2.up * mSpeed;
    }

    public void SetPos(BoltPool pool)
    {
        pool.SetParent(this);
    }

    public void Target(string tag)
    {
        mTargetTag = tag;
    }
    
    public void NoTarget(string tag)
    {
        mNoTargetTag = tag;
    }

    public void SetDamage(int damage)
    {
        m_damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(mTargetTag))
        {
            if(mTargetTag == "Box")
            {
                return;
            }
            collision.gameObject.SendMessage("Hit", m_damage);
        }     
    }
}
