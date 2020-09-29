using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimePrint : MonoBehaviour
{
    Text text;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerController.instance.death) time += Time.deltaTime;
        text.text = $"Время: {time: 00.00}";
    }
}
