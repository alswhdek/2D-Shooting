using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGoldType
{
    Broze,
    Sliver,
    Gold,
    MaxCount,
}

public class Gold : MonoBehaviour
{
    [SerializeField]
    private eGoldType mType;
    [SerializeField]
    private float mSpeed;  
    private Rigidbody2D mRB;

    private void Awake()
    {
        mRB = GetComponent<Rigidbody2D>();
    }
    public void SetPos(GoldPool pool)
    {
        pool.SetParent(this);
    }

    public void GoldDown()
    {
        mRB.velocity = Vector2.down * mSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("NewGold", mType);
            //사운드
            gameObject.SetActive(false);
        }
    }
}
