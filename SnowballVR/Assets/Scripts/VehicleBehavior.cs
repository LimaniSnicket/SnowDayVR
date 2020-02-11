using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VehicleBehavior : MonoBehaviour
{
    //make a dictionary you fucking nerd !!!
    //then make a list of all the colliders attached/parented to the object, check if it contains the collider that is hit, check the tag and return the point value!!
    public static Dictionary<string, int> TagToPointValueLookup
    {
        get => new Dictionary<string, int>
        {
            { "Window", 100},
            { "Body", 10}
        };
    }
    public static event Action<float, float> SnowballHit;

    private void Start()
    {
        SnowballBehavior.SnowballCollision += OnSnowballCollision;
    }

    void OnSnowballCollision(Collider c, float snowballSize)
    {

    }

    private void OnDestroy()
    {
        SnowballBehavior.SnowballCollision -= OnSnowballCollision;
    }
}

