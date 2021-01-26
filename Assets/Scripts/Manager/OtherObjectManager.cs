using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherObjectManager : SingletonMonoBehaviour<OtherObjectManager>
{
    [SerializeField]
    private BgController m_bgController;

    public void FastBakground(float speed)
    {
        m_bgController.SetSpeedScale(speed);
    }
}
