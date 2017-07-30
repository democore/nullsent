using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    public GameObject EndMenu;

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0f;
        InputManager.InMenu = true;
        Instantiate(EndMenu);
    }
}
