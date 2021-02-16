﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneGameObjects
{
    /**
     * Retrieve the Scene's Root Game Objects
     * 
     * @returns Returns the List of Game Objects in Scene's Root
     */
    public static List<GameObject> GetRootGameObjects()
    {
        List<GameObject> rootGameObjects = new List<GameObject>();

        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootGameObjects);

        return rootGameObjects;
    }
}
