using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    private static HandManager vrHands;
    public static OVRGrabber LeftHand, RightHand;
    public GameObject SnowballPrefab;
    private static Dictionary<bool, OVRGrabber> ovrPairs;
    private static float LeftHandPositionDerivative, RightHandPositionDerivative;
  

    private void Awake()
    {
        if (vrHands == null) { vrHands = this; } else { Destroy(this); }
        ovrPairs.Add(true, LeftHand); ovrPairs.Add(false, RightHand);
        StartCoroutine(ChangeInOvrPosition(true)); StartCoroutine(ChangeInOvrPosition(false)); //maybe start these if both hands are in the snow mound collider & stop on exit?
    }

    public static OVRGrabber GetOVRGrabber(bool isLeft)
    {
        return ovrPairs[isLeft];
    }

    public static Vector3 HandPosition(bool isLeft)
    {
        OVRGrabber g = ovrPairs[isLeft];
        if(g == null) { return Vector3.zero; }
        return g.transform.position;
    }

    public static IEnumerator SufficientHandMovement(float threshold, float seconds)
    {
        float t = 0;
        while(LeftHandPositionDerivative > threshold && RightHandPositionDerivative > threshold)
        {
            while (t < seconds)
            {
                t += 1;
                yield return new WaitForSeconds(1);
            }
        }

        if (t>=seconds) { Debug.Log("Sufficient Hand Movement, spawn snowball"); }
        yield return vrHands.StartCoroutine(SufficientHandMovement(threshold, seconds));
    }

    private static IEnumerator ChangeInOvrPosition(bool isLeft)
    {
        Vector3 a = GetOVRGrabber(isLeft).transform.position;
        yield return new WaitForFixedUpdate();
        Vector3 b = GetOVRGrabber(isLeft).transform.position;
        float derivative = (b - a).magnitude;
        if (isLeft) { LeftHandPositionDerivative = derivative; } else { RightHandPositionDerivative = derivative; }
        yield return vrHands.StartCoroutine(ChangeInOvrPosition(isLeft));
    }
}
