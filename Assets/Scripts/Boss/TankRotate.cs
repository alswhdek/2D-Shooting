using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankRotate : MonoBehaviour
{
    private Rigidbody2D mRB;

    [SerializeField]
    private float mRotateSpeed;
    [SerializeField]
    private float mMinPos, mMaxPos;
    
    private void Awake()
    {
        mRB = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(RotatePatten());       
    }

    #region Tank Boss 자동 회전 코루틴
    private IEnumerator RotatePatten()
    {
        WaitForFixedUpdate RotateStep = new WaitForFixedUpdate();
        
        while(true)
        {
            yield return RotateStep;            
            mRB.angularVelocity = -(mRotateSpeed);           
            while (transform.rotation.eulerAngles.z > mMinPos)
            {
                yield return RotateStep;
            }
            transform.Rotate(0, 0, mRotateSpeed);
            mRB.angularVelocity = mRotateSpeed;
            while (transform.rotation.eulerAngles.z < mMaxPos)
            {
                yield return RotateStep;
            }
        }
    }
    #endregion
}
