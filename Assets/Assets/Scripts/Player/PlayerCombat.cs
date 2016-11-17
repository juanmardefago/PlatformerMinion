using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour {

    private Animator anim;
    private bool shooting;
    public float shotCD;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public Transform handgun;
    private int health;
    public int maxHealth;
    public float hitRate;
    public float critRate;

    private PopupTextHandler popup;
    private PlayerSoundManager soundManager;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        shooting = false;
        popup = GetComponent<PopupTextHandler>();
        health = maxHealth;
        soundManager = GetComponent<PlayerSoundManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire"))
        {
            if (!shooting) StartCoroutine(Shoot());
            anim.SetBool("IsShooting", true);
        } else if (Input.GetButtonUp("Fire"))
        {
            anim.SetBool("IsShooting", false);
        }
	}

    private IEnumerator Shoot()
    {
        shooting = true;
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

    public void DoTakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
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

    public void ShowMiss()
    {
        popup.Show("Miss", Color.grey);
    }

    public void Die()
    {
        transform.position = new Vector3(-6f, -1.35f, 0f);
        health = maxHealth;
        soundManager.PlayDeathSound();
    }
}
