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

    
    //Movement
    [SerializeField, Range (1, 20)]
    private float speed = 5;
    private Transform groundCheck;
    [SerializeField, Range(1, 20)]
    private float jumpforce = 10;
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

        if (jumpforce <= 0)
        {
            jumpforce = 5;
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
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
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
