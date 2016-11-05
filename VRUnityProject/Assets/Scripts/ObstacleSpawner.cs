﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour {

    // To increment ingame
    private float spawnInterval = 3f;
    private int numOfObjectsToSpawn = 2;
    private int intervalBetweenDesignedSituations = 5;

    private List<GameObject> obstacles;
    private List<GameObject> handDesignedSituations;

    private bool bSpawn = true;

    void Initialize()
    {
        // List the Obstacles
        obstacles = new List<GameObject>();
        GameObject[] obsts = Resources.LoadAll<GameObject>("Obstacles");
        for (int i = 0; i < obsts.Length; i++)
        {
            obstacles.Add(obsts[i]);
        }

        // List the Hand designed situations
        handDesignedSituations = new List<GameObject>();
        GameObject[] handDes = Resources.LoadAll<GameObject>("PrefabsSituationsObstacles");
        for (int i = 0; i < handDes.Length; i++)
        {
            handDesignedSituations.Add(handDes[i]);
        }

    }

	void Start ()
    {
        Initialize();
        StartCoroutine("Spawn");
    }

    IEnumerator Spawn()
    {
        int interval = 0;
        while (bSpawn)
        {
            yield return new WaitForSeconds(spawnInterval);
            interval++;
            if (interval == intervalBetweenDesignedSituations)
            {
                interval = 0;

                int x = Random.Range(0, 6); int y = Random.Range(0, 6);
                int handDesignedSitu = Random.Range(0, handDesignedSituations.Count);

                Vector3 spawnPos = transform.position;
                GameObject instantiatedObstacle = Instantiate(handDesignedSituations[handDesignedSitu], spawnPos, Quaternion.identity) as GameObject;
            }
            else
            {
                for (int i = 0; i < numOfObjectsToSpawn; i++)
                {
                    int x = Random.Range(0, 6); int y = Random.Range(0, 6);
                    int obstNum = Random.Range(0, obstacles.Count);

                    Vector3 spawnPos = transform.position + new Vector3(0, y, x - 2);
                    GameObject instantiatedObstacle = Instantiate(obstacles[obstNum], spawnPos, Quaternion.identity) as GameObject;

                    //TODO verify if multiple objects are not in the same spot.
                }
            }   
            yield return new WaitForEndOfFrame();
        }
    }
}
