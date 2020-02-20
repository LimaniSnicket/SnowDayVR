using System.Collections;
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
    public  Dictionary<float, List<CarInfo>> CarsByFrequency;

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
        LeftLaneTraffic.ManageTraffic(0);
        RightLaneTraffic.ManageTraffic(180);
    }

    public static void VehicleDestroyed(VehicleBehavior vb)
    {
        if (LeftLaneTraffic.CarsSpawned.Contains(vb))
        {
            LeftLaneTraffic.CarsSpawned.Remove(vb);
        }

        if (RightLaneTraffic.CarsSpawned.Contains(vb))
        {
            RightLaneTraffic.CarsSpawned.Remove(vb);
        }
        beepBeep.StartCoroutine(MaintainCarQueue());
    }

   public static IEnumerator MaintainCarQueue()
    {
        while (CarsToSpawn.Count < 10)
        {
            float f = beepBeep.GetCarListByFrequency();
            List<CarInfo> c = new List<CarInfo>(beepBeep.CarsByFrequency[f]);
            int i = c.Count > 1 ? Mathf.FloorToInt(UnityEngine.Random.Range(0,c.Count)) :0;
            CarsToSpawn.Enqueue(c[i]);
            yield return null;
        }
        yield return null;
    }

    public int Compare(CarInfo a, CarInfo b)
    {
        if(a == null && b == null) { return 0; }
        if(a != null && b == null) { return -1; }
        if(a == null && b != null) { return 1; }
        if(a.SpawnFrequency < b.SpawnFrequency) { return -1; }
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

     float GetCarListByFrequency(float max = 1, float mult = 10)
    {
        float f = Mathf.Floor(UnityEngine.Random.Range(0, max) * mult);
        if (CarsByFrequency.ContainsKey(f)) { return f; }
        if(f >= CarsByFrequency.Last().Key) { return CarsByFrequency.Last().Key; }
        for (int i = 0; i < CarsByFrequency.Keys.Count-1; i++)
        {
            if (i == 0)
            {
                if(f <= CarsByFrequency.ElementAt(i).Key) { return CarsByFrequency.ElementAt(i).Key; }
            }

            if (f > CarsByFrequency.ElementAt(i).Key && f <= CarsByFrequency.ElementAt(i).Key)
            {
                return CarsByFrequency.ElementAt(i + 1).Key;
            }

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
        TimeTillSpawn = UnityEngine.Random.Range(3f, 5f);
    }

    public void ManageTraffic(float m)
    {
        currentSpawnTimer += Time.deltaTime;
        if (currentSpawnTimer >= TimeTillSpawn)
        {
            currentSpawnTimer = 0;
            TimeTillSpawn = UnityEngine.Random.Range(5f, 10f);
            CarInfo i = RoadwayTrafficBeepBeep.CarsToSpawn.Dequeue();
            VehicleBehavior newVehicle = i.GenerateCar(SpawnPosition, false, m);
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
