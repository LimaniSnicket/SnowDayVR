using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    private static HandManager vrHands;
    public GameObject LeftOVR, RightOVR;
    public static OVRGrabber LeftHand, RightHand;
    public GameObject SnowballPrefab;
    private static Dictionary<bool, OVRGrabber> ovrPairs;
    private static float LeftHandPositionDerivative, RightHandPositionDerivative;
  

    private void Awake()
    {
        if (vrHands == null) { vrHands = this; } else { Destroy(this); }
        ovrPairs = new Dictionary<bool, OVRGrabber>();
        LeftHand = LeftOVR.GetComponent<OVRGrabber>(); RightHand = RightOVR.GetComponent<OVRGrabber>();
        ovrPairs.Add(true, LeftHand); ovrPairs.Add(false, RightHand);
        StartCoroutine(ChangeInOvrPosition(true)); StartCoroutine(ChangeInOvrPosition(false)); //maybe start these if both hands are in the snow mound collider & stop on exit?
        SnowMoundBehavior.OnHandMotionDetected += OnHandMotionInMound;
    }

    public static void OnHandMotionInMound(bool valid)
    {
        if (valid)
        {
            vrHands.StartCoroutine(SufficientHandMovement(1, 1));
        } else
        {
            vrHands.StopCoroutine("SufficientHandMovement");
        }
    }

    public static OVRGrabber GetOVRGrabber(bool isLeft)
    {
        return ovrPairs[isLeft];
    }

    public static float AngleOfOVRHand(bool isLeft)
    {
        Vector3 handAngle = GetOVRGrabber(isLeft).transform.right;
        return Mathf.Abs(Vector3.Angle(Vector3.up, handAngle));
    }

    public static Vector3 HandPosition(bool isLeft)
    {
        OVRGrabber g = ovrPairs[isLeft];
        if(g == null) { return Vector3.zero; }
        return g.transform.position;
    }

    static bool ValidHands(float threshold = 0.5f)
    {
        return LeftHandPositionDerivative >= threshold && RightHandPositionDerivative >= threshold;
    }

    static bool ValidHands(bool isLeft, float threshold = 0.5f)
    {
        if (isLeft)
        {
            return LeftHandPositionDerivative >= threshold;
        }

        return RightHandPositionDerivative >= threshold;
    }

    public static IEnumerator SufficientHandMovement(float threshold, float seconds)
    {
        float t = 0;
        while(ValidHands(threshold))
        {
            while (t < seconds)
            {
                t += 1;
                yield return new WaitForSeconds(1);
            }
        }

        if (t >= seconds)
        {
            if (SnowMoundBehavior.canSpawnSnowball)
            {
                SnowballBehavior b = Instantiate(vrHands.SnowballPrefab).GetComponent<SnowballBehavior>();
                b.SetOVRGrabberFollow(GetOVRGrabber(true));
                Debug.Log("Sufficient Hand Movement, spawn snowball");
            }
            else
            {
                Snowfall.ActiveSnowball().ScaleUp(LeftHandPositionDerivative + RightHandPositionDerivative);
                Debug.Log("Add to snowball" + (LeftHandPositionDerivative + RightHandPositionDerivative));
            }
        }
        else
        {
            yield return vrHands.StartCoroutine(SufficientHandMovement(threshold, seconds));
        }
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

    private void OnDestroy()
    {
        SnowMoundBehavior.OnHandMotionDetected -= OnHandMotionInMound;
    }
}
