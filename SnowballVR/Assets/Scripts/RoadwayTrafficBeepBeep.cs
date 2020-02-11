using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoadwayTrafficBeepBeep : MonoBehaviour, IComparer<CarInfo>
{
    private static RoadwayTrafficBeepBeep beepBeep;
    public static TrafficSpawner LeftLaneTraffic, RightLaneTraffic;
    public GameObject LeftLane, RightLane;
    public GameObject DebugVehicle;

    public static Queue<CarInfo> CarsToSpawn;
    public List<CarInfo> CarsSortedByFrequency;

    private void Start()
    {
        if (beepBeep == null) { beepBeep = this; } else { Destroy(beepBeep); }
        LeftLaneTraffic = new TrafficSpawner(LeftLane.transform.GetChild(0).gameObject, LeftLane.transform.GetChild(1).gameObject);
        RightLaneTraffic = new TrafficSpawner(RightLane.transform.GetChild(0).gameObject, RightLane.transform.GetChild(1).gameObject);
        CarsSortedByFrequency = new List<CarInfo>(Resources.LoadAll<CarInfo>("CarInformation"));
        CarsSortedByFrequency.Sort(Compare);
        CarsToSpawn = new Queue<CarInfo>();
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
            // float v = Mathf.Floor(UnityEngine.Random.Range(0, 1) * 10);
            int v = Mathf.FloorToInt(UnityEngine.Random.Range(0, beepBeep.CarsSortedByFrequency.Count));
            CarsToSpawn.Enqueue(beepBeep.CarsSortedByFrequency[v]);
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
            newVehicle.SetDestination(Destination.position);
            CarsSpawned.Add(newVehicle);
        }
    }

    public void DebugTimer()
    {
        Debug.Log(currentSpawnTimer);
    }
}
