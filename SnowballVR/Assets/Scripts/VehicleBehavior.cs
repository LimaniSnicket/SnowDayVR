using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
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
    public CarInfo CarInformation;
    public static event Action<float, int> SnowballHit;
    public List<Collider> CollidersOnVehicle;
    private Vector3 Destination;
    private float Speed;
    GameObject[] wheels;
    private void Start()
    {
        SnowballBehavior.SnowballCollision += OnSnowballCollision;
        CollidersOnVehicle.AddRange(transform.GetComponentsInChildren<Collider>());
        wheels = new GameObject[2];
        int w = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "Wheel")
            {
                wheels[w] = transform.GetChild(i).gameObject;
                w++;
            }
        }
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Destination, Time.deltaTime * Speed);
        try { RotateWheels(); } catch (NullReferenceException) { }
        if (DestinationReached()) { Destroy(gameObject); }
    }

    void OnSnowballCollision(Collider c, float snowballSize)
    {
        if (CollidersOnVehicle == null || CollidersOnVehicle.Count <= 0) { return; }
        if (CollidersOnVehicle.Contains(c))
        {
            string cTag = c.tag;
            if (TagToPointValueLookup.ContainsKey(cTag))
            {
                SnowballHit(snowballSize, TagToPointValueLookup[cTag]);
            }
        }
    }

    public void SetDestination(Vector3 set) { Destination = set; }
    public void SetVehicleValues(Vector3 destination, float speedCap)
    {
        SetDestination(destination);
        Speed = speedCap;
    }

    bool DestinationReached()
    {
        return Mathf.Approximately(Vector3.Distance(transform.position, Destination), 0f);
    }

    void RotateWheels()
    {
        if(wheels == null || wheels.Length <= 0) { return; }
        foreach(var w in wheels)
        {
            w.transform.Rotate(Vector3.right, Time.deltaTime * Speed);
        }
    }

    void FuckingYeet()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.AddForce(Destination + new Vector3(0,20,0), ForceMode.Impulse);
        rb.AddTorque(Destination + new Vector3(100, 100, 100));
    }

    private void OnDestroy()
    {
        SnowballBehavior.SnowballCollision -= OnSnowballCollision;
        RoadwayTrafficBeepBeep.VehicleDestroyed(this);
    }
}

