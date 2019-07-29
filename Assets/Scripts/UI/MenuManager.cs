using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [Header("UI")]
    public MenuUI MainMenu;
    public MenuUI ModeMenu;
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
        if (MainMenu)
        {
            currentMenu = MainMenu;
            activeMenuPosition = currentMenu.transform.position;
            ModeMenu.transform.position = activeMenuPosition + new Vector2(MENU_DISTANCE, 0);
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

        currentMenu.FadeButtons(true, MENU_TRANSITION_DURATION);
        currentMenu.SelectFirst();
        currentMenu.transform.DOMoveX(activeMenuPosition.x, MENU_TRANSITION_DURATION).onComplete += () => currentMenu.enabled = true;

        //BACKGROUNDS
        MenuBackgrounds.transform.DOMoveY(BACKGROUNDS_DISTANCE * NORMAL_MODE_INDEX, MENU_TRANSITION_DURATION);
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