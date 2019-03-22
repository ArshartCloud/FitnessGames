using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [Tooltip("The position for spawning")]
    public Transform spawnPoint;
    [Tooltip("The speed of spawning object")]
    public float speed = 4.63f;
    //[Tooltip("The size of spawning object")]
    //public float size = .2f;
    [Tooltip("The time between spawning two objects")]
    public float timeGap = 5.0f;
    [Tooltip("The object factory objects")]
    public Factory fruitFactory;

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
            //GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //Rigidbody rb = go.AddComponent<Rigidbody>();
            //rb.useGravity = false;
            //go.transform.localScale = new Vector3(size, size, size);
            GameObject go = fruitFactory.Create();

            go.GetComponent<Rigidbody>().velocity = speed * movingDirection;
            go.transform.position = spawnPoint.position;
            go.transform.rotation = spawnPoint.rotation;
            FlyingObject fo = go.GetComponent<FlyingObject>();// get real object from unity  
            fo.SetFactory(fruitFactory);
            fo.ReclaimByTime();
        }
	}
}
