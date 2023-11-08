using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidewaysObstacle : MonoBehaviour
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    float leftMargin = -3f;

    [SerializeField]
    float rightMargin = 3f;

    public Transform[] obstacles;
    public float[] speeds;

    
    void Start()
    {
        speeds = new float[obstacles.Length];
        for (int i = 0; i < obstacles.Length; i++)
        {
            speeds[i] = speed * (i % 2 == 0 ? 1 : -1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].Translate(speeds[i] * speed * Time.deltaTime, 0, 0, Space.Self);
            if (obstacles[i].position.x <= leftMargin)
            {
                speeds[i] *= -1;
            }
            else if (obstacles[i].position.x >= rightMargin)
            {
                speeds[i] *= -1;
            }
        }
    }
}
