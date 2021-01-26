using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_ItemState : MonoBehaviour
{
    [SerializeField]
    private UI2DSprite m_itemIcon;
    [SerializeField]
    private UILabel m_itemNameLabel, m_itemInformationLabel,m_itemEventLabel,m_itemTitleLabel;
    private StoreItem m_storeItem;

    public void SetItemStatus(StoreItem storeItem,int itemIndex,string itemName,string itemInformation,string itemEvent,string itemTitle)
    {
        m_storeItem = storeItem;
        m_itemIcon.sprite2D = Resources.Load<Sprite>(string.Format("Images/StoreItems/Item_{0:00}", itemIndex + 1));
        m_itemNameLabel.text = itemName;
        m_itemInformationLabel.text = itemInformation;
        m_itemEventLabel.text = itemEvent;
        m_itemTitleLabel.text = itemTitle;
    }
    public void OnExitBtn()
    {
        PopupManager.Instance.ClosePopup();
        if(!PopupManager.Instance.IsOpened)
        {
            m_storeItem.IsPopup = false;
            m_storeItem.SetTween();
        }
    }
}
