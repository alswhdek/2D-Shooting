using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PopupManager : DontDestroy<PopupManager>
{
    public delegate void ButtonDelegate();
    [SerializeField]
    GameObject m_popupItemStatus;

    [SerializeField]
    GameObject m_popupOkCancelPrefab;   
    
    [SerializeField]
    GameObject m_popupOkPrefab;

    [SerializeField]
    GameObject m_popupOptionPrefab; 
    
    int m_popupDepth = 1000;
    int m_popupDepthGap = 10;
    List<GameObject> m_popupList = new List<GameObject>();
    public bool IsOpened { get { return m_popupList.Count > 0 ? true : false; } }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        m_popupItemStatus = Resources.Load("Prefab/Popup/PopupOk_StoreItemStatePopup") as GameObject;
        m_popupOkCancelPrefab = Resources.Load("Prefab/Popup/PopupOkCancel") as GameObject;
        m_popupOkPrefab = Resources.Load("Prefab/Popup/PopupOk") as GameObject;
        m_popupOptionPrefab = Resources.Load("Prefab/Popup/PopupOption") as GameObject;       
    }
    /*public void OpenPopupOption()
    {
        var obj = Instantiate(m_popupOptionPrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        var panels = obj.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + m_popupList.Count * m_popupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_Option>();

        popup.SetUI();

        m_popupList.Add(obj);
    }*/
    public void OpenPopupOkCancel(string subject, string body, ButtonDelegate okBtnDel = null, ButtonDelegate cancelBtnDel = null, string okBtnStr = "Ok", string cancelBtnStr = "Cancel")
    {
        var obj = Instantiate(m_popupOkCancelPrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var panels = obj.GetComponentsInChildren<UIPanel>();
        for(int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + m_popupList.Count * m_popupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_OkCancel>();
        popup.SetUI(subject, body, okBtnDel, cancelBtnDel, okBtnStr, cancelBtnStr);

        m_popupList.Add(obj);
    } 
    public void OpenPopupOk(string subject, string body, ButtonDelegate okBtnDel = null, string okBtnStr = "Ok")
    {
        var obj = Instantiate(m_popupOkPrefab) as GameObject;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        var panels = obj.GetComponentsInChildren<UIPanel>();
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + m_popupList.Count * m_popupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_Ok>();
        popup.SetUI(subject, body, okBtnDel, okBtnStr);

        m_popupList.Add(obj);
    }
    public void OpenPopupItemStatusOk(StoreItem storeItem,int itemIndex,string itemName,string itemInformation,string itemEvent, string itemTitle)
    {
        var obj = Instantiate(m_popupItemStatus) as GameObject;
        Menu_Store.Instance.SetParentObject(obj);
        var panels = obj.GetComponentsInChildren<UIPanel>();        
        for(int i=0; i< panels.Length; i++)
        {
            panels[i].depth = m_popupDepth + m_popupList.Count * m_popupDepthGap + i;
        }
        var popup = obj.GetComponent<Popup_ItemState>();
        popup.SetItemStatus(storeItem, itemIndex, itemName, itemInformation, itemEvent, itemTitle);
        m_popupList.Add(obj);
    }
    public void ClosePopup()
    {
        if(m_popupList.Count > 0)
        {
            Destroy(m_popupList[m_popupList.Count - 1].gameObject);
            m_popupList.Remove(m_popupList[m_popupList.Count - 1]);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_popupList.Count > 0)
            {
                ClosePopup();
            }
            else
            {
                var scene = LoadSceneManager.Instance.GetCurrentScene;
                switch(scene)
                {
                    case LoadSceneManager.SceneState.Title:
                        OpenPopupOkCancel("안내", "게임을 종료하시겠습니까?", () =>
                        {
#if UNITY_EDITOR
                            EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                            ClosePopup();
                        }, null, "예", "아니오");
                        break;
                    case LoadSceneManager.SceneState.Lobby:
                        OpenPopupOkCancel("안내", "타이틀 화면으로 돌아가시겠습니까?", () =>
                        {
                            LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Title);
                            ClosePopup();
                        }, null, "예", "아니오");
                        break;
                    /*case LoadSceneManager.SceneState.Game:
                        OpenPopupOkCancel("안내", "로비 화면으로 돌아가시겠습니까?", () =>
                        {
                            LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Lobby);
                            ClosePopup();
                        }, null, "예", "아니오");
                        break;*/
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (Random.Range(1, 101) % 2 == 0)
                OpenPopupOkCancel("Notice", "안녕하세요!\r\nsbs 게임아카데미 입니다.", null, null, "확인", "취소");
            else
                OpenPopupOk("공지사항", "게임 임시점검 안내\r\n게임에서 일부 동작오류 사태가 발생하여 긴급임시점검을 진행합니다.\r\n기간 오늘 ~ 끝날때까지.");
        }
    }
}
