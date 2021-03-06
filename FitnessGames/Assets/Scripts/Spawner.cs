﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnDeltaZ = 50f;
    public float energyDeltaX = .8f;
    public float energyUpDeltaY = 1f;
    public float energyDownDeltaY = -.75f;
    public float twistDeltaY = 0f;
    public float energyTwistDeltaX = 1f;
    public float asteroidTwistDeltaX = 1f;
    public float asteroidSquatDeltaY = .5f;
    public float asteroidSquatDeltaX = .4f;

    //[Tooltip("The object Spawn each time")]
    //public int SpawnNumber = 2;
    //[Tooltip("The position for spawning, continus SpawnNumber position are fix order")]
    //public Transform[] spawnPoints;
    //public Transform wallspawnPoint;
    //public Transform squatWallPoint;
    //public Transform suqatCoinPoint;

    [Tooltip("The speed of spawning object")]
    public float objectSpeed = 4.63f;
    [Tooltip("The time between spawning two objects")]
    public float timeGap = 5.0f;
    [Tooltip("The object factory objects")]
    public Factory asteroidFactory;

    [Tooltip("The object factory objects")]
    public Factory energyFactory;

    public GameMode state;

    //[Tooltip("Parameters for twist")]
    //public float spaceShipWidth = 1f;

    //[Tooltip("Parameters for twist")]
    //public float spaceShipWidth = 1f;

    public Vector3 userHeadPos;
    Vector3 movingDirection = new Vector3(0, 0, -1);
    float lastSpawnTime;
    int step = 0;  // each step for spawning
    Transform world;  // the parent for spawning object
    bool spawnStart = false;

    public void SetHeadPos(Vector3 pos)
    {
        userHeadPos = pos;
        userHeadPos.x = 0;
    }

    public void StartSpawn()
    {
        spawnStart = true;
    }

    // Use this for initialization
    void Start () {
        lastSpawnTime = Time.time;
        world = GameObject.Find("World").transform;
    }
	
    GameObject Spawn(Factory f, Vector3 pos)
    {
        GameObject go = f.Create();
        go.transform.position = pos;
        //go.transform.rotation = Vector3.zero;
        FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
        fo.SetFactory(f);
        fo.ReclaimByTime();
        go.transform.SetParent(world, true);
        fo.SetSpeed(objectSpeed * movingDirection);
        return go;
    }

	// Update is called once per frame
	void Update () {
        if (spawnStart && Time.time - lastSpawnTime > timeGap)
        {
            lastSpawnTime = Time.time;
            if (state == GameMode.ArmRaise)
            {
                if (step == 0)
                {
                    // asteroid up step
                    step++;
                    Spawn(energyFactory, userHeadPos + new Vector3(energyDeltaX, energyUpDeltaY, spawnDeltaZ));
                    Spawn(energyFactory, userHeadPos + new Vector3(-energyDeltaX, energyUpDeltaY, spawnDeltaZ));
                }
                else
                {
                    // asteroid down step
                    step = 0;
                    Spawn(energyFactory, userHeadPos + new Vector3(energyDeltaX, energyDownDeltaY, spawnDeltaZ));
                    Spawn(energyFactory, userHeadPos + new Vector3(-energyDeltaX, energyDownDeltaY, spawnDeltaZ));
                }
            }
            else if (state == GameMode.Twist)
            {
                if (step == 0)
                {
                    step++;
                    Spawn(asteroidFactory, userHeadPos + new Vector3(asteroidTwistDeltaX, twistDeltaY, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(-asteroidTwistDeltaX, twistDeltaY, spawnDeltaZ));
                } else
                {
                    step = 0;
                    Spawn(energyFactory, userHeadPos + new Vector3(energyTwistDeltaX, twistDeltaY, spawnDeltaZ));
                    Spawn(energyFactory, userHeadPos + new Vector3(-energyTwistDeltaX, twistDeltaY, spawnDeltaZ));
                }
            }
            else if (state == GameMode.Squat)
            {
                if (step == 0)
                {
                    step++;
                    Spawn(asteroidFactory, userHeadPos + new Vector3(0f, asteroidSquatDeltaY, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(asteroidSquatDeltaX, asteroidSquatDeltaY, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(-asteroidSquatDeltaX, asteroidSquatDeltaY, spawnDeltaZ));
                }
                else
                {
                    step = 0;
                    Spawn(energyFactory, userHeadPos + new Vector3(energyDeltaX, energyUpDeltaY, spawnDeltaZ));
                    Spawn(energyFactory, userHeadPos + new Vector3(-energyDeltaX, energyUpDeltaY, spawnDeltaZ));
                }
            }
        }
    }
}
