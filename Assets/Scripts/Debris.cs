using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

    public int health;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Projectile") {
            health -= 1;
        }
    }

    void Update() {
        if (health <= 0) {
            // TODO: Explosions! Maybe debris? Maybe fire? Though fire doesn't exist in vacuum. Maybe plasma?
            Destroy(gameObject);
        }
    }

}
