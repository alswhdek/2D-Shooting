using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPool : OBJPool<Gold>
{  
    public void SetParent(Gold gold)
    {
        gold.transform.SetParent(transform);
    }
}
