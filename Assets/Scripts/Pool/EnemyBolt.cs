using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBolt : MonoBehaviour
{
    [Header("Damage")]
    private int m_damage;    
    private Rigidbody2D mRB;

    [SerializeField]
    private float mSpeed;   

    private string mTargetName;
    // Start is called before the first frame update
    private void Awake()
    {       
        mRB = GetComponent<Rigidbody2D>();
    }
  
    private void OnEnable()
    {
        //mRB.velocity = Vector2.down * mSpeed;
    }
    public void SetBolt(int damage,string taget,int speed)
    {
        m_damage = damage;
        mTargetName = taget;
        mSpeed = speed;
        mRB.velocity = Vector2.down * mSpeed;
    }
    public void SetShoot(int damage, string taget, int speed, Vector3 pos)
    {
        m_damage = damage;
        mTargetName = taget;
        mSpeed = speed;
        mRB.velocity = pos * mSpeed;
    }

    /*public void SetBolt(EnemyBoltPool.eEnemyBoltType type)
    {
        m_spriteRenderer.sprite = Resources.Load<Sprite>(string.Format("Images/EnemyBolts/EnemyBolt_{0:00}", (int)type + 1));
        m_damage = (int)type + 1;
    }*/ 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(mTargetName))
        {
            collision.SendMessage("Hit",m_damage);
            gameObject.SetActive(false);            
        }
        /*else if(collision.gameObject.CompareTag("Invincibility"))
        {
            gameObject.SetActive(false);
        }*/
    }
}
