using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    private float whenInstantiated;

    private void Awake() {
        whenInstantiated = Time.timeSinceLevelLoad;
        Physics2D.IgnoreCollision(Manager.instance.player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    void Update() {
        transform.position += transform.up * Time.deltaTime * speed;
        float timeSinceInstantiated = Time.timeSinceLevelLoad - whenInstantiated;
        if (timeSinceInstantiated > 2.0f) {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Debris"){
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Player") {
            // Do nothing
        }
    }
}