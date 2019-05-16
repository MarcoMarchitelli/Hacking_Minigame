using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    List<Rewinder> rewinders = new List<Rewinder>();

    public System.Action OnRewindStart, OnRewindEnd;

    public static RewindManager Instance;
    public static float REWIND_TIME = 5f;

    const float REWIND_COOLDOWN = 10f;

    bool countTime = false;
    [HideInInspector] public float timer = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Rewind"))
            StartRewind();
        if (Input.GetButtonUp("Rewind"))
            StopRewind();

        if (countTime)
        {
            timer += Time.deltaTime;
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