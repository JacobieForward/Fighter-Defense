using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int health;

    private float alertTimer;
    private float alertTime;

    public List<GameObject> debrisList;

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
            Manager.instance.IncrementStationHealthLost();
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

    public void SpawnDebris(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(debrisList[Random.Range(0, debrisList.Count - 1)], transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        alertTimer += Time.deltaTime;
        transform.Rotate(transform.forward * Time.deltaTime * 3);
    }
}