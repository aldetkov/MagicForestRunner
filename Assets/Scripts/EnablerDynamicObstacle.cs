using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablerDynamicObstacle : MonoBehaviour
{
    bool isEnabled = false;
    public float distanceStart = 60f;
    public GameObject [] dynamicObstacles;

    float randomSpawn;
    // Start is called before the first frame update
    void Start()
    {
        SwitcherActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled && transform.TransformPoint(transform.position).z < distanceStart)
        {
            isEnabled = true;
            SwitcherActive(true);
        }
    }

    void SwitcherActive (bool needActive)
    {
        foreach (var i in dynamicObstacles)
        {
            i.SetActive(needActive);
        }
    }
    
}
