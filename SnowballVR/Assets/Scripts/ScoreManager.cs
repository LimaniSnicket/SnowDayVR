using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    static ScoreManager scoreManager;
    public static int CurrentScore;

    private void Awake()
    {
        if (scoreManager == null) { scoreManager = this; } else { Destroy(this); }
    }

    private void Update()
    {
        if (CurrentScore > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", CurrentScore);
        }
    }
}
