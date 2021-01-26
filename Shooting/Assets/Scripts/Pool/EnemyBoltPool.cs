using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoltPool : MonoBehaviour
{
    [SerializeField]
    private Bolt mPrefabBolt;

    private List<Bolt> mPool;
   
    // Start is called before the first frame update
    void Start()
    {
        mPool = new List<Bolt>();       
    }
    public Bolt GetFromEnemyBolt()
    {
        for(int i=0; i<mPool.Count; i++)
        {
            if(!mPool[i].gameObject.activeInHierarchy)
            {
                mPool[i].gameObject.SetActive(true);
                return mPool[i];
            }
        }
        Bolt newBolt = Instantiate(mPrefabBolt);
        mPool.Add(newBolt);
        return newBolt;
    }   
}
