using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [Tooltip("The template of gameObject")]
    public GameObject[] prefabs;

    Stack<GameObject> pool = new Stack<GameObject>();

    /// <summary>
    /// Use Create to get the gameObject you want
    /// </summary>
    /// <returns>prefab object</returns>
    public GameObject Create()
    {
        GameObject go;
        if (pool.Count == 0)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject pre = Instantiate(prefab);
            pre.SetActive(false);
            Reclaim(pre);
            //go = Instantiate(prefab);
        }
        go = pool.Pop();
        go.SetActive(true);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
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
        go.SetActive(false);
    }
}
