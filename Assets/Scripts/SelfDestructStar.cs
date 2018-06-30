using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructStar : MonoBehaviour {
    private Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
    }


    private void Update() {
        if (!render.isVisible) {
            Manager.instance.RemoveStar(gameObject);
        }
    }
}
