using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField]
    private float verticalAcceleration = 20.0f, horizontalAcceleration = 20.0f,
                    velocityDrag = 1.0f, rotationDrag = 1.0f,
                    maxSpeed = 50.0f, maxAngularSpeed = 50.0f,
                    alphaAcceleration = 100.0f;

    private Vector2 velocity;
    private Quaternion rotation;
    private float angularVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        Vector2 acceleration = Input.GetAxis("Vertical") * verticalAcceleration * transform.up + Input.GetAxis("Horizontal") * horizontalAcceleration * transform.right;
        velocity += acceleration * Time.deltaTime;


        Vector3 mosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rotation = Quaternion.LookRotation(Vector3.forward, mosPos - transform.position);

        angularVelocity += alphaAcceleration * Time.deltaTime;

    }

    private void FixedUpdate()
    {
        velocity = velocity *(1-Time.deltaTime*velocityDrag);
        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);

        angularVelocity = angularVelocity * (1 - Time.deltaTime * rotationDrag);
        angularVelocity = Mathf.Clamp(angularVelocity, -maxAngularSpeed, maxAngularSpeed);

        rb.velocity = velocity;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, angularVelocity);

    }
}
