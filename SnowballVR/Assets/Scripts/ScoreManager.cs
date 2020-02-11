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
       VehicleBehavior.SnowballHit += UpdateScore;
    }

    void UpdateScore(float snowballSize, int carModifier)
    {
        CurrentScore += Mathf.FloorToInt(snowballSize * carModifier);
        if (CurrentScore > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", CurrentScore);
        }
    }

    private void OnDestroy()
    {
        VehicleBehavior.SnowballHit -= UpdateScore;
    }
}
