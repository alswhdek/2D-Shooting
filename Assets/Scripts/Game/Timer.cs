using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float mtime;

    private string mTargetTag;

    public void GetTarget(string tag)
    {
        mTargetTag = tag;
    }
    private void OnEnable()
    {
        StartCoroutine(EffectEndTime());
    }
    public void SetParent(EffectPool pool)
    {
        pool.SetParent(this);
    }
    private IEnumerator EffectEndTime()
    {
        yield return new WaitForSeconds(mtime);
        gameObject.SetActive(false);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag(mTargetTag))
        {
            collision.gameObject.SendMessage("Hit",20);            
        }
    }*/
}
