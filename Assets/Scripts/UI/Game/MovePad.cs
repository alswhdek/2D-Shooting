using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePad : MonoBehaviour
{
    [SerializeField]
    private UISprite m_padBg,m_padButton;
    private Camera m_uiCamera;
    private bool m_isDrag;
    private float m_maxDist = 0.20f;
    private float m_maxDistPow;
    private Ray m_ray;
    private Vector3 m_dir;
    private Vector3 GetTouchMove()
    {
        RaycastHit hit;
        m_ray = m_uiCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(m_ray,out hit,10f,1<<LayerMask.NameToLayer("UI")))
        {
            if(hit.collider.transform == m_padBg.transform)
            {
                return hit.point;
            }
        }
        return Vector3.zero;
    }
    public Vector3 GetDir { get { return m_dir; } }
    void Start()
    {
        m_uiCamera = GameObject.Find("UI Root").GetComponentInChildren<Camera>();
        m_maxDistPow = Mathf.Pow(m_maxDist, 2f);
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {           
            var startPos = GetTouchMove();
            if(startPos != Vector3.zero)
            {
                m_isDrag = true;
                var dir = m_padBg.transform.position - startPos;
                if(dir.sqrMagnitude > m_maxDistPow)
                {
                    m_padButton.transform.position = m_padBg.transform.position + dir.normalized * m_maxDist;
                }
                else
                {
                    m_padButton.transform.position = m_padBg.transform.position + dir;
                }
                m_dir = dir;
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            m_isDrag = false;
            m_dir = Vector3.zero;
            m_padButton.transform.position = Vector3.zero;
        }
        if(m_isDrag)
        {
            var dir = m_uiCamera.ScreenToWorldPoint(Input.mousePosition) - m_padBg.transform.position;
            if (dir.sqrMagnitude > m_maxDistPow)
            {
                m_padButton.transform.position = m_padBg.transform.position + dir.normalized * m_maxDist;
            }
            else
            {
                m_padButton.transform.position = m_padBg.transform.position + dir;
            }
            m_dir = dir;
        }
    }
}
