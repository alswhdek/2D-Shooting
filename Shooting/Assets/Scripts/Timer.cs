using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float time;
    private void OnEnable()
    {
        StartCoroutine(Countdown());        
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(time);//5초뒤에 꺼져라
        gameObject.SetActive(false);
    }
}
