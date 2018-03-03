using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour {
    // Navpoints are each individual objects that operate like a linked list
    // Each navpoint points to the next in the chain

    public GameObject nextNavPoint;

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Navpoint Switched.");
            // Set next navpoint
            Manager.instance.currentNavPoint = nextNavPoint;
            // Display messages

            // Destroy this navpoint
            Destroy(this);
        }
    }
}