using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public enum GameMode
    //{
    //    ArmRaise,
    //    Twist,
    //    Squat,
    //    Mixed
    //}

    [Tooltip("The object Spawn each time")]
    public int SpawnNumber = 2;
    [Tooltip("The position for spawning, continus SpawnNumber position are fix order")]
    public Transform[] spawnPoints;

    public Transform wallspawnPoint;
    public Transform squatzWallPoint;
    public Transform suqatCoinPoint;
    [Tooltip("The speed of spawning object")]
    public float objectSpeed = 4.63f;
    //[Tooltip("The size of spawning object")]
    //public float size = .2f;
    [Tooltip("The time between spawning two objects")]
    public float timeGap = 5.0f;
    [Tooltip("The object factory objects")]
    public Factory[] spaceShipFactorys;

    [Tooltip("The object factory objects")]
    public Factory asteroidFactory;

    public GameMode state;


    [Tooltip("Parameters for twist")]
    public float spaceShipWidth = 1f;

    //[Tooltip("Parameters for twist")]
    //public float spaceShipWidth = 1f;

    Vector3 movingDirection = new Vector3(0, 0, -1);
    float lastSpawnTime;
    int step = 0;  // each step for spawning
    Transform world;  // the parent for spawning object

    // Use this for initialization
    void Start () {
        lastSpawnTime = Time.time;
        world = GameObject.Find("World").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastSpawnTime > timeGap)
        {
            if (state == GameMode.Twist)
            {
                if (step == 0)
                {
                    Factory objectFactory = spaceShipFactorys[Random.Range(0, spaceShipFactorys.Length)];
                    step++;
                    float x = spaceShipWidth;
                    lastSpawnTime = Time.time;
                    GameObject go = objectFactory.Create();
                    //Rigidbody rb = go.GetComponent<Rigidbody>();
                    //rb.velocity = objectSpeed * movingDirection;
                    //rb.angularVelocity = Vector3.zero;
                    go.transform.position = wallspawnPoint.position + new Vector3(x, 0.0f, 0.0f);
                    go.transform.rotation = wallspawnPoint.rotation;
                    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    fo.SetSpeed(objectSpeed * movingDirection);

                    go = objectFactory.Create();
                    //rb = go.GetComponent<Rigidbody>();
                    //rb.velocity = objectSpeed * movingDirection;
                    //rb.angularVelocity = Vector3.zero;
                    go.transform.position = wallspawnPoint.position + new Vector3(-x, 0.0f, 0.0f);
                    go.transform.rotation = wallspawnPoint.rotation;
                    fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    fo.SetSpeed(objectSpeed * movingDirection);
                } else
                {
                    step = 0;
                    Factory objectFactory = asteroidFactory;
                    lastSpawnTime = Time.time;
                    GameObject go = objectFactory.Create();
                    //go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
                    go.transform.position = spawnPoints[0].position;
                    go.transform.rotation = spawnPoints[0].rotation;
                    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    fo.SetSpeed(objectSpeed * movingDirection);

                    go = objectFactory.Create();
                    //go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
                    go.transform.position = spawnPoints[1].position;
                    go.transform.rotation = spawnPoints[1].rotation;
                    fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    //go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    fo.SetSpeed(objectSpeed * movingDirection);
                }
            } else if (state == GameMode.ArmRaise)
            {
                Factory objectFactory = asteroidFactory;
                // fix number
                if (spawnPoints.Length < SpawnNumber)
                {
                    return;
                }
                //int index = Random.Range(0, spawnPoints.Length / SpawnNumber);
                //index *= SpawnNumber;
                int index = 2;
                if (step == 0) step++;
                else
                {
                    step = 0;
                    index = 0;
                }
                for (int i = 0; i < SpawnNumber; i++)
                {
                    lastSpawnTime = Time.time;
                    GameObject go = objectFactory.Create();
                    go.transform.SetParent(world, true);
                    //Rigidbody rb = go.GetComponent<Rigidbody>();
                    //rb.velocity = objectSpeed * movingDirection;
                    //rb.angularVelocity = Vector3.zero;
                    go.transform.position = spawnPoints[index].position;
                    go.transform.rotation = spawnPoints[index].rotation;
                    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetSpeed(objectSpeed * movingDirection);
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
		            index++;
                }
            }
            else if (state == GameMode.Squat)
            {
                if (step == 0)
                {
                    Factory objectFactory = spaceShipFactorys[Random.Range(0, spaceShipFactorys.Length)];
                    step++;
                    float x = spaceShipWidth;
                    lastSpawnTime = Time.time;
                    GameObject go = objectFactory.Create();
                    //Rigidbody rb = go.GetComponent<Rigidbody>();
                    //rb.velocity = objectSpeed * movingDirection;
                    //rb.angularVelocity = Vector3.zero;
                    go.transform.position = wallspawnPoint.position + new Vector3(x, 0.0f, 0.0f);
                    go.transform.rotation = wallspawnPoint.rotation;
                    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    fo.SetSpeed(objectSpeed * movingDirection);

                    go = objectFactory.Create();
                    //rb = go.GetComponent<Rigidbody>();
                    //rb.velocity = objectSpeed * movingDirection;
                    //rb.angularVelocity = Vector3.zero;
                    go.transform.position = wallspawnPoint.position + new Vector3(-x, 0.0f, 0.0f);
                    go.transform.rotation = wallspawnPoint.rotation;
                    fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    fo.SetSpeed(objectSpeed * movingDirection);
                }
                else
                {
                    step = 0;
                    Factory objectFactory = asteroidFactory;
                    lastSpawnTime = Time.time;
                    GameObject go = objectFactory.Create();
                    //go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
                    go.transform.position = spawnPoints[0].position;
                    go.transform.rotation = spawnPoints[0].rotation;
                    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    fo.SetSpeed(objectSpeed * movingDirection);

                    go = objectFactory.Create();
                    //go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
                    go.transform.position = spawnPoints[1].position;
                    go.transform.rotation = spawnPoints[1].rotation;
                    fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
                    go.transform.SetParent(world, true);
                    //go.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    fo.SetSpeed(objectSpeed * movingDirection);
                }

            }
        }
    }
}
