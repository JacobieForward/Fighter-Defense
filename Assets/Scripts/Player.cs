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

    private Rigidbody2D rigidbody2d;

    private float lowSpeed;

    private void Start()
    {
        // TODO: Remove thrustspeed and turnspeed declarations out of update when the movement system is designed
        maxHealth = 10;
        maxEnergy = 50;
        health = maxHealth;
        energy = maxEnergy;
        lowSpeed = 0.2f;

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        thrustSpeed = 20;
        turnSpeed = 200;

         // The player moves backwards at reduced speed
         if (inputVertical < 0) {
             inputVertical = inputVertical/2;
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
        if (Input.GetKeyDown("space") && energy > 0) {
            //Fire the player's primary weapon
            Instantiate(projectile, transform.position, transform.rotation);
            energy -= 1;
        }

        if (health <= 0) {
            //TODO: Explosions! Debris!
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy Projectile") {
            health -= 1;
        }
        if (other.gameObject.tag == "Enemy") {
            health -= 1;
        }
    }
}