using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float mSpeed,mTilt;

    [SerializeField]
    private float mXMax, mXMin, mZMax, mZMin;

    private Rigidbody mRB;

    [Header("HP")]
    [SerializeField]
    private float mMaxHP;
    private float mCurrentHP;
    [SerializeField]
    private UIController mUIController;

    [Header("Fuel")]
    [SerializeField]
    private GaugeBar mFuelGauge;
    private float mFuel;//현재 연료게이지
    [SerializeField]
    private float mMaxFuel, mFuelSpend;

    [Header("Fire & Overheat")]
    [SerializeField]
    private float mFireRate;
    [SerializeField]
    private float mOverHeatMax,mOverHeatWeight,mCooldownWeight;//Weight : 계수 올라가는값 , CollDown : 과열되는거 식히는 값(프레임 단위로 깎인다.)
    private float mCurrentFireRate,mCurrentHeat;
    [SerializeField]
    private GaugeBar mOverHeatGauge;
    [SerializeField]
    private BoltPool mPool;
    [SerializeField]
    private Transform mBoltPos;
    [SerializeField]
    private float mBoltGap;//총알 간격
    [SerializeField]
    private int mBoltCount =1;
    [SerializeField]
    private bool mSupporterFlag;
    [SerializeField]
    private GameObject[] mSupporterArr;
    [SerializeField]
    private Transform[] mSuppterBoltPosArr;

    [Header("Bomb")]
    [SerializeField]
    private BombPool mBombPool;
    [SerializeField]
    private int mBombCount;

    private EffectPool mEffectpool;

    private SoundController mSoundController;

    private GameController mGameControl;

    // Start is called before the first frame update
    void Awake()
    {        
        mRB = GetComponent<Rigidbody>();              
    }

    private void OnEnable()
    {
        mCurrentHP = mMaxHP;
        mUIController.ShowPlayerHP(mCurrentHP, mMaxHP);

        mCurrentFireRate = 0;
        mBoltCount = 1;

        mSupporterFlag = false;
        for(int i=0; i<mSupporterArr.Length; i++)
        {
            mSupporterArr[i].gameObject.SetActive(false);
        }

        mFuel = mMaxFuel;
        mFuelGauge.SetValue(mFuel, mMaxFuel);

        mOverHeatGauge.SetValue(mCurrentHeat, mOverHeatMax);//실시간으로 게이지의 UI 노출
        Color color = new Color(1, 1 - mCurrentHeat / mOverHeatMax * .8f, 0, 1);//게이지 UI Color 변환
        mOverHeatGauge.SetColor(color);
       
    }

    private void Start()//외부 컴포넌트는 Start에서 불러온다.
    {
        mSoundController = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundController>();
        mGameControl = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Hit(float value)
    {
        mCurrentHP -= value;
        mUIController.ShowPlayerHP(mCurrentHP, mMaxHP);        
        if(mCurrentHP <= 0)
        {
            if (mEffectpool == null)
            {
                mEffectpool = GameObject.FindGameObjectWithTag("EffectPool").GetComponent<EffectPool>();
            }
            Timer effect = mEffectpool.GetFromPool((int)eEffecttype.Player);
            effect.transform.position = transform.position;

            mSoundController.PlayerEffectSound((int)eSoundType.ExpPlayer);

            gameObject.SetActive(false);
            mGameControl.GameOver();           
        }
    }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical);
        mFuel -= dir.magnitude * Time.deltaTime;//Vector 크기를 가져온다.
        mRB.velocity =  dir.normalized * mSpeed;
        transform.rotation = Quaternion.Euler(0, 0, mTilt * -horizontal);
        //전투기가 맵을 이탈하지 않도록 한다.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,mXMin,mXMax), 
                                                    transform.position.y, 
                                                   Mathf.Clamp(transform.position.z,mZMin,mZMax));
        #region Fire
        float mHeatmax = mOverHeatMax * .8f;//80%이상이 차면 총을 발싸할수가없다.
        if(mCurrentHeat <= mHeatmax && 0 >= mCurrentFireRate && Input.GetButtonDown("Fire1"))
        {
            Fire();
            mCurrentHeat += mOverHeatWeight;//UI
            
            mCurrentFireRate = mFireRate;            
        }
        mCurrentFireRate -= Time.deltaTime;
        if(mCurrentHeat >= 0)
        {
            mCurrentHeat -= mCooldownWeight * Time.deltaTime;//열을 식혀주고
            mOverHeatGauge.SetValue(mCurrentHeat, mOverHeatMax);//실시간으로 게이지의 UI 노출
            Color color = new Color(1, 1 - mCurrentHeat / mHeatmax, 0, 1);//게이지 UI Color 변환
            mOverHeatGauge.SetColor(color);
        }
        #endregion
        if (mBombCount >0 && Input.GetKeyDown(KeyCode.Space))
        {
            Bomb bomb = mBombPool.GetFromPool();
            bomb.transform.position = mBoltPos.position;
            mBombCount--;
        }

        mFuel -= mFuelSpend * Time.deltaTime;
        mFuelGauge.SetValue(mFuel, mMaxFuel); 
        if(mFuel <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void GetItem(eItemType type)
    {
        switch(type)
        {
            case eItemType.Bolt:
                mBoltCount++;
                break;
            case eItemType.Supporter:
                mSupporterFlag = true;
                for (int i = 0; i < mSupporterArr.Length; i++)
                {
                    mSupporterArr[i].SetActive(true);
                }
                break;
            case eItemType.Fuel:
                mFuel += 5f;
                break;
            default:
                Debug.LogError("Wrong item type");
                break;
        }
    }

    private void Fire()
    {
        float startX = (1 - mBoltCount) / 2f * mBoltGap;
        Vector3 pos = mBoltPos.position;
        pos.x += startX;
        for(int i=0; i<mBoltCount; i++)
        {
            Bolt newBolt = mPool.GetFromPool();
            newBolt.SetTargetTag("Enemy");
            newBolt.transform.position = pos;
            pos.x += mBoltGap;           
        }
        mSoundController.PlayerEffectSound((int)eSoundType.FirePlayer);

        if(mSupporterFlag)
        {
            for(int i=0; i<mSuppterBoltPosArr.Length; i++)
            {
                Bolt newBolt = mPool.GetFromPool();
                newBolt.SetTargetTag("Enemy");
                newBolt.transform.position = mSuppterBoltPosArr[i].position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SendMessage("Hit", 3);               
        }
    }
}
