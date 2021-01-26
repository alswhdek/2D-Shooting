using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJPool<T> : MonoBehaviour where T : Component
{
    [SerializeField]
    protected T[] mOrgen;
    protected List<T>[] mPool;
        
    void Start()
    {
        mPool = new List<T>[mOrgen.Length];

        for(int i=0; i< mPool.Length; i++)
        {
            mPool[i] = new List<T>();
        }
    }

    public T GetFromPool(int id=0)
    {
        for(int i=0; i<mPool[id].Count; i++)
        {
            if(!mPool[id][i].gameObject.activeInHierarchy)
            {
                mPool[id][i].gameObject.SetActive(true);
                return mPool[id][i];
            }
        }
        return MakerOjbect(id);
    }

    protected virtual T MakerOjbect(int id=0)
    {
        T NewObj = Instantiate(mOrgen[id]);
        mPool[id].Add(NewObj);
        return NewObj;
    }
}
