using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float mSpeed,mColDamage;
    private Rigidbody mRB;

    [SerializeField]
    private Transform mHPBarPos;
    [SerializeField]
    private GaugeBar mHPBar;
    [SerializeField]
    private float mMaxHP;
    private float mCurrentHP;

    [SerializeField]
    private BoltPool mBoltPool;
    [SerializeField]
    private Transform mBoltPos;
    
    private EffectPool mEffectpool;

    private SoundController mSoundController;

    private GameController mGameController;
    private void Awake()
    {
        mRB = GetComponent<Rigidbody>();
        mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        mGameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        mRB.velocity = Vector3.back * mSpeed;
        mCurrentHP = mMaxHP;
        StartCoroutine(MovePattern()); //코루틴은 SetActive(false)경우 비활성화 처리된다.
        StartCoroutine(AutoFire());               
    }

    public void SetBoltPool(BoltPool pool)
    {
        mBoltPool = pool;
    }

    public void Hit(float value)
    {
        mCurrentHP -= value;
        mHPBar = mBoltPool.EnemyHPBarPool.GetFromPool();
        mHPBar.transform.position = mHPBarPos.position;
        if(mCurrentHP <= 0)
        {
            if (mEffectpool == null)
            {
                mEffectpool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
            }
            Timer effect = mEffectpool.GetFromPool((int)eEffecttype.Enemy);
            effect.transform.position = transform.position;

            mSoundController.PlayerEffectSound((int)eSoundType.ExpEnem);

            mGameController.AddScroe(10f);

            mHPBar.gameObject.SetActive(false);
            //죽으면 소유권을 박탈시켜야된다.
            mHPBar = null;
            gameObject.SetActive(false);
            
        }
    }

    private void Update()
    {
        if(mHPBar != null)
        {
            mHPBar.transform.position = mHPBarPos.position;
        }
    }

    private IEnumerator AutoFire()
    {
        WaitForSeconds fireRate = new WaitForSeconds(.6f);
        while (true)
        {
            yield return fireRate;
            Bolt newBolt = mBoltPool.GetFromPool();
            newBolt.SetTargetTag("Player");
            newBolt.transform.position = mBoltPos.position;            
            newBolt.transform.rotation = mBoltPos.rotation;
            mSoundController.PlayerEffectSound((int)eSoundType.FireEnem);
        }
    }
    private IEnumerator MovePattern()//랜덤 무한루프는 변수 지정해서 안된다.
    {
        while (true)
        {
            // vel = (0,0,mSpeed)
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
            if (transform.position.x < 0)//왼쪽에 있을경우 
            {
                mRB.velocity += Vector3.right * Random.Range(2, 5f);//오른쪽으로 이동 * 속도
            }
            else
            {
                mRB.velocity += Vector3.left * Random.Range(2, 5f);//왼쪽으로 이동 * 속도
            }
            // vel = (x,0,mSpeed)
            //잠시 x좌표로 움직이지 않는다.
            yield return new WaitForSeconds(Random.Range(.5f, 1.5f));
            Vector3 oriVel = mRB.velocity; // oriVel = (x,0,mSpeed)
            oriVel.x = 0; // oriVel = (0,0,mSpeed)
            mRB.velocity = oriVel;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SendMessage("Hit", mColDamage);
        }    
    }
}
