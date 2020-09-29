using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    // 1 уровень сложности - лето
    public GameObject[] safePlatforms;
    public GameObject[] obstaclePlatforms;
    public GameObject[] decorPlatforms;

    // 2 уровень сложности - осень
    public GameObject[] safePlatforms_automn;
    public GameObject[] obstaclePlatforms_automn;
    public GameObject[] decorPlatforms_automn;

    // 3 уровень сложности - зима
    public GameObject[] safePlatforms_winter;
    public GameObject[] obstaclePlatforms_winter;
    public GameObject[] decorPlatforms_winter;

    public GameObject[] flags;

    public Transform platformContainer;

    float countPlatform = 0f;

    bool isAutomn = false;
    bool isWinter = false;

    public ParticleSystem snowParticle;

    Transform lastPlatform = null;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    private void Update()
    {
        // 2 уровень сложности - осень
        if (WorldController.instance.speed >= 9 && !isAutomn)
        {
            safePlatforms = safePlatforms_automn;
            obstaclePlatforms = obstaclePlatforms_automn;
            decorPlatforms = decorPlatforms_automn;
            isAutomn = true;

            CreateGroupFlags(flags[1]);

        }
        // 3 уровень сложности - зима
        if (WorldController.instance.speed >= 11 && !isWinter)
        {
            safePlatforms = safePlatforms_winter;
            obstaclePlatforms = obstaclePlatforms_winter;
            decorPlatforms = decorPlatforms_winter;
            isWinter = true;
            snowParticle = Instantiate(snowParticle, PlayerController.instance.transform.position, Quaternion.identity, PlayerController.instance.transform);
            snowParticle.Play();

            CreateGroupFlags(flags[2]);
        }

    }

    /// <summary>
    /// Начальная генерация уровня
    /// </summary>
    public void Init ()
    {
        CreateSafePlatform();
        CreateSafePlatform();
        CreateSafePlatform();
        CreateGroupFlags(flags[0]);
        for (int i = 0; i < 10; i++)
        {
            CreatePlatform();
        }
    }

    /// <summary>
    /// Создание платформ
    /// </summary>
    public void CreatePlatform()
    {
        switch (countPlatform % 3)
        {
            case 0:
                CreateObstaclePlatform();
                break;
            case 1:
                CreateSafePlatform();
                break;
            case 2:
                CreateSafePlatform();
                break;
        }
        countPlatform++;
    }

    /// <summary>
    /// Создание безопасной платформы
    /// </summary>
    void CreateSafePlatform ()
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPoint.position;
        int index = Random.Range(0, safePlatforms.Length);
        GameObject result = Instantiate(safePlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = result.transform;
        CreateDecorPlatform(20, 5);
        CreateDecorPlatform(-20, 5);
    }

    /// <summary>
    /// Создание опасной платформы
    /// </summary>
    void CreateObstaclePlatform()
    {
        Vector3 pos = (lastPlatform == null) ?
            platformContainer.position :
            lastPlatform.GetComponent<PlatformController>().endPoint.position;
        int index = Random.Range(0, obstaclePlatforms.Length);
        GameObject result = Instantiate(obstaclePlatforms[index], pos, Quaternion.identity, platformContainer);
        lastPlatform = result.transform;
        CreateDecorPlatform(20, 5);
        CreateDecorPlatform(-20, 5);
    }

    /// <summary>
    /// Создание декаративной платформы
    /// </summary>
    void CreateDecorPlatform(float cordX, float cordZ)
    {
        Vector3 pos = lastPlatform.position + Vector3.forward * cordZ + Vector3.right * cordX;
        int index = Random.Range(0, decorPlatforms.Length);
        Instantiate(decorPlatforms[index], pos, Quaternion.identity, lastPlatform);
    }
    /// <summary>
    /// Создаёт флаги по бокам
    /// </summary>
    void CreateFlags(GameObject flag, float sideShift, float forwardShift)
    {
        Vector3 pos = lastPlatform.position + Vector3.right * sideShift +Vector3.forward*forwardShift; 
        Instantiate(flag,  pos, flag.transform.rotation, lastPlatform);
        pos += Vector3.left*pos.x*2;
        Instantiate(flag, pos, flag.transform.rotation, lastPlatform);
    }

    void CreateGroupFlags (GameObject flag)
    {
        CreateFlags(flag, 4.5f, 0);
        CreateFlags(flag, 4.5f, 1);
        CreateFlags(flag, 4.5f, 2);
        CreateFlags(flag, 4.5f, 3);
        CreateFlags(flag, 4.5f, 4);
    }
}
