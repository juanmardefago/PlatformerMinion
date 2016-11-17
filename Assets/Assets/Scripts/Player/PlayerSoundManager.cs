using UnityEngine;
using System.Collections;

public class PlayerSoundManager : MonoBehaviour {

    public AudioClip[] jumpSounds;
    public AudioClip[] deathSounds;
    public AudioClip[] hitSounds;
    public AudioClip[] critHitSounds;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
    public void PlayJumpSound()
    {
        int soundNumber = (Mathf.RoundToInt(Random.value * (jumpSounds.Length - 1))) ;
        audioSource.clip = jumpSounds[soundNumber];
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void PlayDeathSound()
    {
        int soundNumber = (Mathf.RoundToInt(Random.value * (deathSounds.Length - 1)));
        audioSource.clip = deathSounds[soundNumber];
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void PlayHitSound()
    {
        int soundNumber = (Mathf.RoundToInt(Random.value * (hitSounds.Length - 1)));
        audioSource.clip = hitSounds[soundNumber];
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void PlayCriticalHitSound()
    {
        int soundNumber = (Mathf.RoundToInt(Random.value * (critHitSounds.Length - 1)));
        audioSource.clip = critHitSounds[soundNumber];
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

}
