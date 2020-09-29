using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    public float speed = 8f;
    public float speedAdd = 0.5f;
    public float minZ = -20;
    public WorldBuilder worldBuilder;

    public delegate void TryToDelAndAddPlatform();
    public event TryToDelAndAddPlatform OnPlatformMovement;

    // синглтон
    public static WorldController instance;

    public bool isMovement = true;

    private void Awake()
    {
        if (WorldController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        WorldController.instance = this;


    }



    // Start is called before the first frame update
    void Start()
    {
        PlayerController.instance.OnSwitchMove += SwitchMove;
        StartCoroutine(OnPlatformMovementCorutine());
        StartCoroutine(SpeedAdd());
    }

    // Update is called once per frame
    void Update()
    {
       if (isMovement) transform.position -= Vector3.forward * speed * Time.deltaTime;
    }
    private void OnDestroy()
    {
        PlayerController.instance.OnSwitchMove -= SwitchMove;
    }

    /// <summary>
    /// Добавление новой и удаление старой платформ
    /// </summary>
    IEnumerator OnPlatformMovementCorutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(-minZ/speed);
            if (OnPlatformMovement != null) OnPlatformMovement();
        }
    }

    /// <summary>
    /// Увеличение скорости движения
    /// </summary>
    IEnumerator SpeedAdd()
    {
        while (!PlayerController.instance.death) 
        {
            yield return new WaitForSeconds(10f);
            if (speed < 8f) speed += (speedAdd / 2); // Если вдруг скорость добавляется во время падение, которое уменьшает скорость на 2;
            else speed += speedAdd;
            PlayerController.instance.SetSpeedAnimation((speed + speedAdd) / speed);
        }
    }
    
    /// <summary>
    /// Включает и выключает движение
    /// </summary>
    void SwitchMove()
    {
        isMovement = !isMovement;
    }

}
