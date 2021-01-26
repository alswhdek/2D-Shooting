using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Characters : MonoBehaviour,ILobbyMenu
{
    [SerializeField]
    private UI2DSprite m_charSprite;
    [SerializeField]
    private GameObject m_select, m_buy;
    [SerializeField]
    private Vector3[] m_charPos;
    [SerializeField]
    private TweenPosition m_tweenPos;

    [SerializeField]
    private LobbyController m_lobbycontroller;

    private List<string> m_charnameList = new List<string>() { "제너럴", "레이", "카이" };

    private int m_selectIndex;

    private void Start()
    {
        ReSetUI();
    }
    public void ReSetUI()
    {
        var isOpen = PlayerDataManager.Instance.isOpenHero(m_selectIndex);
        m_select.gameObject.SetActive(false);
        m_buy.gameObject.SetActive(false);
        if(isOpen)
        {
            m_select.gameObject.SetActive(true);
        }
        else
        {
            m_buy.gameObject.SetActive(true);
        }
    }
    public void OnPressLeft()
    {
        m_selectIndex--;
        if (m_selectIndex < 0)
        {
            m_selectIndex = m_charPos.Length - 1;
        }
        LoadCharacters();
    }
    public void OnPressRight()
    {
        m_selectIndex++;
        if (m_selectIndex > m_charPos.Length-1)
        {
            m_selectIndex = 0;
        }
        LoadCharacters();
    }

    public void OnPressSelect()
    {
        Close();
        PlayerDataManager.Instance.SetPlayerIndex(m_selectIndex);
        m_lobbycontroller.ResetUI(m_charSprite.sprite2D,m_charPos[m_selectIndex],m_selectIndex);
        m_lobbycontroller.gameObject.SetActive(true);
    }

    public void OnPressBuy()
    {
        if(!PopupManager.Instance.IsOpened) //열려있는 팝업이 없을경우 구매버튼을 클릭 시 구매 팝업이 노출이된다.
        {
            PopupManager.Instance.OpenPopupOkCancel("[ffff00]Notice[-]", string.Format("[ffff00]{0}[-]캐릭터를 [ffff00]{1}[-]원으로 구매하시겠습니까??",m_charnameList[m_selectIndex],40), () =>
              {
                  if (PlayerDataManager.Instance.DecreaseGem(40)) //자본이 40원 이상이면 구매가된다.
                {
                      PopupManager.Instance.ClosePopup();                     
                      PopupManager.Instance.OpenPopupOk("[ffff00]Notice[-]", string.Format("[ffff00]{0}[-]캐틱터가 구매되었습니다.!", m_charnameList[m_selectIndex]));
                      PlayerDataManager.Instance.OpenHero(m_selectIndex);
                      ReSetUI();
                  }
                  else
                  {
                      PopupManager.Instance.OpenPopupOk("[ffff00]Notice[-]", "Gem이 부족합니다. Gem 충전 후 다시 구매해주세요..");
                  }
              });
        }
    }

    private Sprite GetCharactersSprite(int index)
    {
        var spr = Resources.Load<Sprite>(string.Format("Images/Character/character_{0:00}", index + 1));
        return spr;
    }

    private void LoadCharacters()
    {
        m_charSprite.sprite2D = GetCharactersSprite(m_selectIndex);
        ReSetUI();
        m_charSprite.transform.localPosition = m_charPos[m_selectIndex];
        m_tweenPos.from = m_charSprite.transform.localPosition;
        m_tweenPos.to = m_tweenPos.from + (Vector3.up * 17f);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
}
