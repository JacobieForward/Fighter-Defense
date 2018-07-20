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
        if (!render.isVisible && Manager.instance != null) {
           Manager.instance.RemoveStar(gameObject);
        } else if (!render.isVisible)
        {
            Destroy(gameObject);
        }
    }
}
