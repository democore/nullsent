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
    float maxGravity = -0.5f;

    public float JumpPower = 0.4f;

    bool wasGrounded = true;

    CharacterResource resource;

    float curNotMovedTime = 0f;

    public AnimationCurve CameraDistance;

    Camera cam;
    float curCamDistance = 0f;
    float defaultCamDistance = 0f;

    public bool IsMoving = false;
    
    // Use this for initialization hi bearcore
    void Start ()
    {
        cam = GetComponentInChildren<Camera>();
        defaultCamDistance = cam.transform.localPosition.z;

        resource = GetComponent<CharacterResource>();
        animator = GetComponentInChildren<Animator>();
        visual = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponent<CharacterController>();
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
            AudioManager.Instance.PlayEffect("jump");
        }

        if(gravity > 0.1f || gravity < -0.1f)
        {
            curNotMovedTime -= 1f * Time.deltaTime;
            if (curNotMovedTime < CameraDistance.keys[0].time)
                curNotMovedTime = 0f;
            if (curNotMovedTime > CameraDistance.keys[1].time)
                curNotMovedTime = CameraDistance.keys[1].time;
        }

        if (horizontal != 0f)
        {
            IsMoving = true;
            visual.flipX = horizontal < 0f;
            animator.SetBool("Moving", true);

            curNotMovedTime -= 1f * Time.deltaTime;
            if (curNotMovedTime < CameraDistance.keys[0].time)
                curNotMovedTime = 0f;
            if (curNotMovedTime > CameraDistance.keys[1].time)
                curNotMovedTime = CameraDistance.keys[1].time;
        }
        else
        {
            IsMoving = false;
            animator.SetBool("Moving", false);

            curNotMovedTime += Time.deltaTime;
        }
        animator.SetFloat("MoveDir", horizontal);

        if (gravity < maxGravity)
            gravity = maxGravity;

        controller.Move(new Vector3(horizontal * speed * Time.deltaTime, gravity, 0f));

        curCamDistance = CameraDistance.Evaluate(curNotMovedTime);
        Vector3 newPos = cam.transform.localPosition;
        newPos.z = defaultCamDistance - curCamDistance;
        cam.transform.localPosition = newPos;
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
            AudioManager.Instance.PlayEffect("land");
        }
    }
}
