using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObject : MonoBehaviour {

    public float liveTime = 6.0f;
    float spawnTime;
    Factory factory;

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void ReclaimByTime()
    {
        spawnTime = Time.time;
        this.enabled = true;
    }

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void ReclaimByTime(float time)
    {
        liveTime = time;
        spawnTime = Time.time;
        this.enabled = true;
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
    }

	// Update is called once per frame
	void Update () {
		if (Time.time - spawnTime > liveTime)
        {
            Reclaim();
        }
	}
}
