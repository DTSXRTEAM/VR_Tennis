using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform startPoint; // 👈 NEW

    Vector3 initialPos;

    private void Start()
    {
        if (startPoint != null)
        {
            initialPos = startPoint.position;
            transform.position = startPoint.position; // 👈 start here
        }
        else
        {
            initialPos = transform.position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            transform.position = initialPos;
        }
    }
}