using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour {

    public GameObject ClickControlsPrefab;
    public GameObject OptionsMenuPrefab;
    public GameObject startButton;

    private void Start()
    {
        InputManager.InMenu = true;
        AudioManager.Instance.PlayMusic("MainMenu");
        Time.timeScale = 0f; // Paused

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {           
            Button btn = startButton.GetComponent<Button>();
            btn.GetComponent<Button>().onClick.AddListener(Resume);
            startButton.GetComponent<Text>().text = "Resume";
        }
        else
        {
            startButton.GetComponent<Button>().GetComponent<Button>().onClick.AddListener(NewGame);
        }
    }

    void EnterGame()
    {
        Destroy(InputManager.ClickControls);

        if (Application.isMobilePlatform)
        {
            InputManager.ClickControls = Instantiate(ClickControlsPrefab);
        }    

        InputManager.InMenu = false;
        AudioManager.Instance.PlayEffect("beep");
        AudioManager.Instance.PlayMusic("Ingame");        
        Time.timeScale = 1f; // Resume time
    }

    public void Resume()
    {
        EnterGame();
        Destroy(gameObject.transform.parent.gameObject);
    }

    public void NewGame()
    {
        EnterGame();
        SceneManager.LoadScene(1);        
    }   

    public void Options()
    {
        Instantiate(OptionsMenuPrefab);
        Destroy(gameObject.transform.parent.gameObject);
        AudioManager.Instance.PlayEffect("beep");
    }

	public void Exit()
    {
        Application.Quit();
        AudioManager.Instance.PlayEffect("beep");
    }
}
