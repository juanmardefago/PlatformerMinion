using UnityEngine;
using System.Collections;

public class EnemyProjectileScript : ProjectileScript {

    public override void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagged("Player", other))
        {
            HitOrMiss(other);
        }
    }

}
