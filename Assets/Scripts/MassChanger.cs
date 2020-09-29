using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Меняет массу rigidbody на случайную
/// </summary>
public class MassChanger : MonoBehaviour
{
    Rigidbody rb;
    public float minSpeedBoost = 0.4f;
    public float maxSpeedBoost = 1.5f;


    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        MassChange();
    }

    void MassChange ()
    {
            rb.mass = Random.Range(minSpeedBoost, maxSpeedBoost);
    }

}
