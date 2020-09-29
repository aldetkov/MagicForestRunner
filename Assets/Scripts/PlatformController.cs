using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform endPoint;

    // Start is called before the first frame update
    void Start()
    {
        WorldController.instance.OnPlatformMovement += TryToDelAndAddPlatform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        WorldController.instance.OnPlatformMovement -= TryToDelAndAddPlatform;
    }

    /// <summary>
    /// Добавление и удаление платформ - автогенерация
    /// </summary>
    private void TryToDelAndAddPlatform ()
    {
        if (transform.position.z < WorldController.instance.minZ) 
        {
            WorldController.instance.worldBuilder.CreatePlatform();
            Destroy(gameObject);
        }
        
    }



}
