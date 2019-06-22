using System.Collections.Generic;
using UnityEngine;

namespace Rewind
{
    public class Rewinder : MonoBehaviour
    {
        private List<PointInTime> registeredPoints;

        private float timeToRewindFromAPointToAnoter;
        private bool _isRewinding = false;
        private bool IsRewinding
        {
            get { return _isRewinding; }
            set
            {
                if (value != _isRewinding)
                {
                    _isRewinding = value;
                    if (_isRewinding)
                        timeToRewindFromAPointToAnoter = RewindManager.REWIND_TIME / registeredPoints.Count;
                }
            }
        }
        private int currentTargetPointInTimeIdex;

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
                Rewind(RewindManager.Instance.timer / timeToRewindFromAPointToAnoter);
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

        void Rewind(float _currentPointInTimePlusLerpPercentage)
        {
            //TODO: test this new method by printing all this data.
            currentTargetPointInTimeIdex = Mathf.FloorToInt(_currentPointInTimePlusLerpPercentage);
            float tempLerpPercent = _currentPointInTimePlusLerpPercentage - currentTargetPointInTimeIdex;

            if (registeredPoints.Count > 0 && currentTargetPointInTimeIdex >= 0 && currentTargetPointInTimeIdex <= registeredPoints.Count - 1)
            {
                PointInTime temp = registeredPoints[currentTargetPointInTimeIdex];
                transform.position = Vector3.Lerp(transform.position, temp.position, tempLerpPercent);
                transform.rotation = Quaternion.Lerp(transform.rotation, temp.rotation, tempLerpPercent);

                //switch (RewindManager.RewindDirection)
                //{
                //    case 1:
                //        if(currentRewindLerpPercent < previuosRewindLerpPercent)
                //            currentTargetPointInTimeIdex++;
                //        break;
                //    case -1:
                //        if(currentRewindLerpPercent > previuosRewindLerpPercent)
                //            currentTargetPointInTimeIdex--;
                //        break;
                //}

                //previuosRewindLerpPercent = currentRewindLerpPercent;
            }
            else
            {
                EndRewind();
            }
        }

        public void EndRewind()
        {
            while (currentTargetPointInTimeIdex > 0)
            {
                registeredPoints.RemoveAt(0);
                currentTargetPointInTimeIdex--;
            }

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