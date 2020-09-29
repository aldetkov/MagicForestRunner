using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBooster : MonoBehaviour
{
    Rigidbody rb;
    public float minForce;
    public float maxForce;
    public Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * Random.Range(minForce, maxForce));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
