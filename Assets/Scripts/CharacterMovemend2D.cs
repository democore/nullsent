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

    public AudioClip JumpSound;

    AudioSource source;

    CharacterResource resource;

    float curNotMovedTime = 0f;
    
    // Use this for initialization hi bearcore
    void Start ()
    {
        resource = GetComponent<CharacterResource>();
        animator = GetComponentInChildren<Animator>();
        visual = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponent<CharacterController>();
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {   
        float horizontal = Input.GetAxis("Horizontal");       
        if (!resource.IsAlive)
            horizontal = 0f;

        if (controller.isGrounded)
            gravity = -0.01f;
        else
        {
            gravity -= Gravity * Time.deltaTime;           
        }           

        if(Input.GetAxis("Jump") != 0f && controller.isGrounded)
        {
            gravity = JumpPower;
            animator.SetTrigger("Jump");                    
            source.clip = JumpSound;
            source.Play();
        }

        if (horizontal != 0f)
        {
            visual.flipX = horizontal < 0f;
            
            animator.SetBool("Moving", true);

            curNotMovedTime = 0f;
        }
        else
        {
            animator.SetBool("Moving", false);

            curNotMovedTime += Time.deltaTime;
        }
        animator.SetFloat("MoveDir", horizontal);

        if (gravity < maxGravity)
            gravity = maxGravity;

        controller.Move(new Vector3(horizontal * speed * Time.deltaTime, gravity, 0f));    
    }

    private void LateUpdate()
    {
        if (wasGrounded && !controller.isGrounded)
        {
            wasGrounded = false;
            animator.SetBool("Grounded", false);
        }
        else if (!wasGrounded && controller.isGrounded) // This will only run on the frame that we land.
        {
            animator.SetTrigger("Land");
            animator.SetBool("Grounded", true);
            wasGrounded = true;
        }
    }
}
