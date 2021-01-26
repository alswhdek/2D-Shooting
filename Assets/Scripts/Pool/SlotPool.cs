using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPool : OBJPool<StoreSlot>
{
    public void SetParent(StoreSlot slot)
    {
        slot.transform.SetParent(transform);
        //slot.transform.localPosition = pos;
        slot.transform.localScale = Vector3.one;
    }
}
