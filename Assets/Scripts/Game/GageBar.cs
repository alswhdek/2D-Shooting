using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GageBar : MonoBehaviour
{
    [SerializeField]
    private Image mGage;
    public void GetGage(float currentHp,float MaxHp)
    {
        mGage.fillAmount = currentHp / MaxHp;
    }
}
