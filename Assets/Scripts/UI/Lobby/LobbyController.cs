using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : SingletonMonoBehaviour<LobbyController>
{
    [SerializeField]
    private UIGrid m_grid;
    private UIButton[] m_mainMenuBtn;
    [SerializeField]
    private GameObject m_menuObject;
    ILobbyMenu[] m_lobbymenu;

    [SerializeField]
    private UI2DSprite m_charSprite;
    [SerializeField]
    private UISprite m_chariconSprite;
    [SerializeField]
    private TweenPosition m_tweenPos;
   
    public EventDelegate.Parameter MakeParameter(Object _value,System.Type _type)
    {
        EventDelegate.Parameter param = new EventDelegate.Parameter();
        param.obj = _value;
        param.expectedType = _type;
        return param;
    }

    public void OnButtonClick(UIButton button)
    {
        gameObject.SetActive(false);
        for(int i=0; i<m_mainMenuBtn.Length; i++)
        {
            if(m_mainMenuBtn[i] == button)
            {
                m_lobbymenu[i].Open();
                break;
            }
        }
    }
    public void OpenLobby()
    {
        gameObject.SetActive(true);
    }
    public void GameStart()
    {
        if(LoadSceneManager.Instance.GetCurrentScene == LoadSceneManager.SceneState.Lobby) 
        {
            //현재 내가있는 Scene이 로비일때 게임 씬으로 이동
            LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Game);
        }
    }

    public void ResetUI(Sprite charsprite,Vector3 pos,int index)
    {
        m_charSprite.sprite2D = charsprite; // 캐릭터 변경       
        m_chariconSprite.spriteName = string.Format("select_character_{0:00}", index + 1);
        m_charSprite.transform.localPosition = pos;
        m_tweenPos.from = m_charSprite.transform.localPosition;
        m_tweenPos.to = m_tweenPos.from + (Vector3.up * 17f);       
    }

    protected override void OnStart()
    {
        m_mainMenuBtn = m_grid.GetComponentsInChildren<UIButton>();
        m_lobbymenu = m_menuObject.GetComponentsInChildren<ILobbyMenu>();
        for(int i=0; i< m_lobbymenu.Length; i++)
        {
            m_lobbymenu[i].Close(); //메뉴들을 닫아준다.
        }
        for (int i = 0; i < m_mainMenuBtn.Length; i++)
        {
            EventDelegate del = new EventDelegate(this, "OnButtonClick");
            del.parameters[0] = MakeParameter(m_mainMenuBtn[i], typeof(UIButton));
            m_mainMenuBtn[i].onClick.Add(del);
        }
    }
}
