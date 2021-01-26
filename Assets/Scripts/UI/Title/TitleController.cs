using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_bgObject,m_titleCI,m_titleObject;

    public void SetObject()
    {
        m_titleCI.gameObject.SetActive(false);
        m_bgObject.gameObject.SetActive(true);
        m_titleObject.gameObject.SetActive(true);

        LoadSceneManager.Instance.GetCurrentScene = LoadSceneManager.SceneState.Title;
        SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Lobby);
    }

    private void Start()
    {
        m_bgObject.gameObject.SetActive(false);
        m_titleObject.gameObject.SetActive(false);          
    }

    private void Update()
    {
        if(Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape))
        {
            if(LoadSceneManager.Instance.GetCurrentScene == LoadSceneManager.SceneState.Title)
            {
                LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Lobby);
            }
        }
    }
}
