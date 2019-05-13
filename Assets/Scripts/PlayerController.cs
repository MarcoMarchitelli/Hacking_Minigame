using UnityEngine;
using DG.Tweening;

/// <summary>
/// Handles movement and collision.
/// </summary>
[RequireComponent(typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocity;
    float turnSpeed;
    float targetAngle;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * turnSpeed);
        transform.eulerAngles = Vector3.up * angle;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity);
    }

    public void SetTurnSpeed(float _turnSpeed)
    {
        turnSpeed = _turnSpeed;
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Aim(Vector3 _direction)
    {
        targetAngle = 90 - Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;
    }

}