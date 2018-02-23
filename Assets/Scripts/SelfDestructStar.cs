using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructStar : MonoBehaviour {

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
