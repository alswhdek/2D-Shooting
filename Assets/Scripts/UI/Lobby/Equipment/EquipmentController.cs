using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    None=-1,
    Pitching,//투구
    Arrow,
    Armor,
    Sword,
    Gloves,
    Shoes,
    Max
}
public class EquipmentController : SingletonMonoBehaviour<EquipmentController>
{
    private PlayerEquipment[] m_equipment;
    private List<PlayerEquipment> m_equipmentList = new List<PlayerEquipment>();
    private Dictionary<EquipmentType, GameItem> m_equipmentTable = new Dictionary<EquipmentType, GameItem>();   
    private const int m_equipmentMaxCount = 6;

    public Status m_status;
    public List<PlayerEquipment> EquipList { get { return m_equipmentList; } }
    public PlayerEquipment[] EquipLength { get { return m_equipment; } }
    public Dictionary<EquipmentType, GameItem> EquipTable { get { return m_equipmentTable; } }
    
    private void InitState()
    {
        m_status = new Status(1, 0, 200, 100, 10, 50);
        PlayerStateUI.Instance.SetPlayerState(m_status.m_attack, m_status.m_defence, (int)m_status.m_moveSpeed, m_status.m_hp);
    }
    public void OffEquipmentSelect() // 선택 해제
    {
        for(int i=0; i< m_equipment.Length; i++)
        {
            if(m_equipment[i].IsSelect)
            {
                m_equipment[i].IsSelect = false;
                m_equipment[i].SetSelect();
                break;
            }
        }
    }
    public void EquipmentInstall() // 장착
    {
        var slotList = InventoryController.Instance.SlotList;
        var itemTable = InventoryController.Instance.ItemTable;

        for(int i=0; i<slotList.Count; i++)
        {
            if(slotList[i].IsSelect && itemTable[slotList[i]] != null)
            {
                if(m_equipmentTable[itemTable[slotList[i]].EquipMentType] == null) // 장비 칸이 비워있다면
                {
                    m_equipmentTable[itemTable[slotList[i]].EquipMentType] = itemTable[slotList[i]]; // 착용하려는 아이템이 장비칸으로 들어간다.
                    m_equipment[(int)itemTable[slotList[i]].EquipMentType].ChangeEquipment(itemTable[slotList[i]].Type);// 장비칸의 이미지를 변경해준다.
                    SetEquipWearing(itemTable[slotList[i]].EquipMentType,true);
                    InventoryController.Instance.RemoveItem(itemTable[slotList[i]]);                   
                    itemTable[slotList[i]] = null;                   
                    break;
                }
                else
                {
                    var beforeItem = m_equipmentTable[itemTable[slotList[i]].EquipMentType]; //n번째 장비를 변경하기전에 이전에 착용하고있던 장비 백업
                    var afterItem = itemTable[slotList[i]];//착용 하려고하는 아이템
                    SetEquipment(itemTable[slotList[i]].EquipMentType, afterItem); // n번째 장비를 장착                    
                    m_equipment[(int)itemTable[slotList[i]].EquipMentType].ChangeEquipment(afterItem.Type);// n번째 장비 스프라이트를 변경
                    SetEquipWearing(beforeItem.EquipMentType, false); // 해제
                    SetEquipWearing(itemTable[slotList[i]].EquipMentType, true); // 장착                    
                    InventoryController.Instance.RemoveItem(afterItem);
                    itemTable[slotList[i]] = beforeItem; //기존에 착용하고있는 장비를 n번째 슬롯에 복귀
                    InventoryController.Instance.SetItem(slotList[i], itemTable[slotList[i]]);                    
                    break;
                }
            }
        }
    }
    public void SetEquipment(EquipmentType type,GameItem item)
    {
        m_equipmentTable[type] = item;
    }
    private IEnumerator Coroutine_SetAcive(GameObject obj,bool isOn)
    {
        yield return new WaitForEndOfFrame();
        obj.gameObject.SetActive(isOn);
    }
    public void EquipmentRelease() // 해제
    {
        var slotList = InventoryController.Instance.SlotList;
        var itemTable = InventoryController.Instance.ItemTable;
        for(int i=0; i<slotList.Count; i++)
        {
            if(itemTable[slotList[i]] == null)
            {
                for(int j=0; j<m_equipment.Length; j++)
                {
                    if(m_equipment[j].IsSelect && m_equipmentTable[(EquipmentType)j] != null)
                    {
                        var beforeItem = m_equipmentTable[(EquipmentType)j];
                        m_equipment[j].ReChangeEquipment((EquipmentType)j);
                        SetEquipWearing(beforeItem.EquipMentType, false);
                        m_equipmentTable[(EquipmentType)j] = null;                     
                        itemTable[slotList[i]] = beforeItem;
                        InventoryController.Instance.SetItem(slotList[i],itemTable[slotList[i]]);
                    }
                }
            }
        }
    }
    private void SetEquipWearing(EquipmentType equipType,bool isEquipWearing) //장비 장착과 해제
    {
        StoreData storeData = null;
        var item = m_equipmentTable[equipType];
        var storeItemTable = Menu_Store.Instance.storeTable;
        if (!storeItemTable.TryGetValue(item.Type, out storeData)) return;
        switch (equipType)
        {
            case EquipmentType.Pitching: //투구
                PlayerDataManager.Instance.SetDefence(PlayerDataManager.Instance.GetPlayerIndex(), storeData.m_armor, isEquipWearing);
                break;
            case EquipmentType.Arrow: //짧은 검
                PlayerDataManager.Instance.SetAttack(PlayerDataManager.Instance.GetPlayerIndex(), storeData.m_attack, isEquipWearing);
                break;
            case EquipmentType.Armor: //갑옷
                PlayerDataManager.Instance.SetDefence(PlayerDataManager.Instance.GetPlayerIndex(), storeData.m_armor, isEquipWearing);
                break;
            case EquipmentType.Sword: //장검
                PlayerDataManager.Instance.SetAttack(PlayerDataManager.Instance.GetPlayerIndex(), storeData.m_attack, isEquipWearing);
                break;
            case EquipmentType.Gloves: //장갑
                PlayerDataManager.Instance.SetHp(PlayerDataManager.Instance.GetPlayerIndex(), storeData.m_hp, isEquipWearing);
                break;
            case EquipmentType.Shoes: //신발
                PlayerDataManager.Instance.SetSpeed(PlayerDataManager.Instance.GetPlayerIndex(), storeData.m_speed, isEquipWearing);
                break;
        }
        PlayerStateUI.Instance.SetPlayerState(PlayerDataManager.Instance.GetAttack(PlayerDataManager.Instance.GetPlayerIndex()), PlayerDataManager.Instance.GetDefence(PlayerDataManager.Instance.GetPlayerIndex()),
            PlayerDataManager.Instance.GetSpeed(PlayerDataManager.Instance.GetPlayerIndex()), PlayerDataManager.Instance.GetHp(PlayerDataManager.Instance.GetPlayerIndex()));
    }
    public void EquipStateEvent()
    {
        PlayerStateUI.Instance.SetPlayerState(PlayerDataManager.Instance.GetAttack(PlayerDataManager.Instance.GetPlayerIndex()), PlayerDataManager.Instance.GetDefence(PlayerDataManager.Instance.GetPlayerIndex()),
            PlayerDataManager.Instance.GetSpeed(PlayerDataManager.Instance.GetPlayerIndex()), PlayerDataManager.Instance.GetHp(PlayerDataManager.Instance.GetPlayerIndex()));       
    }
    protected override void OnStart()
    {
        EquipStateEvent();
        m_equipment = GetComponentsInChildren<PlayerEquipment>();        
        for (int i=0; i< (int)m_equipment.Length; i++)
        {
            m_equipment[i].IsSelect = false;
            m_equipment[i].SetSelect();
            m_equipmentTable.Add((EquipmentType)i, null);
        }
        InitState();       
    }
}
