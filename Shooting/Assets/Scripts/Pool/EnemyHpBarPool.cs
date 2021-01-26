using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBarPool : OBJPool<GaugeBar>
{
    [SerializeField]
    private Transform mCanvas;
    protected override GaugeBar MakeNewInstance(int id)
    {
        GaugeBar newObj = Instantiate(mOrgin[id], mCanvas);//원하는 위치에 만들어진다.
        mPool[id].Add(newObj);
        return newObj;
    }
}
