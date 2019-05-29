using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public bool infiniteRunner = false;

    List<Rewinder> rewinders = new List<Rewinder>();

    public System.Action OnRewindStart, OnRewindEnd;

    public static RewindManager Instance;
    public static float REWIND_TIME = 5f;
    public static float rewindSpeed = .5f;

    const float REWIND_COOLDOWN = 10f;
    const float RIGHT_STICK_DEADZONE = .8f;
    const float REWIND_STICK_ANGLE_SPEED_MAX = 5f;

    bool countTime = false;
    [HideInInspector] public float timer = 0;

    Vector2 previousRewindCoords;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (!infiniteRunner)
        {
            if (Input.GetButtonDown("Rewind"))
                StartRewind();
            if (Input.GetButtonUp("Rewind"))
                StopRewind();
        }
        else
        {
            Vector2 rewindCoords = new Vector2(Input.GetAxisRaw("Right Stick Horizontal"), Input.GetAxisRaw("Right Stick Vertical")).normalized;

            if (rewindCoords.sqrMagnitude >= RIGHT_STICK_DEADZONE * RIGHT_STICK_DEADZONE)
            {
                if (!countTime)
                    StartRewind();

                float angle = Vector2.Angle(rewindCoords, previousRewindCoords);
                print(angle);
                rewindSpeed = Mathf.Clamp01(angle / REWIND_STICK_ANGLE_SPEED_MAX);

                previousRewindCoords = rewindCoords;
            }
            else if (countTime)
            {
                StopRewind();
            }
        }

        if (countTime)
        {
            timer += Time.deltaTime * rewindSpeed;
            if (timer >= REWIND_TIME)
                StopRewind();
        }
    }

    public void StopRewind()
    {
        countTime = false;
        timer = 0;
        OnRewindEnd?.Invoke();
    }

    public void StartRewind()
    {
        countTime = true;
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
}