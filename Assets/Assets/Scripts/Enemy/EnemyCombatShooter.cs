using UnityEngine;
using System.Collections;
using System;

public class EnemyCombatShooter : EnemyCombatScript {

    public float shootingDistance;
    public float shotCD;
    public float bulletSpeed;
    private bool shooting;

    public Transform handgun;
    public GameObject bulletPrefab;

    // Update is called once per frame
    public override void Update()
    {
        playerPos2D = playerTransform.position;
        myPos2D = myTransform.position;

        if (dieTimer == 0f && (aggro > 0 || (Physics2D.OverlapCircle(myPos2D, awarenessDistance, playerLayer) && !Physics2D.OverlapCircle(myPos2D, shootingDistance, playerLayer))) && !SameXAsPlayer())
        {
            movementScript.MoveWithDirection(DirectionPointingToPlayer());
        } else if(Physics2D.OverlapCircle(myPos2D, shootingDistance, playerLayer) && dieTimer == 0f)
        {
            if(!shooting) StartCoroutine(Shoot());
        }
        else
        {
            movementScript.MoveWithDirection(Vector2.zero);
        }

        DecreaseAggro();
        CheckForDieDelay();
    }

    private IEnumerator Shoot()
    {
        shooting = true;
        movementScript.CorrectLocalScale(DirectionPointingToPlayer().x);
        DoShoot();
        yield return new WaitForSeconds(shotCD);
        shooting = false;
    }

    private void DoShoot()
    {
        GameObject shot = Instantiate(bulletPrefab);
        shot.GetComponent<Rigidbody2D>().velocity = Vector2.right * bulletSpeed * transform.localScale.x;
        shot.transform.localScale = transform.localScale;
        shot.transform.position = handgun.position;
        shot.GetComponent<ProjectileScript>().SetRates(hitRate, critRate);
    }
}
