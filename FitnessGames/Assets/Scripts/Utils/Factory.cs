using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [Tooltip("The template of gameObject")]
    public GameObject prefab;

    Stack<GameObject> pool = new Stack<GameObject>();

    /// <summary>
    /// Use Create to get the gameObject you want
    /// </summary>
    /// <returns>prefab object</returns>
    public GameObject Create()
    {
        GameObject go;
        if (pool.Count > 0)
        {
            go = pool.Pop();
        }
        else
        {
            go = Instantiate(prefab);
        }
        go.SetActive(true);
        return go;
    }

    /// <summary>
    /// Use Reclaim to "Destroy" the GameObject
    /// </summary>
    /// <param name="go"></param>
    public void Reclaim(GameObject go)
    {
        pool.Push(go);
        go.transform.position = new Vector3(1000, 1000, 1000);
        //Reset speed
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        go.SetActive(false);
    }
}
