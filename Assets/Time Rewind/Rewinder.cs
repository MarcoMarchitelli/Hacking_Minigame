using System.Collections.Generic;
using UnityEngine;

namespace Rewind
{
    public class Rewinder : MonoBehaviour
    {
        List<PointInTime> registeredPoints;

        float timer = 0;
        float timeToRewindFromAPointToAnoter;
        bool _isRewinding = false;
        bool IsRewinding
        {
            get { return _isRewinding; }
            set
            {
                if (value != _isRewinding)
                {
                    _isRewinding = value;
                    if (_isRewinding)
                    {
                        timeToRewindFromAPointToAnoter = RewindManager.REWIND_TIME / registeredPoints.Count;
                        timer = 0;
                    }
                }
            }
        }

        private void Start()
        {
            RewindManager.Instance.AddRewinder(this);
            RewindManager.Instance.OnRewindStart += StartRewind;
            RewindManager.Instance.OnRewindEnd += EndRewind;

            registeredPoints = new List<PointInTime>();
        }

        private void OnDisable()
        {
            RewindManager.Instance.RemoveRewinder(this);
        }

        private void FixedUpdate()
        {
            if (IsRewinding)
            {
                timer += Time.fixedDeltaTime * RewindManager.rewindSpeed;
                float percentFromCurrentPointToNext = timer / timeToRewindFromAPointToAnoter;

                Rewind(percentFromCurrentPointToNext);
            }
            else
                Register();
        }

        void Register()
        {
            if (registeredPoints.Count > Mathf.RoundToInt(RewindManager.REWIND_TIME / Time.fixedDeltaTime))
            {
                registeredPoints.RemoveAt(registeredPoints.Count - 1);
            }

            registeredPoints.Insert(0, new PointInTime(transform.position, transform.rotation));
        }

        void Rewind(float _lerpPercent)
        {
            if (registeredPoints.Count > 0)
            {
                PointInTime temp = registeredPoints[0];
                transform.position = Vector3.Lerp(transform.position, temp.position, _lerpPercent);
                transform.rotation = Quaternion.Lerp(transform.rotation, temp.rotation, _lerpPercent);

                if (_lerpPercent >= 1)
                {
                    registeredPoints.RemoveAt(0);
                    timer = 0;
                }
            }
            else
            {
                EndRewind();
            }
        }

        public void EndRewind()
        {
            IsRewinding = false;
        }

        public void StartRewind()
        {
            IsRewinding = true;
        }

        void Die()
        {
            Destroy(gameObject);
        }

    }

    public struct PointInTime
    {
        public Vector3 position;
        public Quaternion rotation;

        public PointInTime(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }
    } 
}