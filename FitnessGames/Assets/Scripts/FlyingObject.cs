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
    public void ReclaimByTime(float time = 6.0f)
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
	
    public void Reclaim()
    {
        factory.Reclaim(gameObject);
        this.enabled = false;
    }

	// Update is called once per frame
	void Update () {
		if (Time.time - spawnTime > liveTime)
        {
            Reclaim();
        }
	}
}
