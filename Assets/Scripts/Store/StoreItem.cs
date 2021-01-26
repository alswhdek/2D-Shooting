using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    Menu_Store.StoreItemType m_type = Menu_Store.StoreItemType.None;
    private string m_itemName;

    private UI2DSprite m_sprite;
    [SerializeField]
    private UILabel m_costLabel;
    private TweenScale m_tweenScale;

    private Menu_Store m_store;

    private bool m_isSelect;
    private bool m_isPopup;
    public bool IsSelect { get { return m_isSelect; } set { m_isSelect = value; } }
    public bool TweenActive { get { return m_tweenScale.enabled; } set { m_tweenScale.enabled = value; } }
    public bool IsPopup { get { return m_isPopup; }set { m_isPopup = value; } }
    public void SetTween()
    {
        if(m_isPopup)
        {
            m_tweenScale.enabled = true;
        }
        if (!m_isPopup)
        {
            m_tweenScale.enabled = false;
        }
    }
    public void SetItem(Menu_Store.StoreItemType type,string itemName)
    {
        m_type = type;
        m_itemName = itemName;
        m_sprite.sprite2D = Resources.Load<Sprite>(string.Format("Images/StoreItems/Item_{0:00}", (int)type+1));     
    }
    public void ShowItem()
    {        
        m_tweenScale.enabled = false;
        if (m_isSelect)
        {
            m_tweenScale.enabled = true;
        }
        else
        {
            m_tweenScale.enabled = false;
        }
    }   
    public void ShowItemCost()
    {
        StoreData storeData;
        m_store.m_storeTable.TryGetValue(m_type, out storeData);
        storeData = m_store.m_storeTable[m_type];       
        m_costLabel.text = string.Format("{0:00}원", storeData.m_cost.ToString());
    }
    public void OnItemStatusBtn()
    {
        StoreData storeData = m_store.m_storeTable[m_type];
        m_store.m_storeTable.TryGetValue(m_type, out storeData);
        string itemEventType = string.Empty;
        int value = 0;

        switch(m_type)
        {
            case Menu_Store.StoreItemType.Armor:
                itemEventType = "방어력";
                value = storeData.m_armor;
                break;
            case Menu_Store.StoreItemType.Arrow:
                itemEventType = "공격력";
                value = storeData.m_attack;
                break;
            case Menu_Store.StoreItemType.Shoes:
                itemEventType = "속력";
                value = storeData.m_speed;
                break;
            case Menu_Store.StoreItemType.Pitching:
                itemEventType = "방어력";
                value = storeData.m_armor;
                break;
            case Menu_Store.StoreItemType.Gloves:
                itemEventType = "HP 증가";
                value = (int)storeData.m_hp;
                break;
            case Menu_Store.StoreItemType.Sword:
                itemEventType = "공격력";
                value = (int)storeData.m_attack;
                break;
        }
        if (m_isSelect)
        {
            if (!PopupManager.Instance.IsOpened) //열려있는 팝업창이 없다면
            {
                PopupManager.Instance.OpenPopupItemStatusOk(this,(int)m_type, m_itemName, string.Format("\n\n설명 : 데마시아에서 전통으로\n\n내려온 {0} 입니다.", m_itemName), string.Format("효과 : {0}+{1}", itemEventType, value), "아이템 정보");
                m_isPopup = true;
                m_tweenScale.enabled = false;
            }
        }
    }  

    private void Awake()
    {
        m_store = GameObject.FindGameObjectWithTag("Store").GetComponent<Menu_Store>();
        m_sprite = GetComponent<UI2DSprite>();
        m_tweenScale = GetComponent<TweenScale>();        
    }   
}
