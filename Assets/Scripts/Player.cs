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
    public float turnSpeed;
    public float thrustSpeed;
    public GameObject projectile;

    private Rigidbody2D rigidbody2d;

    private void Start()
    {
        maxHealth = 10;
        maxEnergy = 50;
        health = maxHealth;
        energy = maxEnergy;

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        /* OLD MOVEMENT SYSTEM
         *float inputHorizontal = Input.GetAxis("Horizontal");
         float inputVertical = Input.GetAxis("Vertical");

         // The player moves backwards at reduced speed
         if (inputVertical < 0) {
             inputVertical = inputVertical/2;
         }

         transform.position += transform.up * inputVertical * Time.deltaTime * thrustSpeed;
         transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * turnSpeed);*/
         // regular movement slower + afterburners
         // Add particle effects for ship

        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        if (inputVertical > 0 ) {
            inputVertical = 0.01f;
        } else {
            inputVertical = 0;
        }

        transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * turnSpeed);
        rigidbody2d.AddForce(transform.up * thrustSpeed * inputVertical);

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