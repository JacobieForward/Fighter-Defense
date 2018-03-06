using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    // This encompases all enemies with variables checked the specify what kind of enemy
    public int health;
    public float speed;
    public float followDistance;
    public bool followPlayer;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Projectile") {
            health -= 1;
        }
        if (other.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }

    void Update() {
        if (health <= 0) {
            // TODO: Explosions! Maybe debris? Maybe fire? Though fire doesn't exist in vacuum. Maybe plasma? Gasses decompressing?
            Destroy(gameObject);
        }
        if(followPlayer && Manager.instance.player != null) {
            if ((Vector3.Distance(Manager.instance.player.transform.position, transform.position) < followDistance)) {
                // Move towards the player
                transform.position = Vector3.MoveTowards(transform.position, Manager.instance.player.transform.position, speed * Time.deltaTime);
            }
        }
    }
}