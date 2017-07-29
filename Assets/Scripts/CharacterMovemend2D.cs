using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovemend2D : MonoBehaviour {

    CharacterController controller;
    [Range(1f, 10f)]
    public float speed = 3f;

    SpriteRenderer visual;

    Animator animator;

    float gravity = 0f;
    public float Gravity = 2f;
    float maxGravity = -0.2f;

    public float JumpPower = 0.4f;

    bool wasGrounded = true;
    
    // Use this for initialization hi bearcore
    void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        visual = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");

        if (controller.isGrounded)
        {
            if (!wasGrounded) // This will only run on the frame that we land.
            {
                animator.SetTrigger("Land");
            }

            gravity = 0;
            wasGrounded = true;           
        }            
        else
        {
            gravity -= Gravity * Time.deltaTime;           
        }           

        if(Input.GetAxis("Jump") != 0f && controller.isGrounded)
        {
            gravity = JumpPower;
            animator.SetTrigger("Jump");
            wasGrounded = false;
        }

        if (horizontal != 0f)
        {
            visual.flipX = horizontal < 0f;
            
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }
        animator.SetFloat("MoveDir", horizontal);

        if (gravity < maxGravity)
            gravity = maxGravity;

        controller.Move(new Vector3(horizontal * speed * Time.deltaTime, gravity, 0f));
    }
}
