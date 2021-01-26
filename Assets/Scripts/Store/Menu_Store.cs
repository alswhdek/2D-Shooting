using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Store : SingletonMonoBehaviour<Menu_Store>,ILobbyMenu
{
    public enum StoreItemType
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
      
    private const int MAX_SLOT = 6;

    [SerializeField]
    private SlotPool m_slotPool;
    [SerializeField]
    private StoreItemPool m_itemPool;

    [SerializeField]
    private UILabel m_playerGold;
    [SerializeField]
    private UILabel m_playerGem;
    [SerializeField]
    private UIGrid m_gridObj;
    [SerializeField]
    private GameObject m_itemPopupParent;

    [SerializeField]
    private LobbyController m_lobbyController;

    public Dictionary<StoreItemType, StoreData> m_storeTable = new Dictionary<StoreItemType, StoreData>();

    private List<string> m_itemNameList = new List<string>() { "라마돈 모자","전설의검","가시갑옷","무한의대검","목장갑","헤르메스의 장화" };
    private List<StoreSlot> m_slotList = new List<StoreSlot>();
    private List<StoreItem> m_storeItemList = new List<StoreItem>();  
    
    public Dictionary<StoreItemType, StoreData> storeTable { get { return m_storeTable; } }    
    private void InitItem()
    {
        m_storeTable.Add(StoreItemType.Armor, new StoreData() { m_cost = 2000,m_armor = 500, m_attack = 0, m_speed = 0, m_hp = 0,m_boltSpeed = 0,m_critical = 0, m_equipType = EquipmentType.Armor});
        m_storeTable.Add(StoreItemType.Arrow, new StoreData() { m_cost = 5000, m_armor = 0, m_attack = 2000, m_speed = 0, m_hp = 0, m_boltSpeed = 0, m_critical = 0, m_equipType = EquipmentType.Arrow });
        m_storeTable.Add(StoreItemType.Shoes, new StoreData() { m_cost = 2500, m_armor = 0, m_attack = 0, m_speed = 5, m_hp = 0, m_boltSpeed = 0, m_critical = 0, m_equipType = EquipmentType.Shoes });
        m_storeTable.Add(StoreItemType.Pitching, new StoreData() { m_cost = 3000, m_armor = 200, m_attack = 0, m_speed = 0, m_hp = 0, m_boltSpeed = 0, m_critical = 0, m_equipType = EquipmentType.Pitching });       
        m_storeTable.Add(StoreItemType.Gloves, new StoreData() { m_cost = 3500, m_armor = 0, m_attack = 0, m_speed = 0, m_hp = 700, m_boltSpeed = 2, m_critical = 0, m_equipType = EquipmentType.Gloves });
        m_storeTable.Add(StoreItemType.Sword, new StoreData() { m_cost = 20000, m_armor = 0, m_attack = 7000, m_speed = 0, m_hp = 0, m_boltSpeed = 0, m_critical = 20, m_equipType = EquipmentType.Sword });
    }
    private void CreateSlot()
    {                    
        for (int i = 0; i < MAX_SLOT; i++)
        {
            StoreSlot newSlot = m_slotPool.GetFromPool();
            newSlot.transform.SetParent(m_gridObj.transform);
            newSlot.transform.localScale = Vector3.one;           
            m_gridObj.Reposition();
            m_slotList.Add(newSlot);
        }
    }
    private void CreateItem()
    {
        for(int i=0; i<m_slotList.Count; i++)
        {
            //StoreItemType type = (StoreItemType)i;           
            //int index = int.Parse(m_itemSprite[i].name.Split('.')[0]) -1;
            StoreItem newItem = m_itemPool.GetFromPool();
            newItem.SetItem((StoreItemType)i,m_itemNameList[i]);           
            newItem.transform.SetParent(m_slotList[i].transform);
            newItem.transform.localScale = Vector3.one;
            newItem.transform.localPosition = Vector3.up * 45f;
            newItem.ShowItemCost();
            m_storeItemList.Add(newItem);
        }
    }  
    public void Close()
    {
        gameObject.SetActive(false);
        LobbyController.Instance.OpenLobby();
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void SetParentObject(GameObject obj)
    {
        obj.transform.SetParent(m_itemPopupParent.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
    }
    public void OnItemSelect() //전에 선택되어있는 아이템 해제
    {
        for(int i=0; i<m_slotList.Count; i++)
        {
            if(m_slotList[i].IsSelect)
            {
                m_slotList[i].SetSelect(false);
                m_storeItemList[i].IsSelect = false;
                m_storeItemList[i].ShowItem();
                break;
            }
        }            
    }
    public void OnItemTween()
    {
        for (int i = 0; i < m_slotList.Count; i++)
        {
            if (m_slotList[i].IsSelect)
            {
                m_storeItemList[i].IsSelect = true;
                m_storeItemList[i].ShowItem();
            }
        }
    }
    public void ShowPlayerAsset() //플레이어의 자산 
    {
        var gold = PlayerDataManager.Instance.GetGold();
        var gem = PlayerDataManager.Instance.GetGem();
        m_playerGold.text = string.Format("{0:00}￦", gold);
        m_playerGem.text = string.Format("{0:00}", gem);
    }
    public void ItemBuy() //아이템 구매
    {
        StoreData storeData;
        for(int i=0; i< m_storeTable.Count; i++)
        {
            m_storeTable.TryGetValue((StoreItemType)i, out storeData);
            if (m_slotList[i].IsSelect) // 슬롯이 선택되어있어야한다.
            {               
                if (!PopupManager.Instance.IsOpened) //오픈되어있는 팝업창이 없어야한다.
                {
                    PopupManager.Instance.OpenPopupOkCancel("알림", string.Format("{1}의 아이템의 가격은 {0:00}원 입니다. 구매하시겠습니까?", m_storeTable[(StoreItemType)i].m_cost, m_itemNameList[i]), () =>                    
                    {                       
                        if (PlayerDataManager.Instance.DecreaseGold(storeData.m_cost))//자산 >= 상점 아이템 가격
                        {
                            if(InventoryController.Instance.AddItem((StoreItemType)i))
                            {
                                PopupManager.Instance.ClosePopup();
                                PopupManager.Instance.OpenPopupOk("알림", "구매가 완료되었습니다.");
                                ShowPlayerAsset();
                            }
                            else
                            {
                                PopupManager.Instance.OpenPopupOk("알림", "인벤토리 칸 수가 부족합니다..");
                            }
                        }
                        else
                        {
                            PopupManager.Instance.ClosePopup();
                            PopupManager.Instance.OpenPopupOk("알림", "골드가 부족합니다. !!");                                                     
                        }
                    });
                }
                break;
            }           
        }
    }
    public void StoreExit()
    {
        Close();
        //m_lobbyController.gameObject.SetActive(true);
    }
    protected override void OnStart()
    {       
        CreateSlot();
        InitItem();
        CreateItem();
        ShowPlayerAsset();

        for (int i=0; i<m_slotList.Count; i++)
        {
            m_slotList[i].SetSelect(false);
        }
        m_slotList[0].SetSelect(true);
        for(int i=0; i<m_storeItemList.Count; i++)
        {
            m_storeItemList[i].IsSelect = false;
            m_storeItemList[i].TweenActive = false;
        }
        m_storeItemList[0].IsSelect = true;
        m_storeItemList[0].ShowItem();
    }
}
