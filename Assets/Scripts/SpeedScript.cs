using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScript : MonoBehaviour
{
    public float scrollSpeed = -5f;

    private float speed = 50f;

    // Update is called once per frame
    void Update()
    {
        float scrollY = Time.deltaTime * scrollSpeed;

        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, scrollY);
    }

    public float getSpeed()
    {
        return speed;
        speed += 50f;
    }
}
