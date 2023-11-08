using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody rb;
    
    public float currentSpeed;

    [SerializeField]
    public float impulseSpeed = 30f;

    [SerializeField]
    private float pushForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;
        
        currentSpeed = rb.velocity.magnitude;

        float sidePush = Input.GetAxis("Horizontal");

        rb.AddForce(Vector3.right * sidePush * pushForce, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            rb.velocity = Vector3.zero;
            Destroy(rb);
            GameObject.Find("GameManager").GetComponent<GameManager>().Death(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Speed")
        {
            float force = other.gameObject.GetComponent<SpeedScript>().speed;
            SpeedUp(force);
            Debug.Log("Speed up!");
        }
        else if (other.gameObject.tag == "Death")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Death(1);
        }
    }

    public void SpeedUp(float force)
    {
        rb.AddForce(Vector3.forward * force, ForceMode.Acceleration);
    }
}
