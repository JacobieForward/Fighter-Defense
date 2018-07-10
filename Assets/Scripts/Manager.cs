﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Manager : MonoBehaviour {
    // A singleton variable created in the script Loader
    // Contains variables and scripts that are used regardless of level and must persist between them

    public static Manager instance = null;
    private float playtime;
    public int mineKillCount;
    public int turretKillCount;
    public int fighterKillCount;
    public GameObject player;
    public GameObject starPrefab;
    public GameObject explosionPrefab;
    public GameObject deathExplosionPrefab;
    
    private Vector3 storedPlayerPosition;
    private Quaternion storedPlayerRotation;

    private Vector3 screenPos;
    private Vector2 onScreenPos;
    private Vector3 cameraOffset;
    
    public GameObject currentRespawnPoint;
    public GameObject playerPrefab;

    private Slider healthSlider;
    private Slider energySlider;
    private Slider stationHealthSlider;
    private GameObject gameOverCanvas;
    private Text gameOverText;
    private Text mineKillCountText;
    private Text turretKillCountText;
    private Text fighterKillCountText;
    private Player playerScript;
    private Station station;
    private GameObject tutorialCanvas;
    private GameObject respawningText;
    private Text livesText;
    public bool tutorial;
    private bool freshSpawn;

    private List<GameObject> starList = new List<GameObject>();
    private int starLimit;

    private float respawnTime;
    private float respawnTimer;

    private float lifeTime;
    private float lifeTimer;

    private int lives;

    void Awake () {
		if (instance == null) {
            instance = this;
        }
        else if (instance != this){
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}

    void Start(){
        Init();
    }

    void Update() {
        // This section manages UI components
        healthSlider.value = playerScript.GetHealth();
        energySlider.value = playerScript.GetEnergy();
        stationHealthSlider.value = station.health;
        livesText.text = lives.ToString();

        if (tutorial)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                tutorial = false;
                tutorialCanvas.SetActive(false);
            }
            return;
        }
        playtime += Time.deltaTime;
        
        // This handles respawn behavior
        if (player != null)
        {
            // Camera follows the player
            Camera.main.transform.position = player.transform.position + cameraOffset;
            // Spawn stars in player's path
            SpawnStars();
        }
        else
        {
            // If player is == null then the player has been destroyed and the respawning process activates
            respawningText.SetActive(true);
            PlayerRespawn();
        }

        // If the player has just spawned at the station create stars surrounding them
        if (freshSpawn)
        {
            InitialStars();
            freshSpawn = false;
        }

        // Runs life timer, adds life if the timer runs up
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            lives += 1;
            lifeTimer += 50.0f;
        }

        // Checks for loss condition
        if (station.health <= 0)
        {
            GameOver();
        }
    }

    void Init() {
        playtime = 0.0f;
        respawnTimer = 0.0f;
        respawnTime = 3.0f;
        starLimit = 80;
        tutorial = true;
        gameOverCanvas = GameObject.Find("GameOverCanvas");
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        mineKillCountText = GameObject.Find("MineKillCountText").GetComponent<Text>();
        turretKillCountText = GameObject.Find("TurretKillCountText").GetComponent<Text>();
        fighterKillCountText = GameObject.Find("FighterKillCountText").GetComponent<Text>();
        if (gameOverCanvas.activeSelf)
        {
            gameOverCanvas.SetActive(false);
        }
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        energySlider = GameObject.Find("EnergySlider").GetComponent<Slider>();
        stationHealthSlider = GameObject.Find("StationHealthSlider").GetComponent<Slider>();
        cameraOffset = new Vector3(0, 0, -2);
        currentRespawnPoint = GameObject.Find("StartRespawnPoint");
        station = GameObject.Find("TheStation").GetComponent<Station>();
        tutorialCanvas = GameObject.Find("TutorialCanvas");
        respawningText = GameObject.Find("RespawningText");
        livesText = GameObject.Find("LivesText").GetComponent<Text>();
        respawningText.SetActive(false);
        mineKillCount = 0;
        turretKillCount = 0;
        fighterKillCount = 0;
        lives = 2;
        lifeTime = 100.0f;
        lifeTimer = 0.0f;
        freshSpawn = true;
    }

    void PlayerRespawn() {
        respawnTimer += Time.deltaTime;
        Time.timeScale = 1.0f;

        if (respawnTimer >= respawnTime)
        {
            Instantiate(playerPrefab, currentRespawnPoint.transform.position, Quaternion.identity);
            player = GameObject.Find("Player(Clone)");
            playerScript = player.GetComponent<Player>();
            respawnTimer = 0.0f;
            freshSpawn = true;
            Instantiate(explosionPrefab, station.transform.position, Quaternion.identity);
            respawningText.SetActive(false);
        }
    }

    void GameOver()
    {
        gameOverText.text = "GAME OVER. You survived for " + playtime + "seconds.";
        mineKillCountText.text = mineKillCount.ToString();
        turretKillCountText.text = turretKillCount.ToString();
        fighterKillCountText.text = fighterKillCount.ToString();
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    void SpawnStars()
    {

        // This section controls the stars spawning in the player's path
        if ((storedPlayerPosition != player.transform.position || storedPlayerRotation != player.transform.rotation) && starList.Count <= starLimit)
        {
            Vector3 position = gameObject.transform.position;
            // Start spawning background stars
            // TODO: EVEN STAR DISTRIBUTION WHEN GOING SLOW OR FAST (certain amount of stars onscreen?)
            // if statement order determines priority of star spawning
            if (player.transform.position.y > storedPlayerPosition.y)
            {
                position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1f), 1, 1));
                CreateStar(position);
            }
            if (player.transform.position.x > storedPlayerPosition.x)
            {
                position = Camera.main.ViewportToWorldPoint(new Vector3(1, Random.Range(0, 1f), 1));
                CreateStar(position);
            }
            if (player.transform.position.y < storedPlayerPosition.y)
            {
                position = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1f), 0, 1));
                CreateStar(position);
            }
            if (player.transform.position.x < storedPlayerPosition.x)
            {
                position = Camera.main.ViewportToWorldPoint(new Vector3(0, Random.Range(0, 1f), 1));
                CreateStar(position);
            }
        }
        storedPlayerPosition = player.transform.position;
        storedPlayerRotation = player.transform.rotation;
    }

    void CreateStar(Vector3 position)
    {
        GameObject newstar = Instantiate(starPrefab, position, Quaternion.identity);
        starList.Add(newstar);
    }

    public bool RemoveStar(GameObject star)
    {
        if (starList.Contains(star))
        {
            starList.Remove(star);
            Destroy(star);
            return true;
        }
        return false;
    }

    void InitialStars()
    {
        for (int i = 0; i < 40; i++)
        {
            Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 1));
            CreateStar(screenPosition);
        }
    }

    public void SubtractLife()
    {
        if (lives <= 0)
        {
            respawningText.SetActive(false);
            GameOver();
        }
        else
        {
            lives -= 1;
        }
    }
}