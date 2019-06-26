using UnityEngine;

public class LevelScroller : MonoBehaviour
{
    public Transform groundPrefab;
    public Transform startingGround;

    private const int GROUNDS_COUNT = 3;
    private const float LIMIT_OFFSET = 50f;

    private Vector3 botLimit;
    private float groundLength;

    private Transform[] grounds;
    private Transform firstGround, lastGround;

    int _firstGroundIndex = 0;
    int FirstGroundIndex
    {
        get { return _firstGroundIndex; }
        set
        {
            if (value != _firstGroundIndex)
            {
                _firstGroundIndex = value;
                if (_firstGroundIndex > grounds.Length - 1)
                    _firstGroundIndex = 0;
            }
        }
    }

    int _lastGroundIndex = 0;
    int LastGroundIndex
    {
        get { return _lastGroundIndex; }
        set
        {
            if (value != _lastGroundIndex)
            {
                _lastGroundIndex = value;
                if (_lastGroundIndex > grounds.Length - 1)
                    _lastGroundIndex = 0;
            }
        }
    }

    private void Awake()
    {
        groundLength = groundPrefab.localScale.z;
        botLimit = Vector3.forward * ( - groundLength - LIMIT_OFFSET );

        grounds = new Transform[GROUNDS_COUNT];
        for (int i = 0; i < GROUNDS_COUNT; i++)
        {
            grounds[i] = Instantiate(groundPrefab, startingGround.position + Vector3.forward * groundPrefab.localScale.z * (i + 1), Quaternion.identity);
        }

        firstGround = grounds[FirstGroundIndex];
        LastGroundIndex = GROUNDS_COUNT - 1;
        lastGround = grounds[LastGroundIndex];
    }

    private void Update()
    {
        if(startingGround && startingGround.position.z < botLimit.z)
        {
            Destroy(startingGround.gameObject);
        }

        if (firstGround.position.z < botLimit.z)
        {
            firstGround.position = lastGround.position + Vector3.forward * groundLength;
            GroundChange();
        }
    }

    void GroundChange()
    {
        FirstGroundIndex++;
        LastGroundIndex++;
        firstGround = grounds[FirstGroundIndex];
        lastGround = grounds[LastGroundIndex]; ;
    }
}