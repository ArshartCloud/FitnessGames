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
        if (pool.Count > 0)
        {
            GameObject go = pool.Pop();
            go.SetActive(true);
            return go;
        }
        else
        {
            GameObject go = Instantiate(prefab);
            go.SetActive(true);
            return go;
        }
    }

    /// <summary>
    /// Use Reclaim to "Destroy" the GameObject
    /// </summary>
    /// <param name="go"></param>
    public void Reclaim(GameObject go)
    {
        pool.Push(go);
        go.SetActive(false);
    }
}
