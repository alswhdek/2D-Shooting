﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    [SerializeField]
    private float mSpeed;
    private Rigidbody mRB;
    private Vector3 mMoveDistance;
    // Start is called before the first frame update
    void Start()
    {
        mRB = GetComponent<Rigidbody>();
        mRB.velocity = Vector3.back * mSpeed;
        //배경1,2 사이 거리 * 2
        mMoveDistance = new Vector3(0, 0, 20.47f * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Bumper"))
        {
            transform.position += mMoveDistance;
        }
    }

    public void StopBG(Vector3 Move)
    {
        mRB.velocity = Move;
    }
}
