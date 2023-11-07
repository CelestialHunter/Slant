using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    Rigidbody rb;

    
    public float currentSpeed;

    [SerializeField]
    public float startSpeed = 30f;

    [SerializeField]
    private float pushForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(Vector3.forward * startSpeed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;
        
        currentSpeed = rb.velocity.magnitude;

        float sidePush = Input.GetAxis("Horizontal");

        rb.AddForce(Vector3.right * sidePush * pushForce, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Death")
        {
            rb.velocity = Vector3.zero;
            Destroy(rb);
        }
    }
}
