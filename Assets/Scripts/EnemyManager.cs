using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance = null;

    [SerializeField]
    public List<GameObject> EnemyWaypointList;

    public Transform[] GetWaypointArray(int i)
    {
        if (i >= EnemyWaypointList.Count)
            return null;

        GameObject tmp = EnemyWaypointList[i];
        Transform[] waypointArray = new Transform[tmp.transform.childCount];

        for (int j = 0; j < tmp.transform.childCount; j++)
        {
            waypointArray[j] = tmp.transform.GetChild(j).GetComponent<Transform>();
        }

        return waypointArray;
    }

    public static EnemyManager GetInstance()
    {
        if (instance == null)
            instance = new EnemyManager();

        return instance;
    }
}
