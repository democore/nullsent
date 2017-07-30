using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squeaker : MonoBehaviour {

    void Squeak()
    {
        AudioManager.Instance.PlayEffect("hide");
    }
}
