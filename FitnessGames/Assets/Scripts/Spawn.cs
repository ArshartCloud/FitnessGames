using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public Transform spawnPoint;
    public float speed = 6;
    public float size = .15f;
    public float timeGap = 1.0f;
    Vector3 direction = new Vector3(0, 0, -1);

    float lastSpawnTime = Time.time;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - lastSpawnTime > timeGap)
        {
            lastSpawnTime = Time.time;
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.velocity = speed * direction;
            go.transform.position = spawnPoint.position;
            go.transform.rotation = spawnPoint.rotation;
            //float size = .15f;
            go.transform.localScale = new Vector3(size, size, size);
        }
	}
}
