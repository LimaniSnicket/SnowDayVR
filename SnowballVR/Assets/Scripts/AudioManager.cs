using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private static AudioManager audioManager;

    private void Start()
    {
        if (audioManager == null) { audioManager = this; } else { Destroy(this); }
        SnowballBehavior.SnowballHit += OnSnowballHit;
    }

    void OnSnowballHit(float snowBallSize, float pointsAdded)
    {
        //snowball size corresponding to volume of SFX, points added corresponding to tone/pitch of SFX?
    }

    private void OnDestroy()
    {
        SnowballBehavior.SnowballHit -= OnSnowballHit;
    }
}
