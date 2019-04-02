using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnerState
    {
        Fruit,
        Wall
    }

    [Tooltip("The object Spawn each time")]
    public int SpawnNumber = 2;
    [Tooltip("The position for spawning, continus SpawnNumber position are fix order")]
    public Transform[] spawnPoints;

    public Transform wallspawnPoint;
    [Tooltip("The speed of spawning object")]
    public float objectSpeed = 4.63f;
    //[Tooltip("The size of spawning object")]
    //public float size = .2f;
    [Tooltip("The time between spawning two objects")]
    public float timeGap = 5.0f;
    [Tooltip("The object factory objects")]
    public Factory[] objectFactorys;

    public SpawnerState state;

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
            Factory objectFactory = objectFactorys[Random.Range(0, objectFactorys.Length)];
            if (state == SpawnerState.Wall)
            {
                lastSpawnTime = Time.time;
                GameObject go = objectFactory.Create();
                go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
                float x = Random.Range(0.5f, 2.0f);
                //print(x);
                go.transform.position = wallspawnPoint.position + new Vector3(x, 0.0f, 0.0f);
                //print(go.transform.position);
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
            } else if (state == SpawnerState.Fruit)
            {
                // fix number
                if (spawnPoints.Length < SpawnNumber)
                {
                    return;
                }
                int index = Random.Range(0, spawnPoints.Length / SpawnNumber);
		index *= SpawnNumber;
                for (int i = 0; i < SpawnNumber; i++)
                {
                    lastSpawnTime = Time.time;
                    GameObject go = objectFactory.Create();
                    go.transform.SetParent(GameObject.Find("World").transform, true);
                    go.GetComponent<Rigidbody>().velocity = objectSpeed * movingDirection;
                    go.transform.position = spawnPoints[index].position;
                    go.transform.rotation = spawnPoints[index].rotation;
                    FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
                    fo.SetFactory(objectFactory);
                    fo.ReclaimByTime();
		    index++;
                }
            }
        }
    }
}
