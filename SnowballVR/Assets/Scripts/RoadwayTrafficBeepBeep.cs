﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class RoadwayTrafficBeepBeep : MonoBehaviour, IComparer<CarInfo>
{
    private static RoadwayTrafficBeepBeep beepBeep;
    public static TrafficSpawner LeftLaneTraffic, RightLaneTraffic;
    public GameObject LeftLane, RightLane;
    public GameObject DebugVehicle;

    public static Queue<CarInfo> CarsToSpawn;
    public List<CarInfo> CarsSortedByFrequency;
    private static Dictionary<float, List<CarInfo>> CarsByFrequency;

    private void Start()
    {
        if (beepBeep == null) { beepBeep = this; } else { Destroy(beepBeep); }
        LeftLaneTraffic = new TrafficSpawner(LeftLane.transform.GetChild(0).gameObject, LeftLane.transform.GetChild(1).gameObject);
        RightLaneTraffic = new TrafficSpawner(RightLane.transform.GetChild(0).gameObject, RightLane.transform.GetChild(1).gameObject);
        CarsSortedByFrequency = new List<CarInfo>(Resources.LoadAll<CarInfo>("CarInformation"));
        CarsSortedByFrequency.Sort(Compare);
        CarsToSpawn = new Queue<CarInfo>();
        CarsByFrequency = CarTypeToFrequencyLookup(CarsSortedByFrequency);
        StartCoroutine(MaintainCarQueue());
    }

    private void Update()
    {
        LeftLaneTraffic.ManageTraffic();
        RightLaneTraffic.ManageTraffic();
    }

    public static void VehicleDestroyed(VehicleBehavior vb)
    {
        if (LeftLaneTraffic.CarsSpawned.Contains(vb))
        {
            Debug.Log("Destroying a vehicle in the Left Lane");
            LeftLaneTraffic.CarsSpawned.Remove(vb);
        }

        if (RightLaneTraffic.CarsSpawned.Contains(vb))
        {
            Debug.Log("Destroying a vehicle in the Right Lane");
            RightLaneTraffic.CarsSpawned.Remove(vb);
        }
        beepBeep.StartCoroutine(MaintainCarQueue());
    }

   public static IEnumerator MaintainCarQueue()
    {
        while (CarsToSpawn.Count < 10)
        {
            //// float v = Mathf.Floor(UnityEngine.Random.Range(0, 1) * 10);
            //int v = Mathf.FloorToInt(UnityEngine.Random.Range(0, beepBeep.CarsSortedByFrequency.Count));
            //CarsToSpawn.Enqueue(beepBeep.CarsSortedByFrequency[v]);
            float f = GetCarListByFrequency();
            List<CarInfo> c = CarsByFrequency[f];
            CarsToSpawn.Enqueue(c[0]);
            yield return null;
        }
        yield return null;
    }

    public int Compare(CarInfo a, CarInfo b)
    {
        if(a == null && b == null) { return 0; }
        if(a != null && b == null) { return -1; }
        if(a == null && b != null) { return 1; }
        if(a.SpawnFrequency >= b.SpawnFrequency) { return -1; }
        return 1;
    }

    Dictionary<float, List<CarInfo>> CarTypeToFrequencyLookup(List<CarInfo> c)
    {
        Dictionary<float, List<CarInfo>> d = new Dictionary<float,List<CarInfo>>();
        if(c.Count <= 0) { return d; }
        for (int i = 0; i < c.Count; i ++)
        {
            if (d.ContainsKey(c[i].SpawnFrequency))
            {
                d[c[i].SpawnFrequency].Add(c[i]);
            } else
            {
                d.Add(c[i].SpawnFrequency, new List < CarInfo > { c[i] });
            }
        }
        return d;
    }

    static float GetCarListByFrequency(float max = 1, float mult = 10)
    {
        float f = Mathf.Floor(UnityEngine.Random.Range(0, max) * mult);
        if (CarsByFrequency.ContainsKey(f)) { return f; }
        if(f >= CarsByFrequency.Last().Key) { return CarsByFrequency.Last().Key; }
        for (int i = 0; i < CarsByFrequency.Keys.Count; i++)
        {
            if(f <= CarsByFrequency.ElementAt(i).Key) { return f; }
        }
        return CarsByFrequency.Last().Key;
    }

   
}

[Serializable]
public class TrafficSpawner
{
    public Transform SpawnPosition, Destination;
    public List<VehicleBehavior> CarsSpawned;
    private float TimeTillSpawn, currentSpawnTimer;
    public TrafficSpawner() { SpawnPosition = null; Destination = null; CarsSpawned = new List<VehicleBehavior>(); }
    public TrafficSpawner(GameObject spawnAt, GameObject goTo)
    {
        SpawnPosition = spawnAt.transform;
        Destination = goTo.transform;
        CarsSpawned = new List<VehicleBehavior>();
        TimeTillSpawn = UnityEngine.Random.Range(10f, 15f);
    }

    public void ManageTraffic(CarInfo carToSpawn)
    {
        currentSpawnTimer += Time.deltaTime;
        if (currentSpawnTimer >= TimeTillSpawn)
        {
            currentSpawnTimer = 0;
            TimeTillSpawn = UnityEngine.Random.Range(10f, 15f);
            VehicleBehavior newVehicle = carToSpawn.GenerateCar(SpawnPosition);
            CarsSpawned.Add(newVehicle);
        }
    }

        public void ManageTraffic()
        {
            currentSpawnTimer += Time.deltaTime;
        if (currentSpawnTimer >= TimeTillSpawn)
        {
            currentSpawnTimer = 0;
            TimeTillSpawn = UnityEngine.Random.Range(10f, 15f);
            CarInfo i = RoadwayTrafficBeepBeep.CarsToSpawn.Dequeue();
            VehicleBehavior newVehicle = i.GenerateCar(SpawnPosition);
            newVehicle.CarInformation = i;
            newVehicle.SetVehicleValues(Destination.position, i.MaxSpeed);
            CarsSpawned.Add(newVehicle);
        }
    }

    public void DebugTimer()
    {
        Debug.Log(currentSpawnTimer);
    }
}
