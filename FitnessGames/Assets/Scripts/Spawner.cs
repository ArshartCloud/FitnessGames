using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnDeltaZ = 20f;
    public float asteroidDeltaX = .8f;
    public float asteroidUpDeltaY = 1f;
    public float asteroidDownDeltaY = -.75f;
    public float spaceshipDeltaX = 1f;
    public float spaceshipDeltaY = .5f;

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
    public Factory[] spaceShipFactorys;

    [Tooltip("The object factory objects")]
    public Factory asteroidFactory;

    public GameMode state;

    //[Tooltip("Parameters for twist")]
    //public float spaceShipWidth = 1f;

    //[Tooltip("Parameters for twist")]
    //public float spaceShipWidth = 1f;

    Vector3 userHeadPos;
    Vector3 movingDirection = new Vector3(0, 0, -1);
    float lastSpawnTime;
    int step = 0;  // each step for spawning
    Transform world;  // the parent for spawning object
    bool spawnStart = false;

    public void SetHeadPos(Vector3 pos)
    {
        userHeadPos = pos;
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
                    Spawn(asteroidFactory, userHeadPos + new Vector3(asteroidDeltaX, asteroidUpDeltaY, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(-asteroidDeltaX, asteroidUpDeltaY, spawnDeltaZ));
                }
                else
                {
                    // asteroid down step
                    step = 0;
                    Spawn(asteroidFactory, userHeadPos + new Vector3(asteroidDeltaX, asteroidDownDeltaY, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(-asteroidDeltaX, asteroidDownDeltaY, spawnDeltaZ));
                }
            }
            else if (state == GameMode.Twist)
            {
                if (step == 0)
                {
                    // spaceship step
                    step++;
                    Factory objectFactory = spaceShipFactorys[Random.Range(0, spaceShipFactorys.Length)];
                    Spawn(objectFactory, userHeadPos + new Vector3(spaceshipDeltaX, 0f, spawnDeltaZ));
                    Spawn(objectFactory, userHeadPos + new Vector3(-spaceshipDeltaX, 0f, spawnDeltaZ));
                } else
                {
                    // asteroid step
                    step = 0;
                    //go.transform.position = spawnPoints[0].position;
                    Spawn(asteroidFactory, userHeadPos + new Vector3(asteroidDeltaX, 0f, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(-asteroidDeltaX, 0f, spawnDeltaZ));
                }
            }
            else if (state == GameMode.Squat)
            {
                if (step == 0)
                {
                    step++;
                    Factory objectFactory = spaceShipFactorys[Random.Range(0, spaceShipFactorys.Length)];
                    Spawn(objectFactory, userHeadPos + new Vector3(0f, spaceshipDeltaY, spawnDeltaZ));
                }
                else
                {
                    step = 0;
                    Spawn(asteroidFactory, userHeadPos + new Vector3(asteroidDeltaX, asteroidUpDeltaY, spawnDeltaZ));
                    Spawn(asteroidFactory, userHeadPos + new Vector3(-asteroidDeltaX, asteroidUpDeltaY, spawnDeltaZ));
                }
            }
        }
    }
}
