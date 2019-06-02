using UnityEngine;

namespace Rewind
{
    public class RewindRotator : MonoBehaviour
    {
        public bool rotateOnStart = true;
        public Vector3 Euler;
        public float speed;

        bool canRotate;

        private void Start()
        {
            if (rotateOnStart)
                StartRotation();

            RewindManager.Instance.OnRewindStart += StopRotation;
            RewindManager.Instance.OnRewindEnd += StartRotation;
        }

        void StartRotation()
        {
            canRotate = true;
        }

        void StopRotation()
        {
            canRotate = false;
        }

        void Update()
        {
            if (canRotate)
                transform.Rotate(Euler * speed * Time.deltaTime, Space.Self);
        }
    } 
}