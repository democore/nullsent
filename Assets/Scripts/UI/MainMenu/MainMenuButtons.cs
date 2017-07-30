using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

    public GameObject OptionsMenuPrefab;

    public void NewGame()
    {
        SceneManager.LoadScene(1);
        AudioManager.Instance.PlayEffect("beep");
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
