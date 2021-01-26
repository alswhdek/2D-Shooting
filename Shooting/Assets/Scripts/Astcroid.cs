using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astcroid : MonoBehaviour
{
    private Rigidbody mRB;

    [SerializeField]
    private float mSpeed, mTorque, mColDamage;

    [SerializeField]
    private float mMaxHP;
    private float mCurrentHP;

    private EffectPool mEffectpool;

    private SoundController mSoundController;

    private GameController mGameController;
    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
        mRB.velocity = Vector3.back * mSpeed;
        mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        mGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    //다시 재활성화 되었을때 회전값 랜덤 적용
    private void OnEnable()
    {
        mRB.angularVelocity = Random.onUnitSphere * mTorque;
        mCurrentHP = mMaxHP;
    }

    public void Hit(float value)
    {
        mCurrentHP -= value;
        if (mCurrentHP <= 0)
        {
            if (mEffectpool == null)
            {
                mEffectpool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
            }
            Timer effect = mEffectpool.GetFromPool((int)eEffecttype.Enemy);
            effect.transform.position = transform.position;

            if (mSoundController == null)
            {
                mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
            }
            mSoundController.PlayerEffectSound((int)eSoundType.ExpEnem);

            mGameController.AddScroe(10f);

            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("Hit", mColDamage);
        }
    }
}

            
           
