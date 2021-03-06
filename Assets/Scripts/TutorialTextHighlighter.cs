﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialTextHighlighter : MonoBehaviour {

    Text exit;
    Text controls;
    Text turret;

	void Awake () {
        exit = GameObject.Find("ExitText").GetComponent<Text>();
        controls = GameObject.Find("ControlsText").GetComponent<Text>();
        turret = GameObject.Find("TurretText").GetComponent<Text>();
	}

    private void Start()
    {
        turret.text = "The counter here keeps track of the points accumulated by destroying enemies. Pressing <color=#03FF00>F</color> spawns a turret at the cost of 100 points. Pressing <color=#03FF00>T</color> spawns a mine. Respawning after death costs 100 points. Respawning costs 25 more points each time.";
        exit.text = "Press <color=#03FF00>P</color> to Start. Press <color=#03FF00>P</color> at any time to pause.";
        controls.text = "Use <color=#03FF00>WASD</color> or the <color=#03FF00>Arrow Keys</color> to move and press <color=#03FF00>space</color> to fire. Holding down <color=#03FF00>Shift</color> activates the afterburners. Keep the station in the center of the map from being destroyed as long as possible. <color=#03FF00>Enter</color> Launches a missile that uses a lot of energy.";
        if (SceneManager.GetActiveScene().name == "MapThree")
        {
            controls.text = "Use <color=#03FF00>WASD</color> or the <color=#03FF00>Arrow Keys</color> to move and press <color=#03FF00>space</color> to fire. <color=#03FF00>Enter</color> Launches a missile that uses a lot of energy. Stay alive as long as possible!";
        }
    }
}
