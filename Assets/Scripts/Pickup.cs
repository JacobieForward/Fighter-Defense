using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    public bool health;
    public bool energy;

    private float dissapearTimer;
    private float timeToDissapear;

    private void Start()
    {
        dissapearTimer = 0.0f;
        timeToDissapear = 15.0f;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        dissapearTimer += Time.deltaTime;
        if (dissapearTimer >= timeToDissapear)
        {
            Destroy(gameObject);
        }
    }
}