using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject Menu_Main;
    [SerializeField] GameObject Menu_Level_Select;
    [SerializeField] GameObject Menu_Options;

    public AudioManager am;
    /*void Awake()
    {
        GameObject am = GameObject.Find("AudioManager");
    }*/

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
        am.onMouseClick();
    }
    public void OnOptionsButton ()
    {
        //DisplayOptions();
        DisplayMenu(Menu_Options);
        am.onMouseClick();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBack();
            am.onMouseClick();
        }
    }
    public void OnLevel_1()
    {
        am.onMouseClick();
        SceneManager.LoadScene("level1");
    }
    public void OnLevel_2()
    {
        am.onMouseClick();
        SceneManager.LoadScene("Jewelry Store");
    }
    public void OnLevel_3()
    {
        am.onMouseClick();
        SceneManager.LoadScene("level3");
    }
    public void OnBack()
    {
        //DisplayMainMenu();
        DisplayMenu(Menu_Main);
        am.onMouseClick();
    }
    public void setQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    public void OnQuitButton ()
    {
        am.onMouseClick();
        Application.Quit();

    }
}

