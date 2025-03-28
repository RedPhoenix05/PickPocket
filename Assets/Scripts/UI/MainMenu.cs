using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject Menu_Main;
    [SerializeField] GameObject Menu_Level_Select;
    [SerializeField] GameObject Menu_Options;

    public void DisplayMenu(GameObject menuToShow)
    {
        GameObject[] menus = { Menu_Main, Menu_Level_Select, Menu_Options};

        foreach (GameObject menu in menus)
        {
            menu.SetActive(menu == menuToShow);
        }
    }

    public void OnPlayButton () 
    {
        //SceneManager.LoadScene("LevelSelect");
        //DisplayLevelSelect();
        DisplayMenu(Menu_Level_Select);
    }
    public void OnOptionsButton ()
    {
        //DisplayOptions();
        DisplayMenu(Menu_Options);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
        }
    }
    public void OnLevel_1()
    {
        SceneManager.LoadScene("level1");
    }
    public void OnLevel_2()
    {
        SceneManager.LoadScene("level2");
    }
    public void OnLevel_3()
    {
        SceneManager.LoadScene("level3");
    }
    public void OnBack()
    {
        //DisplayMainMenu();
        DisplayMenu(Menu_Main);
    }
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void OnQuitButton ()
    {
        Application.Quit();
    }
}

