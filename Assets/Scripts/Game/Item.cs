using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


public class Item : MonoBehaviour
{
    private SpriteRenderer m_itemSprite;
    private TweenPosition m_tween;
    private ItemType m_type;
    private int m_icon;
    private int m_attack;
    private float m_duration;
    private float m_speed;
    private int m_gold;

    private PlayerController m_player;
    private Enemy m_enemy; 

    public void SetItem(ItemType type,Vector3 pos,int icon,Enemy enemy = null)
    {       
        m_type = type;
        m_icon = icon;
        transform.position = pos;
        ItemData itemData;
        ItemManager.Instance.m_dicItem.TryGetValue(m_type, out itemData);
        m_itemSprite.sprite = Resources.Load<Sprite>(string.Format("Images/Item/Item_{0:00}", (int)m_type + 1));
        m_attack = itemData.m_attack;
        m_duration = itemData.m_duration;
        m_speed = itemData.m_speed;
        m_gold = itemData.m_gold;        
        if (!m_player.IsMagnet)
        {
            m_tween.enabled = true;
            m_tween.from = pos;
            m_tween.to = m_tween.from + (Vector3.up * 5f);
            m_tween.ResetToBeginning();
        }
        if (enemy != null)
        {
            m_enemy = enemy;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Magnet"))
        {            
            if (m_player == null) return;
            if (m_type != ItemType.ResetHp && m_type != ItemType.Gold)
            {
                BuffManager.Instance.CreateBuffUI(m_type, m_duration, m_icon);
            }
            switch (m_type)
            {
                case ItemType.Shadow:
                    m_player.SetShadow();
                    break;
                case ItemType.invincibility:
                    m_player.Setinvincibility();
                    BgController.Instance.SetSpeedScale(5f);                   
                    BuffManager.Instance.SetEnemyObject(m_enemy);                    
                    break;
                case ItemType.Magnet:
                    m_player.SetMarent();
                    break;
                case ItemType.Speed:
                    m_player.SetSpeed(m_speed);
                    break;
                case ItemType.ResetHp:
                    m_player.SetHp();
                    break;
                case ItemType.Gold:
                    GameController.Instance.AddGold(m_gold + Random.Range(10,101));
                    break;
                case ItemType.Attack:
                    m_player.SetAttack(m_attack);
                    break;
            }
            SoundController.Instance.PlayItemSound(SoundController.eItemCoinSound.item);
            gameObject.SetActive(false);
        }
    }
    private void Awake()
    {        
        m_tween = GetComponent<TweenPosition>();
        m_itemSprite = GetComponent<SpriteRenderer>();
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
    private void Update()
    {
        if(m_player.IsMagnet)
        {
            m_tween.enabled = false;
            var distance = m_player.transform.position - transform.position;
            transform.position += distance.normalized * 25f * Time.deltaTime;
        }
        else if(!m_player.IsMagnet || m_player == null)
        {
            transform.position += Vector3.down * 15 * Time.deltaTime;
        }
    }
}
