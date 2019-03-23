using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Tooltip("The position for spawning")]
    //public Transform[] spawnPoints;
    public Transform wallspawnPoint;
    [Tooltip("The speed of spawning object")]
    public float objectSpeed = 4.63f;
    //[Tooltip("The size of spawning object")]
    //public float size = .2f;
    [Tooltip("The time between spawning two objects")]
    public float timeGap = 5.0f;
    [Tooltip("The object factory objects")]
    public Factory objectFactory;

    [Tooltip("The object Spawn each time, 1 or 2")]
    public int SpawnNumber = 2;

    Vector3 movingDirection = new Vector3(0, 0, -1);
    float lastSpawnTime;

	// Use this for initialization
	void Start () {
        lastSpawnTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastSpawnTime > timeGap)
        {
            lastSpawnTime = Time.time;
            GameObject go = objectFactory.Create();
            go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
            float x = Random.Range(0.0f, 2.0f);
            print(x);
            go.transform.position = wallspawnPoint.position + new Vector3(x, 0.0f, 0.0f);
            print(go.transform.position);
            go.transform.rotation = wallspawnPoint.rotation;
            FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
            fo.SetFactory(objectFactory);
            fo.ReclaimByTime();

            lastSpawnTime = Time.time;
            go = objectFactory.Create();
            go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
            go.transform.position = wallspawnPoint.position + new Vector3(-x, 0.0f, 0.0f);
            go.transform.rotation = wallspawnPoint.rotation;
            fo = go.GetComponent<FlyingObject>();// get real object from unity  
            fo.SetFactory(objectFactory);
            fo.ReclaimByTime();

            //int leastNumber = SpawnNumber;
            //if (spawnPoints.Length == 1)
            //{
            //    leastNumber = 1;
            //}
            //int index = Random.Range(0, spawnPoints.Length);
            //for (int i = 0; i < leastNumber; i++)
            //{
            //    if (i == 0)
            //    {

            //    }
            //    else
            //    {
            //        int tempIndex = Random.Range(0, spawnPoints.Length);
            //        while ( index == tempIndex)
            //        {
            //             tempIndex = Random.Range(0, spawnPoints.Length);
            //        }
            //        index = tempIndex;
            //    }

            //    lastSpawnTime = Time.time;
            //    //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //    //Rigidbody rb = go.AddComponent<Rigidbody>();
            //    //rb.useGravity = false;
            //    //go.transform.localScale = new Vector3(size, size, size);
            //    GameObject go = objectFactory.Create();

            //    go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
            //    go.transform.position = spawnPoints[index].position;
            //    go.transform.rotation = spawnPoints[index].rotation;
            //    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
            //    fo.SetFactory(objectFactory);
            //    fo.ReclaimByTime();
            //}

        }
    }
}
