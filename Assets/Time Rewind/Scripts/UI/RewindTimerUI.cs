using UnityEngine;
using TMPro;
using Rewind;

public class RewindTimerUI : MonoBehaviour
{
    public TextMeshProUGUI rewindTimerText;

    // Update is called once per frame
    void Update()
    {
        rewindTimerText.text = RewindManager.Instance.timer.ToString("f2");
    }
}