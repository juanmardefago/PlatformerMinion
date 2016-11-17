using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProjectileScript : MonoBehaviour
{

    private float timer = 0f;
    public float maxTime;
    public int damage;
    public Sprite explotionSprite;
    private bool shouldBurst = false;
    private float burstTimer = 0f;
    public float burstMaxTime;
    private float hitRate;
    private float critRate;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckForDisappearTime();
        CheckForBurstTime();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTagged("Ground", other))
        {
            Burst();
        }
        else if (IsTagged("Enemy", other))
        {
            HitOrMiss(other);
        } 
    }

    private void Hit(Collider2D other)
    {
        if (UnityEngine.Random.value <= critRate)
        {
            int critDamage = CalculateNormalDistRandom(damage * 4, 2);
            other.SendMessage("TakeCritDamage", critDamage);
            other.SendMessage("Pushback");
        } else
        {
            int normalDamage = CalculateNormalDistRandom(damage, 2);
            other.SendMessage("TakeDamage", normalDamage);
            other.SendMessage("Pushback");
        }
    }

    private void HitOrMiss(Collider2D other)
    {
        if(UnityEngine.Random.value <= hitRate)
        {
            Hit(other);
            Burst();
        } else
        {
            Miss(other);
        }
    }

    private void Miss(Collider2D other)
    {
        other.SendMessage("Miss");
    }

    private int CalculateNormalDistRandom(int mean, int deviation)
    {
        double u, v, S;

        do
        {
            u = 2.0 * UnityEngine.Random.value - 1.0;
            v = 2.0 * UnityEngine.Random.value - 1.0;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        double fac = Math.Sqrt(-2.0 * Math.Log(S) / S);
        return (int) ((u * fac) * deviation + mean);
    }

    private bool IsTagged(string tag, Collider2D other)
    {
        bool res = false;
        if (other.transform.parent != null)
        {
            res = other.tag == tag || other.transform.parent.tag == tag;
        }
        else
        {
            res = other.tag == tag;
        }
        return res;
    }

    private void Burst()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = explotionSprite;
        shouldBurst = true;
        Rigidbody2D rBody = GetComponent<Rigidbody2D>();
        rBody.velocity = new Vector2(0, 0);
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void CheckForDisappearTime()
    {
        if (timer < maxTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void CheckForBurstTime()
    {
        if (shouldBurst && burstTimer < burstMaxTime)
        {
            burstTimer += Time.deltaTime;
        }
        else if (shouldBurst)
        {
            Destroy(gameObject);
        }
    }

    public void SetRates(float hit, float crit)
    {
        hitRate = hit;
        critRate = crit;
    }
}
