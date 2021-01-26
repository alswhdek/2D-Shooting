using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyBoltPool : OBJPool<EnemyBolt>
{
    public enum eEnemyBoltType
    {
        None=-1,
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
        OneBoss,
        OneBossSkill,
        TwoBoss,
        ThreeBoss,
        FiveBossFire,
        FiveBossIce,
        Max,
    }
    public void SetParentPos(EnemyBolt bolt)
    {
        bolt.transform.SetParent(transform);
    }
}
