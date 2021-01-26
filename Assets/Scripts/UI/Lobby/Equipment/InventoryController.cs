using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : SingletonMonoBehaviour<InventoryController>, ILobbyMenu
{
    private GameObjectPool<GameSlot> m_slotPool;
    private List<GameSlot> m_slotList = new List<GameSlot>();
    private Dictionary<Menu_Store.StoreItemType, GameObjectPool<GameItem>> m_itemPool = new Dictionary<Menu_Store.StoreItemType, GameObjectPool<GameItem>>();
    private Dictionary<GameSlot, GameItem> m_itemTable = new Dictionary<GameSlot, GameItem>();
    private Dictionary<Menu_Store.StoreItemType, List<GameItem>> m_dicItemPool = new Dictionary<Menu_Store.StoreItemType, List<GameItem>>();

    [SerializeField]
    private GameObject m_slotPrefab;
    [SerializeField]
    private GameObject m_itemPrefab;
    [SerializeField]
    private GameObject m_itemPoolObj;
    [SerializeField]
    private UIGrid m_grid;

    private const int m_slotMaxCount = 15;

    [SerializeField]
    private GameObject m_slotObject;

    [SerializeField]
    private UI2DSprite m_playerSprite;

    public List<GameSlot> SlotList { get { return m_slotList; } }
    public Dictionary<GameSlot, GameItem> ItemTable { get { return m_itemTable; } }
    private IEnumerator Coroutine_SetActive(GameObject obj, bool isOn)
    {
        yield return new WaitForEndOfFrame();
        obj.gameObject.SetActive(isOn);
    }
    public void CreateSlot()
    {
        var slot = m_slotPool.Get();
        slot.SetSelect(false);
        slot.IsNull = true;
        m_slotList.Add(slot);
        StartCoroutine(Coroutine_SetActive(slot.gameObject, true));
    }
    public void RemoveItem(GameItem item)
    {          
        item.gameObject.SetActive(false);
        m_dicItemPool[item.Type].Add(item);
        /*var itemPool = m_itemPool[item.Type];
        itemPool.Set(item);
        item.gameObject.SetActive(false);*/
    }
    public void OffSlotSelect()
    {
        for (int i = 0; i < m_slotList.Count; i++)
        {
            if (m_slotList[i].IsSelect)
            {
                m_slotList[i].IsSelect = false;
                m_slotList[i].SetSelect(false);
                break;
            }
        }
    }
    public void SetItem(GameSlot slot,GameItem item)
    {
        item.transform.SetParent(slot.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localScale = Vector3.one;
        item.gameObject.SetActive(true);
    }
    public bool AddItem(Menu_Store.StoreItemType type)
    {
        var itemPool = m_dicItemPool[type];
        GameItem item = null;
        for (int i = 0; i < m_slotList.Count; i++)
        {
            if (m_itemTable[m_slotList[i]] == null)
            {
                if(itemPool.Count < 0)
                {
                    var obj = Instantiate(m_itemPrefab);
                    obj.transform.SetParent(m_slotList[i].transform);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                    var gameItem = obj.GetComponent<GameItem>();
                    gameItem.SetItem(type, (EquipmentType)type);
                    gameItem.gameObject.SetActive(true);
                    m_itemTable[m_slotList[i]] = gameItem;
                    itemPool.Add(gameItem);
                    return true;
                }
                else
                {
                    item = itemPool[0];
                    itemPool.Remove(item);
                    item.transform.SetParent(m_slotList[i].transform);
                    item.transform.localPosition = Vector3.zero;
                    item.transform.localScale = Vector3.one;
                    item.SetItem(type, (EquipmentType)type);
                    item.gameObject.SetActive(true);
                    m_itemTable[m_slotList[i]] = item;
                    return true;
                }
            }
        }
        return false;
    }
    public void DeleteItem()
    {
        for(int i=0; i<m_slotList.Count; i++)
        {
            if(m_slotList[i].IsSelect && m_itemTable[m_slotList[i]] != null)
            {
                RemoveItem(m_itemTable[m_slotList[i]]);
                m_itemTable[m_slotList[i]] = null;
                break;
            }
        }
    }
    public void Exit()
    {
        Close();
        LobbyController.Instance.gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
        //LobbyController.Instance.OpenLobby();
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void LoadCharcterSprite(int index)
    {
        m_playerSprite.sprite2D = Resources.Load<Sprite>(string.Format("Images/Player/Player_{0:00}", PlayerDataManager.Instance.GetPlayerIndex() + 1));
        if(PlayerDataManager.Instance.GetPlayerIndex() == 1)
            m_playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
        else
            m_playerSprite.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
    protected override void OnStart()
    {
        LoadCharcterSprite(PlayerDataManager.Instance.GetPlayerIndex());
        m_slotPool = new GameObjectPool<GameSlot>(m_slotMaxCount, () =>
        {
            var obj = Instantiate(m_slotPrefab);
            StartCoroutine(Coroutine_SetActive(obj, false));
            obj.transform.SetParent(m_grid.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            m_grid.Reposition();
            var slot = obj.GetComponent<GameSlot>();
            return slot;
        });
        for (int i = 0; i < m_slotMaxCount; i++)
        {
            CreateSlot();
        }
        List<GameItem> itemlist = new List<GameItem>();
        for (int i = 0; i < (int)Menu_Store.StoreItemType.Max; i++)
        {
            var pool = new GameObjectPool<GameItem>(3, () =>
            {
                var obj = Instantiate(m_itemPrefab);                              
                obj.transform.SetParent(m_itemPoolObj.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                var item = obj.GetComponent<GameItem>();
                item.SetItem((Menu_Store.StoreItemType)i, (EquipmentType)i);
                obj.gameObject.SetActive(false);
                itemlist.Add(item);
                return item;
            });
            m_dicItemPool.Add((Menu_Store.StoreItemType)i,itemlist);
            m_itemPool.Add((Menu_Store.StoreItemType)i, pool);
        }
        for (int i = 0; i < m_slotMaxCount; i++)
        {
            m_itemTable.Add(m_slotList[i], null);
        }
    }
}
