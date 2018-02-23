using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    public GameObject GameManager;
	// Use this for initialization
	void Start () {
		if (Manager.instance == null) {
            Instantiate(GameManager);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
