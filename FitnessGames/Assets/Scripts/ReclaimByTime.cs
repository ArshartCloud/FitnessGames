using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReclaimByTime : MonoBehaviour {

    public float liveTime = 6.0f;
    float spawnTime;
    Factory factory;

    /// <summary>
    /// enable the component, if it is the first time you use it, you should also set factory for it.
    /// </summary>
    public void Active()
    {
        spawnTime = Time.time;
        this.enabled = true;
    }

    /// <summary>
    /// enable the component and set factory for it.
    /// </summary>
    public void Active(Factory f)
    {
        Active();
        SetFactory(f);
    }

    /// <summary>
    /// Set factory to reclaim the object
    /// </summary>
    /// <param name="f"></param>
    public void SetFactory(Factory f)
    {
        factory = f;
    }
	
	// Update is called once per frame
	void Update () {
		if (Time.time - spawnTime > liveTime)
        {
            factory.Reclaim(gameObject);
            this.enabled = false;
        }
	}
}
