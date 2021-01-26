using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : OBJPool<Enemy>
{
    [SerializeField]
    private EnemyBoltPool mEnemyBoltPool;
    [SerializeField]
    private EffectPool mEffectPool;
    [SerializeField]
    private ItemPool mItemPool;
    [SerializeField]
    private GoldPool mGoldPool;
    
    public void SetParent(Enemy enemy)
    {
        enemy.transform.SetParent(transform);
    }

    #region Enemy생성과 Enemy안에 내장되어있는 Pool 접근
    protected override Enemy MakerOjbect(int id = 0)
    {
        Enemy newEnemy = Instantiate(mOrgen[id]);
        mPool[id].Add(newEnemy);
        newEnemy.SetBoltPool(mEnemyBoltPool);
        newEnemy.SetEffectPool(mEffectPool);
        newEnemy.SetItemPool(mItemPool);
        newEnemy.SetGoldPool(mGoldPool);
        return newEnemy;
    }
    #endregion
}
