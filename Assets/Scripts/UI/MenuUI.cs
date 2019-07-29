using UnityEngine;

public class MenuUI : MonoBehaviour
{
    public enum NavigationType { Vertical, Horizontal }

    public bool autoSelectFirstOne = true;
    public bool fadeOutOnSetup;
    public bool disableOnSetup;
    public NavigationType navigationType;
    [Range(0, 1)]
    public float stickDeadzone;

    ButtonUI[] customButtons;
    ButtonUI currentlySelectedButton;
    int currentlySelectedButtonIndex;
    bool selectable;
    float stick, dpad;

    #region Monos
    private void Awake()
    {
        Setup();
    }

    private void Update()
    {       
        if (Mathf.Abs(stick) < stickDeadzone && dpad == 0)
            selectable = true;

        if (!selectable)
            return;

        switch (navigationType)
        {
            case NavigationType.Vertical:

                stick = Input.GetAxisRaw("Left Stick Vertical");
                dpad = Input.GetAxisRaw("DPAD Vertical");

                if (stick >= stickDeadzone || dpad == 1)
                    ChangeSelection(1);
                else if (stick <= -stickDeadzone || dpad == -1)
                    ChangeSelection(-1);

                break;
            case NavigationType.Horizontal:

                stick = Input.GetAxisRaw("Left Stick Horizontal");
                dpad = Input.GetAxisRaw("DPAD Horizontal");

                if (stick >= stickDeadzone || dpad == 1)
                    ChangeSelection(1);
                else if (stick <= -stickDeadzone || dpad == -1)
                    ChangeSelection(-1);

                break;
        }

        if (Input.GetButtonDown("Confirm"))
            currentlySelectedButton.Click();
    }
    #endregion

    void ChangeSelection(int _directionSign)
    {
        selectable = false;

        switch (Mathf.Sign(_directionSign))
        {
            case -1:
                if (currentlySelectedButtonIndex == customButtons.Length - 1)
                    currentlySelectedButtonIndex = 0;
                else
                    currentlySelectedButtonIndex++;
                break;
            case 1:
                if (currentlySelectedButtonIndex == 0)
                    currentlySelectedButtonIndex = customButtons.Length - 1;
                else
                    currentlySelectedButtonIndex--;
                break;
        }

        currentlySelectedButton?.Deselect();
        currentlySelectedButton = customButtons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();
    }

    #region API
    public void Setup()
    {
        customButtons = GetComponentsInChildren<ButtonUI>();

        foreach (ButtonUI button in customButtons)
        {
            button.Setup();
        }

        if (autoSelectFirstOne)
        {
            currentlySelectedButton = customButtons[0];
            currentlySelectedButton.Select();
        }

        if (fadeOutOnSetup)
            FadeButtons(false, 0);

        enabled = !disableOnSetup;
    }

    public void FadeButtons(bool _fadeValue, float _duration, System.Action _callback = null)
    {
        for (int i = 0; i < customButtons.Length; i++)
        {
            if (i == 0)
                customButtons[i].FadeAll(_fadeValue, _duration, _callback);
            else
                customButtons[i].FadeAll(_fadeValue, _duration);
        }
    }

    public void DeselectAll()
    {
        foreach (ButtonUI button in customButtons)
        {
            button.Deselect();
        }
    }

    public void SelectFirst()
    {
        DeselectAll();

        currentlySelectedButtonIndex = 0;
        currentlySelectedButton = customButtons[currentlySelectedButtonIndex];
        currentlySelectedButton.Select();
    }
    #endregion
}