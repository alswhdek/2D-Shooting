using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSlot : MonoBehaviour
{
    [SerializeField]
    private UISprite m_selectSprite;
    private bool m_isSelect;
    private bool m_isNull;
    public bool IsSelect { get { return m_isSelect; }set { m_isSelect = value; } }
    public bool IsNull { get { return m_isNull; }set { m_isNull = value; } }
    
    public void SetSelect(bool isSelect)
    {
        m_selectSprite.gameObject.SetActive(isSelect);
    }    
    public void SetParent(GameItem item)
    {
        item.transform.SetParent(item.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
    }
    public void OnSelect()
    {
        var equipmentLength = EquipmentController.Instance.EquipLength;
        InventoryController.Instance.OffSlotSelect();
        m_isSelect = true;
        m_selectSprite.gameObject.SetActive(true);
        for(int i=0; i< equipmentLength.Length; i++)
        {
            equipmentLength[i].IsSelect = false;
            equipmentLength[i].SetSelect();
        }
    }
    private void Update()
    {
        
    }
}
