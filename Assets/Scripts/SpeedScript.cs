using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScript : MonoBehaviour
{
    public float scrollSpeed = -5f;

    private static float speed = 0f;

    [SerializeField]
    private float speedStep = 100f;
    
    // Update is called once per frame
    void Update()
    {
        float scrollY = Time.deltaTime * scrollSpeed;

        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, scrollY);
    }

    public float getSpeed()
    {
        speed += speedStep;
        return speed;
    }
}
