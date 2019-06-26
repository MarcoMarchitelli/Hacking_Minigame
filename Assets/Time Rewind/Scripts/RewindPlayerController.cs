using UnityEngine;

/// <summary>
/// Handles movement and collision.
/// </summary>
[RequireComponent(typeof (Rigidbody))]
public class RewindPlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector3 velocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity);
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

}