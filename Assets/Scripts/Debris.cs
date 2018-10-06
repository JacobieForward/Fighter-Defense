using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

    Rigidbody2D rb;
    Vector2 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = Random.onUnitSphere * Random.Range(5, 20);
    }

    void OnCollisionEnter2D(Collision2D other) {
        if ((other.gameObject.tag == "Projectile" || other.gameObject.tag == "Enemy Projectile" || other.gameObject.tag == "TurretProjectile") && !gameObject.name.Contains("Station")) {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}