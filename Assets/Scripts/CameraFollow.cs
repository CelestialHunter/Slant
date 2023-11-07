using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;

    private Vector3 minOffset = new Vector3(0, 2, -2);
    private Vector3 maxOffset = new Vector3(0, 5, -5);

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position + minOffset;
    }

    // Update is called once per frame
    void Update()
    {
        offset = Vector3.Lerp(minOffset, maxOffset, Mathf.SmoothStep(0, 1, 1 - player.GetComponent<PlayerControl>().startSpeed / player.GetComponent<PlayerControl>().currentSpeed));
        transform.position = player.position + offset;
    }
}
