using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            GetComponent<Animator>().SetTrigger("Play");
        }
    }
}
