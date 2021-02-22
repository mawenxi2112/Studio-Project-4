using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;
    public List<Transform[]>[] EnemyWaypointList;
    public List<GameObject>[] EnemyWaypointHolder;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        GameObject waypointManager = transform.Find("WaypointManager").gameObject;
        EnemyWaypointList = new List<Transform[]>[waypointManager.transform.childCount];
        EnemyWaypointHolder = new List<GameObject>[waypointManager.transform.childCount];

        if (waypointManager)
        {
            for (int i = 0; i < waypointManager.transform.childCount; i++)
            {
                EnemyWaypointHolder[i] = new List<GameObject>();
                EnemyWaypointList[i] = new List<Transform[]>();

                GameObject waypointLevel = waypointManager.transform.GetChild(i).gameObject;

                for (int j = 0; j < waypointLevel.transform.childCount; j++)
				{
                    GameObject waypointSet = waypointLevel.transform.GetChild(j).gameObject;
                    Transform[] tmp = new Transform[waypointSet.transform.childCount];

                    for (int k = 0; k < waypointSet.transform.childCount; k++)
                    {
                        tmp[k] = waypointSet.transform.GetChild(k);
                    }

                    EnemyWaypointHolder[i].Add(waypointSet);
                    EnemyWaypointList[i].Add(tmp);
                }

            }

            for (int i = 0; i < waypointManager.transform.childCount; i++)
            {
                Debug.Log("Number of EnemyWaypointHolder in " + i + ": " + EnemyWaypointHolder[i].Count);
                Debug.Log("Number of EnemyWaypointList in " + i + ": " + EnemyWaypointList[i].Count);
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
