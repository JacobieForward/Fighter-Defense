using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    public GameObject projectile;
    public int followDistance;

    private float shootTime;
    private float shootTimer;

    private GameObject[] enemiesNearby;
    private GameObject closestEnemy;

	// Use this for initialization
	void Start () {
        shootTimer = 0.0f;
        shootTime = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
        enemiesNearby = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemiesNearby.Length != 0)
        {
            closestEnemy = enemiesNearby[0];
            foreach (GameObject enemy in enemiesNearby)
            {
                if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) < Vector3.Distance(gameObject.transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemy;
                }
            }
            if (Vector3.Distance(closestEnemy.transform.position, transform.position) < followDistance)
            {
                Vector3 dir = closestEnemy.transform.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.Rotate(0, 0, -90);
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootTime)
                {
                    GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
                    Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    shootTimer = 0.0f;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Colliding with player projectiles causes death
        if (other.gameObject.tag == "Enemy Projectile")
        {
            Destroy(other.gameObject);
            Death();
        }
        // Colliding with any enemy causes death
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Death();
        }
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
