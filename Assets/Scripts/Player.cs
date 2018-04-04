using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    // This script handles every piece of player input
    public int health;
    public int energy;
    private int maxHealth;
    private int maxEnergy;
    private float turnSpeed;
    private float thrustSpeed;
    public GameObject projectile;

    private float shootTimer;
    public float roundsPerSecond;
    private float energyTimer;
    public float energyPerSecond;

    private Rigidbody2D rigidbody2d;

    private float lowSpeed;

    public bool timeFrozen;

    private void Start()
    {
        // TODO: Remove thrustspeed and turnspeed declarations out of update when the movement system is designed
        maxHealth = 10;
        maxEnergy = 50;
        health = maxHealth;
        energy = maxEnergy;
        lowSpeed = 0.2f;

        shootTimer = 0.0f;
        energyTimer = 0.0f;

        timeFrozen = false;

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        thrustSpeed = 20;
        turnSpeed = 200;

         // The player moves backwards at reduced speed
         if (inputVertical < 0) {
            inputVertical = 0;
         }

         transform.position += transform.up * inputVertical * Time.deltaTime * thrustSpeed;
         transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * turnSpeed);
        // regular movement slower + afterburners
        // Add particle effects for ship

        /* EXPERIMENTAL MOVEMENT SYSTEM
         * thrustSpeed = 120;
        turnSpeed = 200;

        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        if (inputVertical > 0 ) {
            inputVertical = 0.01f;
        } else {
            inputVertical = 0;
        }

        transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * turnSpeed);

        if (rigidbody2d.velocity.x > lowSpeed || rigidbody2d.velocity.y > lowSpeed) {
            rigidbody2d.AddForce(2 * (transform.up * thrustSpeed * inputVertical));
        } else {
            rigidbody2d.AddForce(transform.up * thrustSpeed * inputVertical);
        }
        
        if (Input.GetKeyDown("e")) {
            rigidbody2d.velocity = Vector3.zero;
        }
        */
        if (energy < maxEnergy) {
            energyTimer += Time.deltaTime;
            if (energyTimer >= energyPerSecond) {
                energy += 1;
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

        if (Input.GetKeyDown("t")) {
            Debug.Log("Time toggled");
            if (timeFrozen)
            {
                timeFrozen = false;
                Time.timeScale = 1.0f;
            } else {
                timeFrozen = true;
                Time.timeScale = 0.5f;
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
                energy += 20;
            }
            if (other.gameObject.GetComponent<Pickup>().health) {
                health += 2;
            }
        }
    }
}