using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    // This script handles every piece of player input
    private int health;
    private int energy;
    public int maxHealth;
    public int maxEnergy;
    private float turnSpeed;
    private float thrustSpeed;
    public GameObject projectile;
    public GameObject turret;
    public GameObject missile;
    private GameObject station;

    private float shootTimer;
    public float roundsPerSecond;
    private float energyTimer;
    public float energyPerSecond;

    private bool afterburner;
    private bool timeFrozen;

    private void Start()
    {
        // TODO: Remove thrustspeed and turnspeed declarations out of update when the movement system is designed
        health = maxHealth;
        energy = maxEnergy;

        shootTimer = 0.0f;
        energyTimer = 0.0f;

        timeFrozen = false;
        afterburner = false;

        station = GameObject.Find("TheStation");
    }

    void FixedUpdate() {
        if (Manager.instance.tutorial)
        {
            return;
        }
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        thrustSpeed = 20;
        turnSpeed = 180;

        // The player moves backwards at reduced speed
        if (inputVertical < 0) {
           inputVertical = 0;
        }
        if (!afterburner)
        {
            transform.position += transform.up * inputVertical * Time.deltaTime * thrustSpeed;
            transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * turnSpeed);
        } else
        {
            transform.position += transform.up * inputVertical * Time.deltaTime * (thrustSpeed * 2);
            transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * (turnSpeed / 2));
        }
        
        if (energy < maxEnergy) {
            energyTimer += Time.deltaTime;
            if (energyTimer >= energyPerSecond) {
                AddEnergy(1);
                energyTimer = 0.0f;
            }
        }
        shootTimer += Time.deltaTime;
        if (Input.GetKey("space") && energy > 0 && shootTimer >= roundsPerSecond) {
            //Fire the player's primary weapon
            GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            energy -= 1;
            shootTimer = 0.0f;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            afterburner = true;
        } else
        {
            afterburner = false;
        }

        if (Input.GetKeyUp("t")) {
            if (timeFrozen)
            {
                timeFrozen = false;
                Time.timeScale = 1.0f;
            } else {
                timeFrozen = true;
                Time.timeScale = 0.5f;
            }
        }

        if (Input.GetKey("return") && energy > 11 && shootTimer >= roundsPerSecond)
        {
            //Fire the player's secondary weapon
            GameObject missileInstance = Instantiate(missile, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(missileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            energy -= 10;
            shootTimer = 0.0f;
        }

        if (Input.GetKeyUp("f"))
        {
            //Spawn a turret under the player
            if (Manager.instance.CheckPoints(100))
            {
                GameObject turretInstance = Instantiate(turret, transform.position, transform.rotation);
                Physics2D.IgnoreCollision(turretInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            } else
            {
                //play "Not enough resources" sound
            }

        }

        if (health <= 0) {
            //TODO: Explosions! Debris!
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy Projectile") {
            health -= 1;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy>().followPlayer) {
            health -= 1;
        }
        if (other.gameObject.tag == "Pickup") {
            if (other.gameObject.GetComponent<Pickup>().energy) {
                AddEnergy(20);
                
            }
            if (other.gameObject.GetComponent<Pickup>().health) {
                AddHealth(4);
            }
        }
    }

    void AddEnergy(int amount) {
        if ((amount + energy) > maxEnergy) {
            energy = maxEnergy;
        } else
        {
            energy += amount;
        }
    }

    void AddHealth(int amount) {
        if ((amount + health) > maxHealth)
        {
            health = maxHealth;
        } else
        {
            health += amount;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetEnergy()
    {
        return energy;
    }
}