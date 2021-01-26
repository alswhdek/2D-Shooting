using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {       
        other.gameObject.SetActive(false);//재활용
    }
}
