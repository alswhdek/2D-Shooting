using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectPool : OBJPool<Timer> 
{
    public void SetParent(Timer effect)
    {
        effect.transform.SetParent(transform);
    }
    public enum eEffectType
    {
        None = -1,
        player,
        Enemy_0102,
        Enemy_0304,
        Enemy_0506,
        Enemy_0708,
        Enemy_0910,        
        Max
    }
}
