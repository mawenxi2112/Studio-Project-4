using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;
    public List<Transform[]> EnemyWaypointList;
    public List<GameObject> EnemyWaypointHolder;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        EnemyWaypointList = new List<Transform[]>();
        EnemyWaypointHolder = new List<GameObject>();
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
                    //Debug.Log("Adding child transform" + tmp[j].position);
                }

                EnemyWaypointHolder.Add(waypointSet);
                EnemyWaypointList.Add(tmp);
            }

            Debug.Log("Number of EnemyWaypointHolder: " + EnemyWaypointHolder.Count);
            Debug.Log("Number of EnemyWaypointList: " + EnemyWaypointList.Count);
        }
    }
    public static EnemyManager GetInstance()
    {
        if (instance == null)
            instance = new EnemyManager();

        return instance;
    }
}
