using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rBody;
    private bool facingRight;
    public float speed;
    public float jumpForce;
    [HideInInspector]
    public bool grounded;
    private Animator anim;
    private PlayerSoundManager soundManager;

    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        facingRight = true;
        grounded = false;
        anim = GetComponent<Animator>();
        soundManager = GetComponent<PlayerSoundManager>();
    }

    void Update()
    {
        HorizontalMovement();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

    }

    private void HorizontalMovement()
    {
        float input = Input.GetAxis("Horizontal");
        if (input != 0)
        {
            rBody.velocity = new Vector2(input * speed, rBody.velocity.y);
            anim.SetBool("IsWalking", true);
            anim.SetFloat("speed", input);
        }
        else
        {
            rBody.velocity = new Vector2(0, rBody.velocity.y);
            anim.SetBool("IsWalking", false);
        }

        CorrectLocalScale(input);
    }

    // Puede ser usado por los state para corregir el localScale si lo necesitan
    public void CorrectLocalScale(float axis)
    {
        if ((axis > 0 && !facingRight) || (axis < 0 && facingRight))
        {
            FlipScale();
        }
    }

    private void FlipScale()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x = scale.x * -1;
        transform.localScale = scale;
    }

    private void Jump()
    {
        if (grounded)
        {
            soundManager.PlayJumpSound();
            anim.SetBool("IsJumping", true);
            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
            grounded = false;
        }
    }

    public bool IsMovingVertically()
    {
        return rBody.velocity.y != 0;
    }

    public void IsKinematic(bool status)
    {
        rBody.isKinematic = status;
    }
}
