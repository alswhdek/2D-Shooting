using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool<T> where T : class
{
    public delegate T CreateFuncDel();
    CreateFuncDel m_createFunc;
    Queue<T> m_objectQueue = new Queue<T>();
    int m_count;
    public GameObjectPool(int count, CreateFuncDel createFunc)
    {
        m_count = count;
        m_createFunc = createFunc;
        Allocation();
    }
    void Allocation()
    {
        for (int i = 0; i < m_count; i++)
        {
            var obj = m_createFunc();
            m_objectQueue.Enqueue(obj);
        }
    }
    public T Get()
    {
        if (m_objectQueue.Count > 0)
        {
            return m_objectQueue.Dequeue();
        }
        else
        {
            var obj = m_createFunc();
            return obj;
        }
    }
    public void Set(T obj)
    {
        m_objectQueue.Enqueue(obj);
    }
}
