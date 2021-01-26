using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private Rigidbody2D mRB;

    [SerializeField]
    private float mSpeed,mtime;

    [SerializeField]
    private BombEffectPool mBombEffectPool;
    private void Awake()
    {
        mRB = GetComponent<Rigidbody2D>();    
    }
    private void OnEnable()
    {
        mRB.velocity = Vector2.up * mSpeed;
        StartCoroutine(BombPatten());
    }
    #region 폭탄이 비활성화 시 폭탄 이텍트 생성
    private void OnDisable()
    {
        Timer NewBombEffect = mBombEffectPool.GetFromPool();
        NewBombEffect.GetTarget("Enemy");
        NewBombEffect.transform.position = transform.position;        
    }
    #endregion

    public void GetBombEffectPool(BombEffectPool pool)
    {
        mBombEffectPool = pool;
    }
    #region 지정한 시간뒤 폭탄 비활성화 코루틴
    private IEnumerator BombPatten()
    {
        yield return new WaitForSeconds(mtime);
        gameObject.SetActive(false);
    }
    #endregion
}
