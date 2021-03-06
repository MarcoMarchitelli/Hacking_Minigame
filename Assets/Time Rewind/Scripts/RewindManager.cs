﻿using System.Collections.Generic;
using UnityEngine;

namespace Rewind
{
    public class RewindManager : MonoBehaviour
    {
        public enum Quadrant { topRight, topLeft, botRight, botLeft }

        List<Rewinder> rewinders = new List<Rewinder>();

        public System.Action OnRewindStart, OnRewindEnd;

        public static RewindManager Instance;
        public static float REWIND_COOLDOWN = 10f;
        public static float REWIND_TIME = 5f;
        public static float REWIND_SPEED = 1f;
        /// <summary>
        /// -1 if going back in time. 1 if going forward.
        /// </summary>
        public static int RewindDirection = -1;

        const float RIGHT_STICK_DEADZONE = .8f;
        const float REWIND_STICK_ANGLE_SPEED_MAX = 5f;
        /// <summary>
        /// When going forward in time when do we stop the rewind AFTER hitting the starting point?
        /// </summary>
        const float FORWARD_TIME_ACTIVATION_TRESHOLD = .3f;

        /// <summary>
        /// How much time we rewinded in this rewind session.
        /// </summary>
        [HideInInspector] public float timer = 0;

        private Vector2 previousRewindCoords, currentRewindCoords;
        private Quadrant currentQuadrant, previousQuadrant;
        private bool rewinding = false;
        private bool canRewind = false;

        #region Monos

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            RewindPlayer player = FindObjectOfType<RewindPlayer>();
            if (player)
            {
                player.OnDeath += () => canRewind = false;
            }

            canRewind = true;
        }

        private void Update()
        {
            if (!canRewind)
                return;

            currentRewindCoords = new Vector2(Input.GetAxisRaw("Right Stick Horizontal"), Input.GetAxisRaw("Right Stick Vertical")).normalized;

            if (currentRewindCoords.sqrMagnitude >= RIGHT_STICK_DEADZONE * RIGHT_STICK_DEADZONE)
            {
                if (!rewinding)
                    StartRewind();

                CheckQuadrant(currentRewindCoords);
                CompareQuadrants();

                float angle = Vector2.Angle(currentRewindCoords, previousRewindCoords);
                REWIND_SPEED = Mathf.Clamp01(angle / REWIND_STICK_ANGLE_SPEED_MAX);

                previousRewindCoords = currentRewindCoords;
                previousQuadrant = currentQuadrant;
            }
            else if (rewinding)
            {
                StopRewind();
            }

            if (rewinding)
            {
                //if we are going BACK in time we INCREMENT this timer, which counts how much time we are REWINDING.
                timer += Time.deltaTime * REWIND_SPEED * -RewindDirection;
                if (timer > REWIND_TIME || timer < 0 - FORWARD_TIME_ACTIVATION_TRESHOLD)
                    StopRewind();
            }

            #region Debug
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (!countTime)
            //        StartRewind();
            //    else
            //        StopRewind();
            //}

            //if (countTime)
            //{
            //    if (Input.GetKeyDown(KeyCode.A))
            //        RewindDirection = 1;
            //    else if (Input.GetKeyDown(KeyCode.D))
            //        RewindDirection = -1;

            //    //if we are going BACK in time we INCREMENT this timer, which counts how much time we are REWINDING.
            //    timer += Time.deltaTime * REWIND_SPEED * -RewindDirection;
            //    if (timer >= REWIND_TIME || timer <= 0)
            //        StopRewind();
            //}
            #endregion
        }

        #endregion

        #region Internals
        private void CheckQuadrant(Vector2 _normalizedDirection)
        {
            //HACK: this does not consider one of the two coords to be EQUALS to 0.
            if (_normalizedDirection.x > 0 && _normalizedDirection.y > 0)
                currentQuadrant = Quadrant.topRight;
            else if (_normalizedDirection.x > 0 && _normalizedDirection.y < 0)
                currentQuadrant = Quadrant.botRight;
            else if (_normalizedDirection.x < 0 && _normalizedDirection.y < 0)
                currentQuadrant = Quadrant.botLeft;
            else if (_normalizedDirection.x < 0 && _normalizedDirection.y > 0)
                currentQuadrant = Quadrant.topLeft;
        }

