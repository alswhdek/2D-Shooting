using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSlot : MonoBehaviour
{
    [SerializeField]
    private UISprite m_selectSprite;
    private Menu_Store m_store;
    private bool m_isSelect;    
    public bool IsSelect { get { return m_isSelect; } }    
    public void SetSelect(bool isSelect)
    {
        m_isSelect = isSelect;
        m_selectSprite.gameObject.SetActive(false);
        if (m_isSelect)
        {
            m_selectSprite.gameObject.SetActive(true);
        }
        else
        {
            m_selectSprite.gameObject.SetActive(false);
        }
    }
    public void OnSelectSlot()
    {        
        m_store.OnItemSelect();
        SetSelect(true);
        m_store.OnItemTween();
    }
    public void OnBuyBtnPress()
    {
        if(m_isSelect)
            m_store.ItemBuy();
    }
    private void Awake()
    {
        m_store = GameObject.FindGameObjectWithTag("Store").GetComponent<Menu_Store>();
    }
}
