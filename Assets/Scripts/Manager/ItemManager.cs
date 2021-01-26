using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager> 
{
    [SerializeField]
    private ItemPool m_itemPool;
    private int[] m_itemTable = new int[7] {5,10,5,2,6,70,5};
    public Dictionary<ItemType, ItemData> m_dicItem = new Dictionary<ItemType, ItemData>();
    public void CreateItem(Vector3 pos, Enemy enemy = null)
    {
        ItemType type = (ItemType)Util.GetItemDropRate(m_itemTable);
        Item newItem = m_itemPool.GetFromPool();
        if (enemy != null)
        {
            newItem.SetItem(type, pos, m_dicItem[type].m_icon, enemy);
        }
        else
        {
            newItem.SetItem(type, pos, m_dicItem[type].m_icon);
        }
        newItem.transform.SetParent(transform);           
    }
    private void InitItem()
    {        
        m_dicItem.Add(ItemType.Shadow, new ItemData() { m_icon = 1,m_attack = 0, m_duration = 15f, m_speed = 0, m_gold = 0 });
        m_dicItem.Add(ItemType.invincibility, new ItemData() { m_icon=2, m_attack = 0, m_duration = 5f, m_speed = 0, m_gold = 0 });
        m_dicItem.Add(ItemType.Magnet, new ItemData() { m_icon = 3,m_attack = 0, m_duration = 10f, m_speed = 0, m_gold = 0 });
        m_dicItem.Add(ItemType.Speed, new ItemData() { m_icon = 4,m_attack = 0, m_duration = 10f, m_speed = 2f, m_gold = 0 });
        m_dicItem.Add(ItemType.ResetHp, new ItemData() { m_icon = 0,m_attack = 0, m_duration = 0f, m_speed = 0, m_gold = 0 });
        m_dicItem.Add(ItemType.Gold, new ItemData() { m_icon = 0, m_attack = 0, m_duration = 0, m_speed = 0, m_gold = 10 });
        m_dicItem.Add(ItemType.Attack, new ItemData() { m_icon = 6,m_attack = 5, m_duration = 15f, m_speed = 0, m_gold = 0 });
    }
    protected override void OnAwake()
    {
        InitItem();
    }
}