        private void CompareQuadrants()
        {
            switch (previousQuadrant)
            {
                case Quadrant.topRight:
                    switch (currentQuadrant)
                    {
                        case Quadrant.topRight:
                            CheckSameQuadrantCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                        case Quadrant.topLeft:
                            RewindDirection = -1;
                            break;
                        case Quadrant.botRight:
                            RewindDirection = 1;
                            break;
                        case Quadrant.botLeft:
                            CheckMirroredQuadrantsCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                    }
                    break;
                case Quadrant.topLeft:
                    switch (currentQuadrant)
                    {
                        case Quadrant.topRight:
                            RewindDirection = 1;
                            break;
                        case Quadrant.topLeft:
                            CheckSameQuadrantCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                        case Quadrant.botRight:
                            CheckMirroredQuadrantsCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                        case Quadrant.botLeft:
                            RewindDirection = -1;
                            break;
                    }
                    break;
                case Quadrant.botRight:
                    switch (currentQuadrant)
                    {
                        case Quadrant.topRight:
                            RewindDirection = -1;
                            break;
                        case Quadrant.topLeft:
                            CheckMirroredQuadrantsCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                        case Quadrant.botRight:
                            CheckSameQuadrantCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                        case Quadrant.botLeft:
                            RewindDirection = 1;
                            break;
                    }
                    break;
                case Quadrant.botLeft:
                    switch (currentQuadrant)
                    {
                        case Quadrant.topRight:
                            CheckMirroredQuadrantsCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                        case Quadrant.topLeft:
                            RewindDirection = 1;
                            break;
                        case Quadrant.botRight:
                            RewindDirection = -1;
                            break;
                        case Quadrant.botLeft:
                            CheckSameQuadrantCase(currentQuadrant, currentRewindCoords, previousRewindCoords);
                            break;
                    }
                    break;
            }
        }

        private void CheckSameQuadrantCase(Quadrant _quadrant, Vector2 _currentCoords, Vector2 _previousCoords)
        {
            switch (_quadrant)
            {
                case Quadrant.topRight:
                case Quadrant.botLeft:
                    if (Mathf.Abs(_currentCoords.x) > Mathf.Abs(_previousCoords.x))
                        RewindDirection = 1;
                    else if (Mathf.Abs(_currentCoords.x) < Mathf.Abs(_previousCoords.x))
                        RewindDirection = -1;
                    break;
                case Quadrant.topLeft:
                case Quadrant.botRight:
                    if (Mathf.Abs(_currentCoords.x) < Mathf.Abs(_previousCoords.x))
                        RewindDirection = 1;
                    else if (Mathf.Abs(_currentCoords.x) > Mathf.Abs(_previousCoords.x))
                        RewindDirection = -1;
                    break;
            }
        }

        private void CheckMirroredQuadrantsCase(Quadrant _quadrant, Vector2 _currentCoords, Vector2 _previousCoords)
        {
            switch (_quadrant)
            {
                case Quadrant.topLeft:
                case Quadrant.topRight:
                    if (Mathf.Abs(_currentCoords.x) < Mathf.Abs(_previousCoords.x))
                        RewindDirection = -1;
                    else if (Mathf.Abs(_currentCoords.x) > Mathf.Abs(_previousCoords.x))
                        RewindDirection = 1;
                    break;
                case Quadrant.botLeft:
                case Quadrant.botRight:
                    if (Mathf.Abs(_currentCoords.x) < Mathf.Abs(_previousCoords.x))
                        RewindDirection = 1;
                    else if (Mathf.Abs(_currentCoords.x) > Mathf.Abs(_previousCoords.x))
                        RewindDirection = -1;
                    break;
            }
        }
        #endregion

        #region API

        public void StopRewind()
        {
            canRewind = false;
            rewinding = false;
            timer = 0;
            OnRewindEnd?.Invoke();
            Timer.StartTimer(REWIND_COOLDOWN, () => canRewind = true);
        }

        public void StartRewind()
        {
            rewinding = true;
            RewindDirection = -1;
            OnRewindStart?.Invoke();
        }

        public void AddRewinder(Rewinder _rewinder)
        {
            rewinders.Add(_rewinder);
        }

        public void RemoveRewinder(Rewinder _rewinder)
        {
            rewinders.Remove(_rewinder);
        }

        #endregion
    }
}