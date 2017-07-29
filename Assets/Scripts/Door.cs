using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public string NextLevelName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {

        }
    }
}
