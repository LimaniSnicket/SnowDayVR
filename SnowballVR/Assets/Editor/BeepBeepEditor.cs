using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadwayTrafficBeepBeep))]
public class BeepBeepEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoadwayTrafficBeepBeep b = (RoadwayTrafficBeepBeep)target;
        GUILayout.Label("CARS BY FREQUENCY: ", EditorStyles.boldLabel);
        if (b.CarsByFrequency.Count > 0 && b.CarsByFrequency != null)
        {
            foreach (var i in b.CarsByFrequency)
            {
                GUILayout.Label(i.Key * 10 + "%: ");
                string disp = "";
                foreach (var h in i.Value)
                {
                    disp += h.CarID + " ";
                }
                GUILayout.Label(disp);
            }
        }

        if (RoadwayTrafficBeepBeep.CarsToSpawn != null && RoadwayTrafficBeepBeep.CarsToSpawn.Count > 0)
        {
            GUILayout.BeginHorizontal();
            foreach (var c in RoadwayTrafficBeepBeep.CarsToSpawn)
            {
                GUILayout.Label(c.CarID + " ");
            }
            GUILayout.EndArea();
        }
    }
}
