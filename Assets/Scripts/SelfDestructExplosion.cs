using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructExplosion : MonoBehaviour {

    private float destructTimer;
    private float destructTime;

	// Use this for initialization
	void Start () {
        destructTimer = 0.0f;
        destructTime = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
        destructTimer += Time.deltaTime;
        if (destructTimer >= destructTime)
        {
            Destroy(gameObject);
        }
	}
}
