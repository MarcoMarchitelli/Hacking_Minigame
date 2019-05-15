using UnityEngine;

public class Follower : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    Transform target;
    float targetAngle, angle;
    Vector3 directionToTarget;

    void Awake()
    {
        target = FindObjectOfType<Player>().transform;
    }

    void Update()
    {
        //face target
        targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;
        angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * turnSpeed);
        transform.eulerAngles = Vector3.up * angle;

        //go towards target
        directionToTarget = (target.position - transform.position).normalized;
        transform.Translate(directionToTarget * moveSpeed * Time.deltaTime);
    }
}