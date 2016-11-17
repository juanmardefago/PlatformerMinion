using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        if (Random.value <= critRate)
        {
            int critDamage = System.Math.Max(NormalDistribution.CalculateNormalDistRandom(damage * 4, 5),0);
            other.SendMessage("TakeCritDamage", critDamage);
            other.SendMessage("Pushback");
        } else
        {
            int normalDamage = System.Math.Max(NormalDistribution.CalculateNormalDistRandom(damage, 5),0);
            other.SendMessage("TakeDamage", normalDamage);
            other.SendMessage("Pushback");
        }
    }

    private void HitOrMiss(Collider2D other)
    {
        if(Random.value <= hitRate)
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
        other.SendMessage("ShowMiss");
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
