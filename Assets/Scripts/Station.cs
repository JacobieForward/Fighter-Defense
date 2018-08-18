using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int health;

    private float alertTimer;
    private float alertTime;

    AudioSource audioSource;

    private void Start()
    {
        alertTimer = 0.0f;
        alertTime = 3.0f;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "Enemy Projectile")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            health -= 1;
            if (alertTimer >= alertTime)
            {
                // play alert sound
                if (!Manager.instance.muteToggle.isOn)
                {
                    audioSource.Play(0);
                }
                alertTimer = 0.0f;
            }
        }
    }

    private void Update()
    {
        alertTimer += Time.deltaTime;
    }
}