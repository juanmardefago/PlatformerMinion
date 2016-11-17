using UnityEngine;
using System.Collections;

public class EnemyCombatPatrol : EnemyCombatScript {

    private EnemyPatrolMovement patrolScript;
    public GameObject rootGameObject;

    public override void Start()
    {
        anim = GetComponent<Animator>();
        myTransform = GetComponent<Transform>();
        patrolScript = GetComponent<EnemyPatrolMovement>();
        popup = GetComponent<PopupTextHandler>();
        canHit = true;
        soundManager = GetComponent<PlayerSoundManager>();
    }

    // Update is called once per frame
    public override void Update () {
        CheckForDieDelay();
    }

    protected override void CheckForDieDelay()
    {
        if (dieTimer > 0f && dieTimer < dieDelay)
        {
            dieTimer += Time.deltaTime;
        }
        else if (dieTimer >= dieDelay)
        {
            Destroy(rootGameObject);
        }
    }

    public override void Die()
    {
        dieTimer += Time.deltaTime;
        //anim.SetTrigger("dying");
        patrolScript.Die();
        GetComponent<BoxCollider2D>().enabled = false;
        soundManager.PlayDeathSound();
    }
}
