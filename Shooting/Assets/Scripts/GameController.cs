using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private float mScore;
    [SerializeField]
    private UIController mUIControl;
    [SerializeField]
    private PlayerController mPlayer;
    [SerializeField]
    private int mStartLife;
    private int mCurrentLife;
    private bool mbRestart;
    [SerializeField]
    private ItemPool mItemPool;

    [Header("Hazard")]
    [SerializeField]
    private AsteroidPool mAstPool;
    [SerializeField]
    private EnemyPool mEnemyPool;
    [SerializeField]
    private Boss mBoss;
    [SerializeField]
    private float mPeriod;
    [SerializeField]
    private int mASTSpawnCount ,mEnemySpawnCount;
    private Coroutine mHazardRountine;
    private int mRoundCount;
    private float mCountdonw;

    private BGScroller mBGScroller;
    // Start is called before the first frame update
    void Start()
    {        
        mbRestart = false;
        mScore = 0;
        mUIControl.ShowScore(mScore);
        mCountdonw = 0;       
        mCountdonw = mPeriod;
        mCurrentLife = mStartLife;
        mHazardRountine = StartCoroutine(SpwanHazard());
        mBGScroller = GameObject.FindGameObjectWithTag("BG").GetComponent<BGScroller>();
        mRoundCount = 1;
    }

    public void AddScroe(float amount)
    {
        mScore += amount;
        mUIControl.ShowScore(mScore);       
    }

    public void GameOver()
    {
        mCurrentLife--;
        mUIControl.LooseLife(mCurrentLife);
        if(mCurrentLife > 0)
        {
            mPlayer.transform.position = Vector3.zero;
            mPlayer.gameObject.SetActive(true);
            return;//동작을 시키지않는다.
        }
        mbRestart = true;
        mUIControl.ShowState("Game Over!");
        mUIControl.ShowRestartText(true);
        StopCoroutine(mHazardRountine);        
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
        /*mPlayer.gameObject.SetActive(true);
        mPlayer.transform.position = Vector3.zero;

        mScore = 0;
        mUIControl.ShowScore(mScore);

        mHazardRountine = StartCoroutine(SpwanHazard());

        mUIControl.ShowRestartText(false);
        mUIControl.ShowState("");
        mbRestart = false;*/
    }

    private void Update()
    {
        if (mbRestart && Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }       
    }

    private IEnumerator SpwanHazard()
    {
        WaitForSeconds pointFive = new WaitForSeconds(.5f);//메모리 누수 방지를 위해 
        WaitForSeconds period = new WaitForSeconds(mPeriod);//메모리 누수 방지를 위해
        int currentAST, currentEnemy;
        float AstRate;
        while (true)
        {
            yield return mPeriod;
            currentAST = mASTSpawnCount + mRoundCount * (mRoundCount+1)/2;//n(n+1)/2
            currentEnemy = mEnemySpawnCount + mRoundCount/2;//만들어야될 Enemy 갯수
            AstRate = (float)currentAST / (currentAST + currentEnemy); //운석나오는 확률(총합/운석갯수) -> 강제형 float로 기재
            
            while(currentAST > 0 && currentEnemy > 0)
            {
                float rate = Random.Range(0, 1f);
                if (rate < AstRate) //ex) 7:3 비율로 설정하면(적7,운석3) AstRate는 0.7확률
                {
                    Astcroid ast = mAstPool.GetFromPool(Random.Range(0, 3));
                    ast.transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0, 16);
                    currentAST--;
                }
                else //적 생성
                {
                    Enemy enemy = mEnemyPool.GetFromPool();
                    enemy.transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0, 16);
                    currentEnemy--;
                }
                yield return pointFive;
            }
            //ex) Enemy가 0이면 운석만 나와라
            for(int i=0; i<currentAST; i++)
            {
                Astcroid ast = mAstPool.GetFromPool(Random.Range(0,3));
                ast.transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0, 16);
                yield return pointFive;
            }
            for (int i = 0; i < currentEnemy; i++)
            {
                Enemy enemy = mEnemyPool.GetFromPool();
                enemy.transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0, 16);
                yield return pointFive;
            }
            
            mRoundCount++;
            Item item = mItemPool.GetFromPool(Random.Range(2, 3));
            item.transform.position = new Vector3(Random.Range(-5.5f, 5.5f), 0, 16);

            if(mRoundCount % 5 == 0)
            {
                mBoss.transform.position = new Vector3(0, 0, 20f);
                mBoss.gameObject.SetActive(true);
                if (mBoss.IsAlive)
                {
                    yield return pointFive; // 다음 루프를 실행하지말고 0.5초간 대기해
                }
            }            
        }
    }   
}
