using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

    public GameObject OptionsMenuPrefab;

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {
        Instantiate(OptionsMenuPrefab);
        Destroy(gameObject.transform.parent.gameObject);
    }

	public void Exit()
    {
        Application.Quit();
    }
}
