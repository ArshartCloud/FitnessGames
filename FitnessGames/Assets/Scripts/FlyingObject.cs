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
    public bool breakable = true;

    /// <summary>
    /// the score of object
    /// </summary>
    public int score = 1;

    public FlyingObjectState state = FlyingObjectState.Untouched;

    /// <summary>
    /// time to start the counter to reclaim.
    /// </summary>
    float startTime;
    /// <summary>
    /// the father that create the object
    /// </summary>
    Factory factory;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
            rb.AddExplosionForce(10.0f, transform.position, 5.0f, 3.0F);
    }

    public void Init()
    {
        state = FlyingObjectState.Untouched;
        this.enabled = true;
    }

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void ReclaimByTime()
    {
        startTime = Time.time;
        Init();
    }

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void ReclaimByTime(float time)
    {
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
        factory.Reclaim(gameObject);
        this.enabled = false;
    }

    /// <summary>
    /// Reclaim gameObject and do some animation
    /// </summary>
    public void Explode()
    {
        Reclaim();
        // add some animation here
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// do some fantasctic shining
    /// </summary>
    public void Shine()
    {
        state = FlyingObjectState.Touched;
    }

	// Update is called once per frame
	void Update () {
		if (Time.time - startTime > liveTime)
        {
            Reclaim();
        }
	}
}
