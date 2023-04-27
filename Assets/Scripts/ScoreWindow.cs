using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    public Text scoreText;
    public Text highScoreText;
    private void Awake()
    {
        //scoreText = transform.Find("ScoreText").GetComponent<Text>();
    }
    private void Start()
    {
        highScoreText.text = "highscore: " + Score.GetHighScore().ToString();
    }
    private void Update()
    {
        scoreText.text = Level.GetInstance().GetPipePassedCount().ToString();
       
    }
}
