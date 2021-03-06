﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Rigidbody mRB;
    [SerializeField]
    private float mSpeed, mZPos;
    [SerializeField]
    private BoltPool mBoltPool;
    [SerializeField]
    private Transform mBoltPos;
    [SerializeField]
    private float mMaxHP;
    private float mCurrentHP;
    [SerializeField]
    private GaugeBar mHPBar;
    [SerializeField]
    private ItemPool mItemPool;
    private SoundController mSoundController;
    private EffectPool mEffectPool;

    private bool mbInvinsible;

    public bool IsAlive
    {
        get { return mCurrentHP > 0; }
    }

    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
        mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        mEffectPool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
    }

    private void OnEnable()
    {
        // movement
        StartCoroutine(Movement());
        mbInvinsible = true;
        mCurrentHP = mMaxHP;
        mHPBar.SetValue(mCurrentHP, mMaxHP);
        mHPBar.gameObject.SetActive(true);
    }

    private IEnumerator AutoFire()
    {
        WaitForSeconds oneSec = new WaitForSeconds(1f);
        while (true)
        {
            yield return oneSec;
            Bolt bolt = mBoltPool.GetFromPool(1);
            bolt.SetTargetTag("Player");
            bolt.transform.position = mBoltPos.position;
            mSoundController.PlayerEffectSound((int)eSoundType.FireEnem);
        }
    }

    private IEnumerator Movement()
    {
        WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
        mRB.velocity = Vector3.back * mSpeed;
        while (transform.position.z > mZPos)
        {
            yield return fixedUpdate;
        }

        mbInvinsible = false;

        StartCoroutine(AutoFire());

        while (true)
        {
            mRB.velocity = Vector3.left * mSpeed;
            while (transform.position.x > -5.5f)
            {
                yield return fixedUpdate;
            }
            mRB.velocity = Vector3.right * mSpeed;
            while (transform.position.x < 5.5f)
            {
                yield return fixedUpdate;
            }
        }
    }

    public void Hit(int value)
    {       
        mCurrentHP -= value;
        mHPBar.SetValue(mCurrentHP, mMaxHP);

        if (mCurrentHP <= 0)
        {
            mSoundController.PlayerEffectSound((int)eSoundType.ExpPlayer);
            Timer effect = mEffectPool.GetFromPool((int)eEffecttype.Enemy);
            effect.transform.position = transform.position;

            Item item = mItemPool.GetFromPool(Random.Range(0, 3));
            float xPos = Random.Range(-2.0f, 2f);
            item.transform.position = transform.position + Vector3.right * xPos;

            item = mItemPool.GetFromPool(Random.Range(0, 3));
            xPos = Random.Range(-2.0f, 2f);
            item.transform.position = transform.position + Vector3.right * xPos;

            gameObject.SetActive(false);
            mHPBar.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("Hit", 5);
        }
    }
}
            
            