using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Rider.Unity.Editor;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    private Boolean isGrounded = false;
    private Coroutine jumpForceChange;
    private Coroutine speedChange;

    public void PowerupValueChange(Pickup.PickupType type)
    {
        if (type == Pickup.PickupType.PowerupSpeed)
            StartPowerupCoroutine(ref speedChange, ref speed, type);

        if (type == Pickup.PickupType.PowerupJump)
            StartPowerupCoroutine(ref jumpForceChange, ref jumpForce, type);
    }

    public void StartPowerupCoroutine(ref Coroutine InCoroutine, ref float inVar, Pickup.PickupType type)
    {
        if (InCoroutine != null)
        {
            StopCoroutine(InCoroutine);
            InCoroutine = null;
            inVar /= 2;
        }

        InCoroutine = StartCoroutine(PowerupChange(type));
    }

    IEnumerator PowerupChange(Pickup.PickupType type)
    {
        //this code runs before the wait
        if (type == Pickup.PickupType.PowerupSpeed)
            speed *= 2;

        if (type == Pickup.PickupType.PowerupJump)
            jumpForce *= 2;

        Debug.Log($"Jump force value is {jumpForce}, Speed value is {speed}");

        yield return new WaitForSeconds(5.0f);

        if (type == Pickup.PickupType.PowerupSpeed)
        {
            speed /= 2;
            speedChange = null;
        }
        if (type == Pickup.PickupType.PowerupJump)
        {
            jumpForce /= 2;
            jumpForceChange = null;
        }

        Debug.Log($"Jump force value is {jumpForce}, Speed value is {speed}");
    }

    //Private Lives Variable
    private int _lives = 10;

    //public variable for getting and setting lives
    public int lives
    {
        get
        {
            return _lives;
        }
        set
        {
            //all lives lost (zero counts as a life due to the check)
            if (value < 0)
            {
                //game over function called here
                //return to prevent the rest of the function to be called
                return;
            }

            //lost a life
            if (value < _lives)
            {
                //Respawn function called here
            }

            //cannot roll over max lives
            if (value > maxLives)
            {
                value = maxLives;
            }

            _lives = value;

            Debug.Log($"Lives value on {gameObject.name} has changed to {lives}");
        }
    }

    //max lives that are possible
    [SerializeField] private int maxLives = 10;

    //Movement
    [SerializeField, Range (1, 20)]
    private float speed = 5;
    private Transform groundCheck;
    [SerializeField, Range(1, 20)]
    private float jumpForce = 10;
    [SerializeField, Range(0.01f, 1)]
    private float groundCheckRadius = 0.02f;

    [SerializeField]
    private LayerMask isGroundLayer;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        Debug.Log(rb.name);

        if (speed <= 0)
        {
            speed = 5;
            Debug.Log("Speed was set incorrectly");
        }

        if (jumpForce <= 0)
        {
            jumpForce = 5;
            Debug.Log("JumpForce was set incorrectly");
        }

        if (!groundCheck)
        {
            GameObject obj = new GameObject();
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.name = "GroundCheck";
            groundCheck = obj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Ground touch check
        if (!isGrounded)
        {
            if (rb.velocity.y <= 0)
            {
                isGrounded = IsGrounded();
            }
        }
        else
            isGrounded = IsGrounded();

       



        float hInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(hInput * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //Sprite Flipping
        if (hInput != 0) sr.flipX = (hInput < 0);
        // if (hInput > 0 && sr.flipX || hInput < 0 && !sr.flipX) sr.flipX = !sr.flipX;

        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
        if (Input.GetButtonDown("Fire1") && isGrounded)
        {
            anim.SetTrigger("aInput");
        }


        if (Input.GetButtonDown("Fire1") && !isGrounded)
        {
            anim.SetTrigger("AirInput");
        }




        bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);
        }
    }
}
