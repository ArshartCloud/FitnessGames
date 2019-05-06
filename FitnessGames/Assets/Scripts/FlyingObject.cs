using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour {
    // Enumerate states of virtual hand interactions
    public enum FlyingObjectState
    {
        Untouched,
        Touched
    };

    /// <summary>
    /// maximum time that the flying object can live
    /// </summary>
    public float liveTime = 6.0f;

    /// <summary>
    /// The object is break on touch or not. eg: fruit is breakable while wall is not.
    /// </summary>
    public bool collectable =false;
    public bool autoRotate = false;
    public float rotateSpeed = 5f;

    /// <summary>
    /// the score of object
    /// </summary>
    public int score = 1;

    //public FlyingObjectState state = FlyingObjectState.Untouched;

    /// <summary>
    /// time to start the counter to reclaim.
    /// </summary>
    float startTime;
    /// <summary>
    /// the father that create the object
    /// </summary>
    Factory factory;
    bool isReclaim = false;
    // TO fix some very wired bug
    Vector3 velocity = Vector3.back;
    //Vector3 angularVelocity = Vector3.zero;


    private void Start()
    {
        //Rigidbody rb = GetComponent<Rigidbody>();
    }

    public void SetSpeed(Vector3 speed)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        velocity = speed;
        rb.velocity = velocity;

            //rb.angularVelocity = new Vector3(0, 0, 1) * rotateSpeed;
    }

    public void Init()
    {
        //state = FlyingObjectState.Untouched;
        this.enabled = true;
    }

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void ReclaimByTime()
    {
        isReclaim = true;
        startTime = Time.time;
        Init();
    }

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void ReclaimByTime(float time)
    {
        isReclaim = true;
        liveTime = time;
        startTime = Time.time;
        Init();
    }

    /// <summary>
    /// Set factory to reclaim the object
    /// </summary>
    /// <param name="f"></param>
    public void SetFactory(Factory f)
    {
        factory = f;
    }
	
    /// <summary>
    /// Reclaim gameObject
    /// </summary>
    public void Reclaim()
    {
	    if (isReclaim) {
		    isReclaim = false;
        	factory.Reclaim(gameObject);
        	this.enabled = false;
	    }
    }

    /// <summary>
    /// Reclaim gameObject and do some animation
    /// </summary>
    public void Explode()
    {
        // add some animation here
   //     GetComponent<AudioSource>().Play();
        Reclaim();
    }

    /// <summary>
    /// do some fantasctic shining
    /// </summary>
    public void Shine()
    {
        // add some animation here
        //GetComponent<AudioSource>().Play();
        //state = FlyingObjectState.Touched;
    }

	// Update is called once per frame
	void Update () {
        //print(GetComponent<Rigidbody>().angularVelocity);
		if (Time.time - startTime > liveTime)
        {
            Reclaim();
        }
        if (autoRotate)
        {
            transform.Rotate(new Vector3(0, 0, 1) * rotateSpeed * Time.deltaTime);
        }
        // to fix some bug
        //Rigidbody rb = GetComponent<Rigidbody>();
        //transform.position += velocity * Time.deltaTime;
        //rb.velocity = velocity;
        //rb.angularVelocity = angularVelocity;
    }
}
