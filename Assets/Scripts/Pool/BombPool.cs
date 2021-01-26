using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPool : OBJPool<Bomb>
{
    [SerializeField]
    private BombEffectPool mBombEffectPool;

    /*protected override Bomb MakerOjbect(int id = 0)
    {
        Bomb newBomb = Instantiate(mOrgen[id]);
        mPool[id].Add(newBomb);
        newBomb.GetBombEffectPool(mBombEffectPool);
        return newBomb;
    }*/
}
