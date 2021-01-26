using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : SingletonMonoBehaviour<BuffManager>
{
    [SerializeField]
    private UIGrid m_gridObj;
    [SerializeField]
    private BuffPool m_buffPool;
    private PlayerController m_player;
    Dictionary<ItemType, BuffUI> m_dicBuffUi = new Dictionary<ItemType, BuffUI>();
    private Enemy m_enemy;
    private IEnumerator Coroutine_BuffMode(BuffUI buffui)
    {
        ItemData itemData;
        if(!ItemManager.Instance.m_dicItem.TryGetValue(buffui.GetItemType,out itemData))
        {
            Debug.LogError("타입이 존재하지않습니다.");
            yield break;
        }
        while (true)
        {           
            if(!buffui.IsBuff) //버프 적용이끝났으면
            {
                switch(buffui.GetItemType)
                {
                    case ItemType.Shadow:
                        m_player.EndShadow();
                        break;
                    case ItemType.invincibility:
                        m_player.Endinvincibility();
                        BgController.Instance.SetSpeedScale(3f);                        
                        break;
                    case ItemType.Magnet:
                        m_player.EndMarent();
                        break;
                    case ItemType.Speed:
                        m_player.EndSpeed(itemData.m_speed);
                        break;
                    case ItemType.ResetHp: // 필요 X
                        break;
                    case ItemType.Gold: // 필요 X
                        break;
                    case ItemType.Attack:
                        m_player.endAttack(itemData.m_attack);
                        break;
                }
                m_dicBuffUi.Remove(buffui.GetItemType);
                buffui.gameObject.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
    public void SetEnemyObject(Enemy enemy)
    {
        m_enemy = enemy;
    }
    public void CreateBuffUI(ItemType type, float duration, int iconIndex)
    {
        BuffUI buffui;
        if (m_dicBuffUi.TryGetValue(type, out buffui))
        {
            buffui.ResetTime();
        }
        else
        {
            buffui = m_buffPool.GetFromPool();
            buffui.SetUI(type, duration, iconIndex);
            buffui.transform.SetParent(m_gridObj.transform);
            buffui.transform.localScale = Vector3.one;
            m_gridObj.Reposition();
            m_dicBuffUi.Add(type, buffui);
            StartCoroutine(Coroutine_BuffMode(buffui));
        }
    }
    protected override void OnStart()
    {
        m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
}
