using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemiesCounter : MonoBehaviour
{
    int _count = 0;

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
            Restart();
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}