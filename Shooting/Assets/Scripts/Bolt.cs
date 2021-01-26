using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    private Rigidbody mRB;
    [SerializeField]
    private float mSpeed, mPower;
    private string mTargetTag;
    // Start is called before the first frame update
    void Awake()
    {
        mRB = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        mRB.velocity = transform.forward * mSpeed;
    }

    public void SetTargetTag(string tag)//검출해야되는 대상
    {
        mTargetTag = tag;
    }

    //타켓포인트를 넘겨받으면 그방향으로 속도를 준다.
    public void SetTarget(Vector3 TargetPoint)
    {
        //dir = 플레이어위치 - 총알위치
        Vector3 dir = TargetPoint - transform.position;
        mRB.velocity = dir.normalized * mSpeed;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(mTargetTag))//총알이 부딪힌 대상
        {
            other.gameObject.SendMessage("Hit", mPower);
            gameObject.SetActive(false);
        }
    }
}
