using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControls : MonoBehaviour {

    private GameObject mainMenuPrefab;

    void Awake()
    {      
        DontDestroyOnLoad(gameObject);
        mainMenuPrefab = Resources.Load<GameObject>("MainMenu");
    }

	public void MoveRight()
    {
        InputManager.Horizontal = 1f;
    }

    public void MoveLeft()
    {
        InputManager.Horizontal = -1f;
    }

    public void ResetMoveLeft()
    {
        if (InputManager.Horizontal < 0)
        {
            InputManager.Horizontal = 0f;
        }        
    }

    public void ResetMoveRight()
    {
        if (InputManager.Horizontal > 0)
        {
            InputManager.Horizontal = 0f;
        }
    }

    public void ResetJump()
    {
        InputManager.Jump = false;
    }

    public void Jump()
    {
        InputManager.Jump = true;
    }

    public void Pause()
    {
        Instantiate(mainMenuPrefab);
    }
}
