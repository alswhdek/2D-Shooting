using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 0.1f;
    private float m_speedScale = 1f;
    SpriteRenderer m_sprRen;
    Material m_material;
    public void SetSpeedScale(float scale)
    {
        m_speedScale = scale;
    }
    private void Start()
    {
        m_sprRen = GetComponent<SpriteRenderer>();
        m_material = m_sprRen.material;
    }
    private void Update()
    {
        m_material.mainTextureOffset += Vector2.up * m_speed * m_speedScale * Time.deltaTime;
    }
}
