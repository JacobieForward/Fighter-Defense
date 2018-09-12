using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // This encompases all enemies with variables checked the specify what kind of enemy
    public float speed;
    public float followDistance;
    public bool followPlayer;
    private float chanceToDropEnergy;
    private float chanceToDropHealth;
    public bool circlePlayer;

    private float circlePauseTime;
    private float circlePauseTimer;

    public GameObject energyPickup;
    public GameObject healthPickup;
    private GameObject station;

    private float shootTime;
    private float shootTimer;

    public GameObject projectile; // If there is a projectile object attached then the Enemy will shoot at the player

    private float distanceBetweenEnemies;
    private GameObject[] enemiesNearby;

    private GameObject[] playersNearby;
    private GameObject closestPlayer;

    public List<GameObject> debrisList;

    private void Start()
    {
        shootTimer = 0.0f;
        shootTime = Random.Range(2.0f, 3.0f);
        chanceToDropEnergy = 0.15f;
        chanceToDropHealth = 0.07f;
        distanceBetweenEnemies = 5.0f;
        station = GameObject.Find("TheStation");
    }

    void OnCollisionEnter2D(Collision2D other) {
        // Colliding with player projectiles causes death
        if (other.gameObject.tag == "Projectile") {
            Destroy(other.gameObject);
            Death();
        }
        // Colliding with the station causes death, does damage to station (seen in station script)
        if (other.gameObject.name == "TheStation")
        {
            Death();
        }
        // Collision with the player only causes death if the enemy is a Mine enemy
        if (other.gameObject.tag == "Player" && followPlayer) {
            Death();
        }

        if (other.gameObject.tag == "Station")
        {
            Death();
        }
    }

    void Update() {
        playersNearby = GameObject.FindGameObjectsWithTag("Player");
        if (playersNearby.Length != 0)
        {
            closestPlayer = playersNearby[0];
            foreach (GameObject player in playersNearby)
            {
                if (Vector3.Distance(gameObject.transform.position, player.transform.position) < Vector3.Distance(gameObject.transform.position, closestPlayer.transform.position))
                {
                    closestPlayer = player;
                }
            }
            RotateToTClosestPlayer();
            enemiesNearby = GameObject.FindGameObjectsWithTag("Enemy");
            if (followPlayer && Manager.instance.player != null)
            {
                if (Vector3.Distance(closestPlayer.transform.position, transform.position) < followDistance)
                {
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
                    if ((closestEnemy <= distanceBetweenEnemies) && (Vector3.Distance(closestPlayer.transform.position, transform.position) > 3.0))
                    {
                        Vector3 dir = (transform.position - closestEnemyObject.transform.position).normalized;
                        transform.position += dir * speed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, closestPlayer.transform.position, speed * Time.deltaTime);
                    }
                }
            }

            if (projectile != null && Manager.instance.player != null && (Vector3.Distance(closestPlayer.transform.position, transform.position) < followDistance))
            {
                RotateToTClosestPlayer();
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootTime)
                {
                    GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
                    Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    shootTimer = 0.0f;
                }
                if (circlePlayer && shootTimer <= 2.5f)
                {
                    transform.RotateAround(closestPlayer.transform.position, Vector3.forward, 20 * Time.deltaTime);
                }
            }
            else
            {
                shootTimer = 0.0f;
            }
        }
        else
        {
            MoveToStation();
        }
        if (Manager.instance.player != null && (Vector3.Distance(closestPlayer.transform.position, transform.position) > followDistance))
        {
            MoveToStation();
        }
        if (Manager.instance.player == null)
        {
            MoveToStation();
        }
    }

    public void Death()
    { 
        float dropChance = Random.Range(0, 1.0f);
        // Chance to drop a health pack
        if (dropChance < chanceToDropHealth)
        {
            Instantiate(healthPickup, transform.position, transform.rotation);
        }
        // chance to drop an energy pack
        else if (dropChance < chanceToDropEnergy)
        {
            Instantiate(energyPickup, transform.position, transform.rotation);
        }
        if (gameObject.name.Contains("Mine"))
        {
            Manager.instance.mineKillCount += 1;
        }
        else if (gameObject.name.Contains("Turret"))
        {
            Manager.instance.turretKillCount += 1;
        }
        else if (gameObject.name.Contains("Fighter"))
        {
            Manager.instance.fighterKillCount += 1;
        }
        Manager.instance.AddPoints(5);
        GameObject explosionInstance = Instantiate(Manager.instance.deathExplosionPrefab, transform.position, transform.rotation);
        if (Manager.instance.muteToggle.isOn)
        {
            explosionInstance.GetComponent<AudioSource>().mute = true;
        }
        SpawnDebris(Random.Range(1, debrisList.Count));
        Destroy(gameObject);
    }

    void MoveToStation()
    {
        transform.position = Vector3.MoveTowards(transform.position, station.transform.position, (speed / 1.5f) * Time.deltaTime);
    }

    void RotateToTClosestPlayer()
    {
        Vector3 dir = closestPlayer.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.Rotate(0, 0, -90);
    }

    void SpawnDebris(int numOfDebris)
    {
        if (debrisList.Count != 0)
        {
            List<GameObject> tempDebrisList = debrisList;
            GameObject debris = tempDebrisList[0];
            for (int i = 0; i < numOfDebris; i++)
            {
                if (tempDebrisList.Count == 0)
                {
                    debris = debrisList[0];
                }
                else
                {
                    debris = tempDebrisList[Random.Range(0, tempDebrisList.Count)];
                }
                if (debris != null)
                {
                    Instantiate(debris, transform.position, transform.rotation);
                }
                tempDebrisList.Remove(debris);
            }
        } else
        {
            return;
        }
    }
}