using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : SingletonMonoBehaviour<MonsterManager>
{   
    public enum eEnemyType
    {
        None = -1,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Max
    }
    [SerializeField]
    private HudPool m_hudPool;
    [SerializeField]
    private EnemyPool m_enemyPool;
    private eEnemyType type;
    public void CreateEnemy(eEnemyType type)
    {
        Enemy newEnemy = m_enemyPool.GetFromPool();      
        m_enemyPool.SetParent(newEnemy);
        newEnemy.SetEnemy(type);
        newEnemy.transform.position = new Vector2(Random.Range(-5.7f, 5.7f), 22f);
        newEnemy.transform.rotation = Quaternion.Euler(0, 0, 180f);      
        //StartCoroutine(Coroutine_SetAcive(hud.gameObject, true));
    }

}
