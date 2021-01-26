using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoltPool : OBJPool<Bolt>
{
    public enum eBoltType
    {
        None = -1,
        Player01_01,
        Player01_02,
        Player01_03,
        Player01_04,
        Player01_05,
        Player02_01,
        Player02_02,
        Player02_03,
        Player02_04,
        Player02_05,
        Player03_01,
        Player03_02,
        Player03_03,
        Player03_04,
        Player03_05,
        Max
    }
    public void SetParent(Bolt bolt)
    {
        bolt.transform.SetParent(transform);
    }
}
