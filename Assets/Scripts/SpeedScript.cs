using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedScript : MonoBehaviour
{
    public float scrollSpeed = -5f;

    public float speed = 50f;

    // Update is called once per frame
    void Update()
    {
        float scrollY = Time.deltaTime * scrollSpeed;

        GetComponent<Renderer>().material.mainTextureOffset += new Vector2(0f, scrollY);
    }
}
