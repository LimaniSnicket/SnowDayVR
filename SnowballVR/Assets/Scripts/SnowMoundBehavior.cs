using UnityEngine;
using System.Collections;

public class SnowMoundBehavior : MonoBehaviour
{
    public float availableSnow;

    public void Start()
    {
        Snowfall.GameTimerDiminished += OnGameTimerDiminished;
    }

    private void OnGameTimerDiminished()
    {
        availableSnow = 0f;
    }

    private void OnDestroy()
    {
        Snowfall.GameTimerDiminished -= OnGameTimerDiminished;
    }
}
