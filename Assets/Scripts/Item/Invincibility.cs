using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    private float m_speed;
    private Rigidbody2D m_rb;
    private PlayerController m_player;
    
    public void SetSpeed()
    {
        m_rb.velocity = Vector3.up * 20f;
    }
    private void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void OnDisable()
    {
        transform.position = m_player.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.SetDie();
            }
        }
        if(collision.gameObject.CompareTag("EnemyBolt"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
