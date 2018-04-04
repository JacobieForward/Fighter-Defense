﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // This encompases all enemies with variables checked the specify what kind of enemy
    public int health;
    public float speed;
    public float followDistance;
    public bool followPlayer;
    public float chanceToDropEnergy;
    public float chanceToDropHealth;

    public GameObject energyPickup;
    public GameObject healthPickup;

    private float shootTime;
    private float shootTimer;

    public GameObject projectile; // If there is a projectile object attached then the Enemy will shoot at the player

    private float distanceBetweenEnemies;
    private GameObject[] enemiesNearby;

    private void Start()
    {
        shootTimer = 0.0f;
        shootTime = 2.0f;
        distanceBetweenEnemies = 5.0f;
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Projectile") {
            health -= 1;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Player" && followPlayer) {
            Destroy(gameObject);
        }
    }

    void Update() {
        shootTimer += Time.deltaTime;
        enemiesNearby = GameObject.FindGameObjectsWithTag("Enemy");
        if (health <= 0) {
            // Chance to drop an energy pack
            if (Random.Range(0 , 1.0f) < chanceToDropEnergy) {
                Instantiate(energyPickup, transform.position, transform.rotation);
            }
            // chance to drop a health pack
            if (Random.Range(0, 1.0f) < chanceToDropHealth)
            {
                Instantiate(healthPickup, transform.position, transform.rotation);
            }
            // TODO: Explosions! Maybe debris? Maybe fire? Though fire doesn't exist in vacuum. Maybe plasma? Gasses decompressing?
            Destroy(gameObject);
        }
        if(followPlayer && Manager.instance.player != null) {
            if ((Vector3.Distance(Manager.instance.player.transform.position, transform.position) < followDistance)) {
                // Move towards the player while keeping distance from other enemies
                float closestEnemy = 10.0f;
                GameObject closestEnemyObject = null;
                foreach (GameObject enemy in enemiesNearby)
                {
                    if ((Vector3.Distance(enemy.transform.position, transform.position) < closestEnemy) && (enemy != this.gameObject))
                    {
                        closestEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                        closestEnemyObject = enemy;
                    }
                }
                if ((closestEnemy <= distanceBetweenEnemies))
                {
                    Debug.Log(closestEnemy);
                    Vector3 dir = (transform.position - closestEnemyObject.transform.position).normalized;
                    Debug.Log(-dir);
                    transform.position +=  dir * speed * Time.deltaTime;
                } else {
                    transform.position = Vector3.MoveTowards(transform.position, Manager.instance.player.transform.position, speed * Time.deltaTime);
                }
            }
        }

        if (projectile != null && Manager.instance.player != null) {
            Vector3 dir = Manager.instance.player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.Rotate(0, 0, -90);
            if (shootTimer >= shootTime)
            {
                GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
                Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                shootTimer = 0.0f;
            }
        }
    }
}