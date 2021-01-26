using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJPool<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    protected T[] mOrgin;
    protected List<T>[] mPool;
    // Start is called before the first frame update
    void Start()
    {
        mPool = new List<T>[mOrgin.Length];
        for(int i=0; i<mPool.Length; i++)
        {
            mPool[i] = new List<T>();
        }
    }
    public T GetFromPool(int id = 0)
    {
        for(int i=0; i<mPool[id].Count; i++)
        {
            if(!mPool[id][i].gameObject.activeInHierarchy)
            {
                mPool[id][i].gameObject.SetActive(true);
                return mPool[id][i];
            }
        }
        // ex)여러개이 Enemy가 총알을 발사해야되기 때문에 
        return MakeNewInstance(id);
    }
    protected virtual T MakeNewInstance(int id)
    {
        T newObj = Instantiate(mOrgin[id]);
        mPool[id].Add(newObj);
        return newObj;
    }

}
