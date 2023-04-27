using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;

    private void Start()
    {
        Bird.GetInstance().onDied += Bird_OnDied;
        Hide();
    }
    private void Bird_OnDied(object sender,System.EventArgs e)
    {
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
        if (Level.GetInstance().GetPipePassedCount() >= Score.GetHighScore())
        {
            highScoreText.text = "new highscore";
        }
        else
        {
            highScoreText.text ="highscore: "+ Score.GetHighScore().ToString();
        }
        Show();
    }
    public void ScoreText()
    {
        
    }
    public void RetryFun()
    {
        SceneManager.LoadScene("GameScene");
        Hide();
    }
    public void MenuFun()
    {
        SceneManager.LoadScene(2);
        Hide();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
   
}
