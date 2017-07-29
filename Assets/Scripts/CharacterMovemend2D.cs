using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovemend2D : MonoBehaviour {

    CharacterController controller;
    [Range(0.01f, 1f)]
    public float speed = 0.1f;
    // Use this for initialization hi bearcore
    void Start () {
        controller = GetComponent<CharacterController>();
        //controller.Move();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal != 0f)
        {
            controller.Move(new Vector3(horizontal * speed, 0f, 0f));
        }
    }
}
