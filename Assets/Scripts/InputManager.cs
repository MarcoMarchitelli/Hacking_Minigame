using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public ControllerDetectionUI controllerDetectionUI;

    bool found, connected;
    string[] controllers;

    private void Awake()
    {
        Cursor.visible = false;
        StartCoroutine("ControllerCheckRoutine");

        foreach (string deviceName in Input.GetJoystickNames())
        {
            if (deviceName.Contains("XBOX"))
            {
                found = true;
                break;
            }
        }

        connected = found;

        if(!found)
            controllerDetectionUI.DisconnectionPopUp();
    }

    IEnumerator ControllerCheckRoutine()
    {
        while (true)
        {
            found = false;
            controllers = Input.GetJoystickNames();

            foreach (string deviceName in controllers)
            {
                if (deviceName.Contains("XBOX"))
                {
                    found = true;
                    break;
                }
            }

            if(connected && !found)
            {
                controllerDetectionUI.DisconnectionPopUp();
                connected = false;
            }
            else if(!connected && found)
            {
                controllerDetectionUI.ConnectionPopDown();
                connected = true;
            }

            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i] = null;
            }

            yield return .5f;
        }
    }
}