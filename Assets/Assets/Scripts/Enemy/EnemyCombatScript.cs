using UnityEngine;
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

    public float hitRate;
    public float critRate;
    public float recoveryTimeAfterAttack;

    private PopupTextHandler popup;
    private PlayerSoundManager soundManager;

    private bool canHit;

    // Use this for initialization
    public void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        movementScript = GetComponent<EnemyMovementBasic>();
        popup = GetComponent<PopupTextHandler>();
        canHit = true;
        soundManager = GetComponent<PlayerSoundManager>();
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
        if (canHit && collision.collider.tag == "Player" && dieTimer == 0f)
        {
            HitOrMiss(collision.gameObject);
            StartCoroutine(AttackRecovery());
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (canHit && collision.collider.tag == "Player" && dieTimer == 0f)
        {
            HitOrMiss(collision.gameObject);
            StartCoroutine(AttackRecovery());
        }
    }

    private IEnumerator AttackRecovery()
    {
        canHit = false;
        yield return new WaitForSeconds(recoveryTimeAfterAttack);
        canHit = true;
    }

    private void Hit(GameObject other)
    {
        if (Random.value <= critRate)
        {
            int critDamage = System.Math.Max(NormalDistribution.CalculateNormalDistRandom(damage * 3, 5),0);
            other.SendMessage("TakeCritDamage", critDamage);
        }
        else
        {
            int normalDamage = System.Math.Max(NormalDistribution.CalculateNormalDistRandom(damage, 5),0);
            other.SendMessage("TakeDamage", normalDamage);
        }
    }

    private void HitOrMiss(GameObject other)
    {
        if (Random.value <= hitRate)
        {
            Hit(other);
        }
        else
        {
            Miss(other);
        }
    }

    private void Miss(GameObject other)
    {
        other.SendMessage("ShowMiss");
    }

    public void TakeDamage(int damage)
    {
        soundManager.PlayHitSound();
        DoTakeDamage(damage);
        popup.Show(damage.ToString());
    }

    public void TakeCritDamage(int damage)
    {
        soundManager.PlayCriticalHitSound();
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

    public void ShowMiss()
    {
        popup.Show("Miss",Color.grey);
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
        soundManager.PlayDeathSound();
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
