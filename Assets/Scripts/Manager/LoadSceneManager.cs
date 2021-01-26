using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneManager : DontDestroy<LoadSceneManager>
{
    public enum SceneState
    {
        None = -1,
        Title,
        Lobby,
        Game,
        Max
    }
    SceneState m_state = SceneState.None;
    SceneState m_loadState = SceneState.None;
    AsyncOperation m_loadingInfo;
    public SceneState GetCurrentScene { get { return m_state; } set { m_state = value; } }

    #region 씬 이동
    public void LoadSceneAsync(SceneState sceneState)
    {
        if (m_loadState != SceneState.None) return;
        m_loadState = sceneState;
        m_loadingInfo = SceneManager.LoadSceneAsync(sceneState.ToString());
    }
    #endregion
    private void Update()
    {
        if(m_loadState != SceneState.None && m_loadingInfo != null)
        {
            if(m_loadingInfo.isDone)
            {
                m_state = m_loadState;
                m_loadState = SceneState.None;
                m_loadingInfo = null;
            }    
        }
    }
}
