using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    public MenuUI MainMenu;
    public MenuUI ModeMenu;
    public MenuUI TutorialSelectionMenu;
    public TextMeshProUGUI TutorialSelectionText;
    public GameObject ConfirmUI, BackUI;
    [Header("Background")]
    public GameObject MenuBackgrounds;

    private MenuUI currentMenu;
    private Vector2 activeMenuPosition;

    private const float MENU_TRANSITION_DURATION = .5f;
    /// <summary>
    /// Measured in pixels
    /// </summary>
    private const float MENU_DISTANCE = 50;
    /// <summary>
    /// Measured in unity units
    /// </summary>        
    private const float BACKGROUNDS_DISTANCE = 50;
    private const int MAIN_MENU_INDEX = 0;
    private const int NORMAL_MODE_INDEX = 1;
    private const int REWIND_MODE_INDEX = 2;
    private const int OPTIONS_MENU_INDEX = 3;

    #region Monos
    private void Awake()
    {
        Setup();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Back") && currentMenu != MainMenu)
        {
            GoToMainMenu();
        }
    }
    #endregion

    #region API
    public void Setup()
    {
        Cursor.visible = false;

        if (MainMenu)
        {
            currentMenu = MainMenu;
            activeMenuPosition = currentMenu.transform.position;
            TutorialSelectionText.transform.localScale = Vector3.zero;
            ModeMenu.transform.position = activeMenuPosition + new Vector2(MENU_DISTANCE, 0);
            BackUI.transform.localScale = Vector3.zero;
        }

    }

    /// <summary>
    /// Go to mode selection menu
    /// </summary>
    public void GoToModeMenu()
    {
        //UI
        currentMenu.DeselectAll();
        currentMenu.enabled = false;
        currentMenu.FadeButtons(false, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(activeMenuPosition.x - MENU_DISTANCE, MENU_TRANSITION_DURATION);

        currentMenu = ModeMenu;

        currentMenu.SelectFirst();
        currentMenu.FadeButtons(true, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(activeMenuPosition.x, MENU_TRANSITION_DURATION).onComplete += () => currentMenu.enabled = true;

        //BACKGROUNDS
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * NORMAL_MODE_INDEX, MENU_TRANSITION_DURATION);

        BackUI?.transform.DOScale(1, MENU_TRANSITION_DURATION).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// Go to main menu
    /// </summary>
    public void GoToMainMenu()
    {
        //UI
        currentMenu.DeselectAll();
        currentMenu.enabled = false;
        currentMenu.FadeButtons(false, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(activeMenuPosition.x + MENU_DISTANCE, MENU_TRANSITION_DURATION);

        currentMenu = MainMenu;

        currentMenu.FadeButtons(true, MENU_TRANSITION_DURATION);
        currentMenu.SelectFirst();
        currentMenu.transform.DOMoveX(activeMenuPosition.x, MENU_TRANSITION_DURATION).onComplete += () => currentMenu.enabled = true;

        //BACKGROUNDS
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * MAIN_MENU_INDEX, MENU_TRANSITION_DURATION);

        BackUI?.transform.DOScale(0, MENU_TRANSITION_DURATION).SetEase(Ease.OutCubic);
    }

    public void GoToTutorialSelectionMenu()
    {
        //UI
        currentMenu.DeselectAll();
        currentMenu.enabled = false;
        currentMenu.FadeButtons(false, MENU_TRANSITION_DURATION);
        currentMenu.transform.DOMoveX(activeMenuPosition.x - MENU_DISTANCE, MENU_TRANSITION_DURATION);

        currentMenu = TutorialSelectionMenu;

        currentMenu.SelectFirst();
        currentMenu.FadeButtons(true, MENU_TRANSITION_DURATION);

        TutorialSelectionText.transform.DOScale(1, MENU_TRANSITION_DURATION).SetEase(Ease.OutCubic).onComplete += () => currentMenu.enabled = true;

        BackUI?.transform.DOScale(0, MENU_TRANSITION_DURATION).SetEase(Ease.OutCubic);
    }

    public void BackgroundForNormalMode()
    {
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * NORMAL_MODE_INDEX, MENU_TRANSITION_DURATION);
    }

    public void BackgroundForRewindMode()
    {
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * REWIND_MODE_INDEX, MENU_TRANSITION_DURATION);
    }

    public void LoadScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}