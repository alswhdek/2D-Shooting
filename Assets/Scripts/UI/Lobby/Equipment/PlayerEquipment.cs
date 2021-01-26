using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{  
    private UI2DSprite m_equipmentIcon;
    private EquipmentType m_type;
    private bool m_isInstall;

    [SerializeField]
    private UISprite m_selectSprite;
    private bool m_isSelect;

    public bool IsInstall { get { return m_isInstall; }set { m_isInstall = value; } }
    public bool IsSelect { get { return m_isSelect; }set { m_isSelect = value; } }
    public EquipmentType Type { get { return m_type; } }
    public void SetEquipment(EquipmentType type)
    {
        m_type = type;
    }
    public void ChangeEquipment(Menu_Store.StoreItemType type)
    {
        m_equipmentIcon.sprite2D = Resources.Load<Sprite>(string.Format("Images/StoreItems/Item_{0:00}", (int)type + 1));
    }
    public void ReChangeEquipment(EquipmentType type)
    {
        m_equipmentIcon.sprite2D = Resources.Load<Sprite>(string.Format("Images/UI/Lobby/Equipment/Equipment_{0:00}", (int)type + 1));
    }
    public void SetSelect()
    {
        m_selectSprite.gameObject.SetActive(false);
    }
    public void OnEquipmentSelect()
    {
        var slotList = InventoryController.Instance.SlotList;
        EquipmentController.Instance.OffEquipmentSelect();        
        m_isSelect = true;
        m_selectSprite.gameObject.SetActive(true);
        for(int i=0; i<slotList.Count; i++)
        {
            slotList[i].IsSelect = false;
            slotList[i].SetSelect(false);
        }
    }
    private void Awake()
    {
        m_equipmentIcon = GetComponent<UI2DSprite>();       
    }
}
