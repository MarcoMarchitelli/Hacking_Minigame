using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesCounter : MonoBehaviour
{
    int LEVEL_COUNT;
    int _count = 0;

    private void Awake()
    {
        LEVEL_COUNT = 0;
        _count = 0;
        DontDestroyOnLoad(this);
    }

    public void AddCount()
    {
        _count++;
        CheckCount();
    }

    public void RemoveCount()
    {
        _count--;
        CheckCount();
    }

    void CheckCount()
    {
        if (_count <= 0)
            GoToNextLevel();
    }

    void GoToNextLevel()
    {
        _count = 0;
        LEVEL_COUNT++;
        //TODO: WIN SCREEN OR SOMETHING =>
        SceneManager.LoadScene("Level_" + LEVEL_COUNT);
    }
}