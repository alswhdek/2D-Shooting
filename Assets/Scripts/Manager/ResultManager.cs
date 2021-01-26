using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : SingletonMonoBehaviour<ResultManager>
{
    [SerializeField]
    private UILabel m_huntScore;
    [SerializeField]
    private UILabel m_huntGold;
    [SerializeField]
    private UILabel m_bestScore;
    [SerializeField]
    private UISprite m_bestSprite;
    string lastScore;
    public void SetResult(int score,int gold)
    {
        gameObject.SetActive(true);
        m_bestSprite.gameObject.SetActive(false);
        m_huntScore.text = string.Format("사냥점수 : {0:00}", score);
        m_huntGold.text = string.Format("사냥골드 : {0:00}￦", gold);
        var defaultBestScore = PlayerDataManager.Instance.GetBestScore();
        
        if(score >= defaultBestScore)
        {
            PlayerDataManager.Instance.SetBestScore(score);
            lastScore = score.ToString();
            m_bestSprite.gameObject.SetActive(true);
        }
        else
        {
            lastScore = defaultBestScore.ToString();
            m_bestSprite.gameObject.SetActive(false);
        }
        m_bestScore.text = string.Format("사냥점수 : {0:00}", lastScore);
    }
    public void EndGame()
    {        
        SoundController.Instance.PlayBackgroundSound(SoundController.eBGMType.Lobby);        
        LoadSceneManager.Instance.LoadSceneAsync(LoadSceneManager.SceneState.Lobby);
    }
    private void Start()
    {        
        gameObject.SetActive(false);
    }
}
