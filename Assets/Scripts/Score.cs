using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score 
{
    public static void Start()
    {
        ResetHighScore();
        Bird.GetInstance().onDied += Bird_OnDied;
    }
    private static void Bird_OnDied(object sender,System.EventArgs e)
    {
        TrySetNewHighScore(Level.GetInstance().GetPipePassedCount());
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }
    public static bool TrySetNewHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetInt("highscore"));
    }
}
