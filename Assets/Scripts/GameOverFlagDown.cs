using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFlagDown : MonoBehaviour
{
    Rigidbody rb;

    public float speed = 10f;
    public ParticleSystem particleDown;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.down* speed);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        rb.isKinematic = true;
        particleDown.Play();
    }
}
