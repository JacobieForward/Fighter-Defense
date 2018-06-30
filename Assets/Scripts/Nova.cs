using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nova : MonoBehaviour {

    ParticleSystem emitter;

    CircleCollider2D collider;

    private float growthSpeed;
    private float secondsAlive;
    private float aliveCounter;

    // Use this for initialization
    void Awake () {
        secondsAlive = 5.0f; // maximum amount of seconds alive. There is no control over this however
        growthSpeed = 20.0f;
        aliveCounter = 0.0f;

        collider = GetComponent<CircleCollider2D>();
        emitter = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        ParticleSystem.ShapeModule shape = emitter.shape;
        shape.radius += Time.deltaTime * growthSpeed;
        collider.radius += Time.deltaTime * growthSpeed;
        aliveCounter += Time.deltaTime;
        if (aliveCounter >= secondsAlive)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().Death();
        }
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, collider);
        }
    }

    public void SetTime(float time)
    {
        secondsAlive = time;
    }
    
    public void SetGrowth(float growth)
    {
        growthSpeed = growth;
    }
}
