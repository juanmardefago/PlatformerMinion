﻿using UnityEngine;
using System.Collections;

public class EnemyCombatScript : MonoBehaviour
{

    public int health;
    public float awarenessDistance;
    public LayerMask playerLayer;
    protected Animator anim;
    public Transform playerTransform;
    protected Vector2 playerPos2D;
    protected Transform myTransform;
    protected Vector2 myPos2D;
    protected EnemyMovementBasic movementScript;
    protected float aggro = 0f;

    protected float dieDelay = 2f;
    protected float dieTimer = 0f;

    public int damage;

    protected PopupTextHandler popup;

    // Use this for initialization
    public void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        movementScript = GetComponent<EnemyMovementBasic>();
        popup = GetComponent<PopupTextHandler>();
    }

    // Update is called once per frame
    public void Update()
    {
        playerPos2D = playerTransform.position;
        myPos2D = myTransform.position;

        if (dieTimer == 0f && (aggro > 0 || Physics2D.OverlapCircle(myPos2D, awarenessDistance, playerLayer)) && !SameXAsPlayer())
        {
            movementScript.MoveWithDirection(DirectionPointingToPlayer());
        }
        else
        {
            movementScript.MoveWithDirection(Vector2.zero);
        }

        DecreaseAggro();
        CheckForDieDelay();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && dieTimer == 0f)
        {
            collision.transform.gameObject.SendMessage("TakeDamage", damage);
        }
    }

    public void TakeDamage(int damage)
    {
        DoTakeDamage(damage);
        popup.Show(damage.ToString());
    }

    public void TakeCritDamage(int damage)
    {
        DoTakeDamage(damage);
        popup.Show(damage.ToString(), Color.red);
    }

    private void DoTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        aggro = 3f;
    }

    public void Miss()
    {
        popup.Show("Miss");
    }

    public void Pushback()
    {
        movementScript.SetRecovering();
        movementScript.PushbackTo(-DirectionPointingToPlayer());
    }

    public void Die()
    {
        dieTimer += Time.deltaTime;
        //anim.SetTrigger("dying");
        movementScript.MakeKinematic();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    protected void DecreaseAggro()
    {
        if (aggro > 0)
        {
            aggro -= Time.deltaTime;
        }
        else if (aggro < 0)
        {
            aggro = 0;
        }
    }

    protected void CheckForDieDelay()
    {
        if (dieTimer > 0f && dieTimer < dieDelay)
        {
            dieTimer += Time.deltaTime;
        }
        else if (dieTimer >= dieDelay)
        {
            Destroy(gameObject);
        }
    }

    protected Vector2 DirectionPointingToPlayer()
    {
        Vector2 res;
        if (playerPos2D.x > myPos2D.x)
        {
            res = Vector2.right;
        }
        else if (myPos2D.x > playerPos2D.x)
        {
            res = Vector2.left;
        }
        else
        {
            res = Vector2.zero;
        }
        return res;
    }

    protected bool SameXAsPlayer()
    {
        return myPos2D.x >= playerPos2D.x - 0.1 && myPos2D.x <= playerPos2D.x + 0.1;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, awarenessDistance);
    }
}
