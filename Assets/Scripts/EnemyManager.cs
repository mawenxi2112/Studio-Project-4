using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;
    public List<Transform[]> EnemyWaypointList;

    public void Start()
    {
        GameObject waypointList = transform.Find("Waypoint").gameObject;

        if (waypointList)
        {
            for (int i = 0; i < waypointList.transform.childCount; i++)
            {
                GameObject waypointSet = waypointList.transform.GetChild(i).gameObject;
                Transform[] tmp = new Transform[waypointSet.transform.childCount];

                for (int j = 0; j < waypointSet.transform.childCount; j++)
                {
                    tmp[j] = waypointSet.transform.GetChild(j);
                    Debug.Log("Adding child transform" + tmp[j].position);
                }

                EnemyWaypointList.Add(tmp);
            }
        }
    }
    public static EnemyManager GetInstance()
    {
        if (instance == null)
            instance = new EnemyManager();

        return instance;
    }
}
