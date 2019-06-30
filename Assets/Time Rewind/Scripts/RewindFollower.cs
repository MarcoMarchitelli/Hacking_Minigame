using UnityEngine;

namespace Rewind
{
    public class RewindFollower : MonoBehaviour
    {
        public float moveSpeed;
        public float turnSpeed;

        Transform target;
        float targetAngle, angle;
        Vector3 directionToTarget;

        bool follow = false;

        void Awake()
        {
            Player player = FindObjectOfType<Player>();
            if (player)
            {
                player.OnDeath += EndFollow;
                target = player.transform;
            }
        }

        private void Start()
        {
            RewindManager.Instance.OnRewindStart += EndFollow;
            RewindManager.Instance.OnRewindEnd += StartFollow;

            StartFollow();
        }

        void Update()
        {
            if (!follow)
                return;

            //go towards target
            directionToTarget = (target.position - transform.position).normalized;
            transform.Translate(-directionToTarget * moveSpeed * Time.deltaTime);

            //face target
            targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;
            angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, Time.deltaTime * turnSpeed);
            transform.eulerAngles = Vector3.up * angle;
        }

        void StartFollow()
        {
            follow = true;
        }

        void EndFollow()
        {
            follow = false;
        }
    } 
}