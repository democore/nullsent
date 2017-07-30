using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

    public GameObject OptionsMenuPrefab;

    private void Start()
    {
        AudioManager.Instance.PlayMusic("MainMenu");
    }

    public void NewGame()
    {
        AudioManager.Instance.PlayEffect("beep");
        AudioManager.Instance.PlayMusic("Ingame");
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
