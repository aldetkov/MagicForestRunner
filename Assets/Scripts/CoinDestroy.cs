using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDestroy : MonoBehaviour
{
    public ParticleSystem destroyCoin;

    private void Start()
    {
        // Создание партикла уничтожения монеты
        if (destroyCoin != null)
        {
            destroyCoin = Instantiate(destroyCoin, transform.position, Quaternion.identity, transform.parent);
        }
    }

    private void OnDestroy()
    {
        // Запуск партикла уничтожения монеты
        destroyCoin.Play();
    }

}
