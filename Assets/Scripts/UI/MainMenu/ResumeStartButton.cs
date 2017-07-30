using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResumeStartButton : MonoBehaviour {

    void Awake()
    {

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GetComponent<Text>().text = "Resume";
        }
    }
}
