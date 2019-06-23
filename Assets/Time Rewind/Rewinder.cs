using System.Collections.Generic;
using UnityEngine;

namespace Rewind
{
    public class Rewinder : MonoBehaviour
    {
        private List<PointInTime> registeredPoints;

        private float timeToRewindFromAPointToAnoter;
        private bool _isRewinding = false;
        private bool isRewinding
        {
            get { return _isRewinding; }
            set
            {
                if (value != _isRewinding)
                {
                    _isRewinding = value;
                    if (_isRewinding)
                    {
                        if (registerTimer >= RewindManager.REWIND_TIME)
                            registerTimer = RewindManager.REWIND_TIME;
                        timeToRewindFromAPointToAnoter = registerTimer / registeredPoints.Count;
                    }
                    else
                        registerTimer = 0;
                }
            }
        }
        private int currentPointInTimeIndex;
        private float registerTimer;

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
            if (isRewinding)
            {
                Rewind(RewindManager.Instance.timer / timeToRewindFromAPointToAnoter);
            }
            else
            {
                registerTimer += Time.fixedDeltaTime;
                Register();
            }
        }

        void Register()
        {
            if (registeredPoints.Count > Mathf.RoundToInt(RewindManager.REWIND_TIME / Time.fixedDeltaTime))
            {
                registeredPoints.RemoveAt(registeredPoints.Count - 1);
                print(name + " has too many point! removed one.");
            }

            registeredPoints.Insert(0, new PointInTime(transform.position, transform.rotation));
            print(name + " registered a point!");
        }

        void Rewind(float _currentPointInTimePlusLerpPercentage)
        {
            currentPointInTimeIndex = Mathf.FloorToInt(_currentPointInTimePlusLerpPercentage);
            float tempLerpPercent = _currentPointInTimePlusLerpPercentage - currentPointInTimeIndex;

            if (registeredPoints.Count > 0 && currentPointInTimeIndex >= 0 && currentPointInTimeIndex + 1 <= registeredPoints.Count - 1)
            {
                PointInTime tempStart = registeredPoints[currentPointInTimeIndex];
                PointInTime tempTarget = registeredPoints[currentPointInTimeIndex + 1];

                transform.position = Vector3.Lerp(tempStart.position, tempTarget.position, tempLerpPercent);
                transform.rotation = Quaternion.Lerp(tempStart.rotation, tempTarget.rotation, tempLerpPercent);

                print(name + " rewinded from point: " + currentPointInTimeIndex + ", to point: " + (currentPointInTimeIndex + 1) + ", with a lerp precentage of: " + tempLerpPercent);
            }
            else
            {
                EndRewind();
            }
        }

        public void EndRewind()
        {
            isRewinding = false;

            for (int i = 0; i < registeredPoints.Count; i++)
            {
                if (i <= currentPointInTimeIndex)
                {
                    registeredPoints.RemoveAt(i);
                }
                else
                    break;
            }
        }

        public void StartRewind()
        {
            isRewinding = true;
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