using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{   
    private UI2DSprite m_itemIcon;

    private Menu_Store.StoreItemType m_itemType;
    private EquipmentType m_equipType;
    public Menu_Store.StoreItemType Type { get { return m_itemType; } }
    public EquipmentType EquipMentType { get { return m_equipType; } }
    public void SetItem(Menu_Store.StoreItemType itemType,EquipmentType equipType)
    {
        m_itemType = itemType;
        m_equipType = equipType;
        m_itemIcon.sprite2D = Resources.Load<Sprite>(string.Format("Images/StoreItems/Item_{0:00}", (int)itemType+1));
    }
    private void Awake()
    {
        m_itemIcon = GetComponent<UI2DSprite>();
    }
}
