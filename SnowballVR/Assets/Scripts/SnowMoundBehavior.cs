using UnityEngine;
using System.Collections;

public class SnowMoundBehavior : MonoBehaviour
{
    public float availableSnow;

    public void Start()
    {
        Snowfall.GameTimerDiminished += OnGameTimerDiminished;
        transform.localScale = new Vector3(2, 0, 2);
        StartCoroutine(AccumulateSnow());
    }

    private IEnumerator AccumulateSnow()
    {
        yield return new WaitForSecondsRealtime(.1f);
        while (Snowfall.Snowing())
        {
            availableSnow += Snowfall.RateOfIncrease;
            Vector3 lerpTo = transform.localScale + Vector3.up * Snowfall.RateOfIncrease;
            while(!transform.localScale.Approximately(lerpTo))
            {
                transform.localScale = Vector3.Lerp(transform.localScale, lerpTo, Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSecondsRealtime(1);
        }
        yield return null;
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

public static class HelperFunctions
{
    public static bool Approximately(this Vector3 v, Vector3 comp, float threshold = 0.01f)
    {
        return Mathf.Abs(Vector3.Distance(v, comp)) <= threshold;
    }
}
