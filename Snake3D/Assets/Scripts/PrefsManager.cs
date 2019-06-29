using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefsManager
{
    private const string score = "Score";

    public PrefsManager() { }

    public void SaveHighScore(int highScore)
    {
        if(highScore > GetHighScore())
        {
            PlayerPrefs.SetInt(score, highScore);
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(score, 0);
    }


}
