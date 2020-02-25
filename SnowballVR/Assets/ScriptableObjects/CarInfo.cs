using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Car", menuName = "Car")]
public class CarInfo : ScriptableObject
{
    public string CarID;
    public int OnHitCarBonus;
    public float MaxSpeed;
    public GameObject CarModel;
    public Texture[] AvailableTextures;
    public float SpawnFrequency;
    public OnHitEffect OnHitDo;
    public Vector3 RotationOffset;

    public VehicleBehavior GenerateCar(Transform spawnAt, bool add = false, float m = 1)
    {
        GameObject g = Instantiate(CarModel);
        g.transform.position = spawnAt.position;
        g.transform.forward = spawnAt.right;
        g.transform.eulerAngles = RotationOffset+ new Vector3(0,m,0);
        VehicleBehavior v = add ? g.AddComponent<VehicleBehavior>() : g.GetComponent<VehicleBehavior>(); 
        v.CarInformation = this;
        ApplyTexture(g);
        return v;
    }

    void ApplyTexture(GameObject g)
    {
        if(AvailableTextures == null || AvailableTextures.Length <= 0) { return; }
        int i = Mathf.FloorToInt(Random.Range(0, AvailableTextures.Length));
        Texture t = AvailableTextures[i];
        g.GetComponent<MeshRenderer>().material.mainTexture = t;
    }
}

public enum OnHitEffect
{
    NONE = 0,
    PENALTY = 1,
    BONUS = 2
}
