using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 distance;
    Vector3 currentPosTarget;

    bool isBack = false;
    // Start is called before the first frame update
    void Start()
    {
        distance = target.position - transform.position;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position - distance, 0.07f);
        currentPosTarget = target.position;
        if (PlayerController.instance.death && !isBack)
        {
            distance += Vector3.forward * 2f;
            isBack = true;
        }
    }
    
  //  IEnumerator CameraBack()
  //  {
  //      float distance = 2f;
  //      while (distance > 0)
  //      {
  //          transform.position = Vector3.Lerp(
  //      }
  //      yield return new WaitForFixedUpdate();
  //  }
}
