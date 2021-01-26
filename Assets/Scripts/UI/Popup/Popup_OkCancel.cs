using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_OkCancel : MonoBehaviour
{
    [SerializeField]
    UILabel m_subject;
    [SerializeField]
    UILabel m_body;
    [SerializeField]
    UILabel m_okBtnStr;
    [SerializeField]
    UILabel m_cancelBtnStr;
    PopupManager.ButtonDelegate m_okBtnDel;
    PopupManager.ButtonDelegate m_cancelBtnDel;
    public void SetUI(string subject, string body, PopupManager.ButtonDelegate okBtnDel = null, PopupManager.ButtonDelegate cancelBtnDel = null, string okBtnStr = "Ok", string cancelBtnStr = "Cancel")
    {
        m_subject.text = subject;
        m_body.text = body;
        m_okBtnStr.text = okBtnStr;
        m_cancelBtnStr.text = cancelBtnStr;
        m_okBtnDel = okBtnDel;
        m_cancelBtnDel = cancelBtnDel;
    }
    public void OnPressOk()
    {
        if(m_okBtnDel != null)
        {
            m_okBtnDel();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
    public void OnPressCancel()
    {
        if (m_cancelBtnDel != null)
        {
            m_cancelBtnDel();
        }
        else
        {
            PopupManager.Instance.ClosePopup();
        }
    }
    public void OnClose()
    {
        PopupManager.Instance.ClosePopup();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
