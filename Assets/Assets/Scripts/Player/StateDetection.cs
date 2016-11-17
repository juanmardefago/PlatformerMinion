using UnityEngine;
using System.Collections;

public class StateDetection : MonoBehaviour
{
    private PlayerMovement movementScript;
    private Animator anim;

    public void Start()
    {
        movementScript = GetComponentInParent<PlayerMovement>();
        anim = GetComponentInParent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            movementScript.grounded = true;
            if (!movementScript.IsMovingVertically())
            {
                anim.SetBool("IsJumping", false);
            }
        }
        if (other.tag == "MovingPlatform")
        {
            anim.SetBool("IsJumping", false);
            movementScript.grounded = true;
        }
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "MovingPlatform")
        {
            movementScript.grounded = true;
            if (!movementScript.IsMovingVertically())
            {
                anim.SetBool("IsJumping", false);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground" || other.tag == "MovingPlatform")
        {
            movementScript.grounded = false;
            anim.SetBool("IsJumping", true);
        }
    }
}
