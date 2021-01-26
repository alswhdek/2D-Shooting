using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_Ok : MonoBehaviour
{
    [SerializeField]
    UILabel m_subject;
    [SerializeField]
    UILabel m_body;
    [SerializeField]
    UILabel m_okBtnStr;   
    PopupManager.ButtonDelegate m_okBtnDel;    
    public void SetUI(string subject, string body, PopupManager.ButtonDelegate okBtnDel = null, string okBtnStr = "Ok")
    {
        m_subject.text = subject;
        m_body.text = body;
        m_okBtnStr.text = okBtnStr;
        m_okBtnDel = okBtnDel;
    }
    public void OnPressOk()
    {
        if (m_okBtnDel != null)
        {
            m_okBtnDel();
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
}
