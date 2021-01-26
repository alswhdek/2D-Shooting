using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    private SpriteRenderer m_sprite;
    // Start is called before the first frame update
    void Awake()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_sprite.sprite = Resources.Load<Sprite>(string.Format("Images/Player/Player_{0:00}", PlayerDataManager.Instance.GetPlayerIndex() + 1));
        if(PlayerDataManager.Instance.GetPlayerIndex() == 1)
        {
            m_sprite.flipY = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
