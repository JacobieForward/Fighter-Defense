using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Manager : MonoBehaviour {
    // A singleton variable created in the script Loader
    // Contains variables and scripts that are used regardless of level and must persist between them

    public static Manager instance = null;
    private float playtime;
    public GameObject player;
    private GameObject navigationArrow;
    public Camera minimapCam;
    public GameObject starPrefab;

    private Vector3 storedPlayerPosition;
    private Quaternion storedPlayerRotation;

    private Vector3 screenPos;
    private Vector2 onScreenPos;
    private Vector3 cameraOffset;

    public GameObject currentNavPoint;
    public GameObject currentRespawnPoint;
    public GameObject playerPrefab;

    private Slider healthSlider;
    private Slider energySlider;
    private Slider stationHealthSlider;
    private Player playerScript;
    private Station station;

    private float respawnTime;
    private float respawnTimer;

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
        playtime += Time.deltaTime;
        
        // This causes the camera to follow the player and handles respawn behavior
        if (player != null)
        {
            Camera.main.transform.position = player.transform.position + cameraOffset;
            minimapCam.transform.position = player.transform.position + cameraOffset;

            // This section controls the stars spawning in the player's path
            if ((storedPlayerPosition != player.transform.position || storedPlayerRotation != player.transform.rotation))
            {
                // Start spawning background stars
                // TODO: EVEN STAR DISTRIBUTION WHEN GOING SLOW OR FAST (certain amount of stars onscreen?)
                if (player.transform.position.y > storedPlayerPosition.y) {
                    Vector3 topPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1f), 1, 1));
                    Instantiate(starPrefab, topPos, Quaternion.identity);
                }
                if (player.transform.position.y < storedPlayerPosition.y) {
                    Vector3 bottomPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0, 1f), 0, 1));
                    Instantiate(starPrefab, bottomPos, Quaternion.identity);
                }
                if (player.transform.position.x > storedPlayerPosition.x) {
                    Vector3 rightPos = Camera.main.ViewportToWorldPoint(new Vector3(1, Random.Range(0, 1f), 1));
                    Instantiate(starPrefab, rightPos, Quaternion.identity);
                }
                if (player.transform.position.x < storedPlayerPosition.x) {
                    Vector3 leftPos = Camera.main.ViewportToWorldPoint(new Vector3(0, Random.Range(0, 1f), 1));
                    Instantiate(starPrefab, leftPos, Quaternion.identity);
                }
            }
            storedPlayerPosition = player.transform.position;
            storedPlayerRotation = player.transform.rotation;

            // This section updates the navigation arrow's positioning
            Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(currentNavPoint.transform.position);
            Vector3 pointerScreenPosition = Camera.main.WorldToScreenPoint(player.transform.position);
            Vector3 distance = targetScreenPosition - pointerScreenPosition;
            float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            navigationArrow.transform.localEulerAngles = new Vector3(0f, 0f, angle - 90); // For some reason the angle was off by 90 degrees, so that is the magic number that makes this work properly

            // This section managers UI components
            healthSlider.value = playerScript.health;
            energySlider.value = playerScript.energy;
            stationHealthSlider.value = station.health;
        }
        else
        {
            PlayerRespawn();
        }

        if (station.health <= 0)
        {
            GameOver();
        }
    }

    void Init() {
        playtime = 0.0f;
        respawnTimer = 0.0f;
        respawnTime = 3.0f;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();
        navigationArrow = GameObject.Find("Navigation Arrow");
        minimapCam = GameObject.Find("MinimapCamera").GetComponent<Camera>();
        currentNavPoint = GameObject.Find("TheStation");
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        energySlider = GameObject.Find("EnergySlider").GetComponent<Slider>();
        stationHealthSlider = GameObject.Find("StationHealthSlider").GetComponent<Slider>();
        cameraOffset = new Vector3(0, 0, -2);
        currentRespawnPoint = GameObject.Find("StartRespawnPoint");
        station = GameObject.Find("TheStation").GetComponent<Station>();
    }

    void PlayerRespawn() {
        Camera.main.transform.position = currentRespawnPoint.transform.position + cameraOffset;
        minimapCam.transform.position = currentRespawnPoint.transform.position + cameraOffset;
        respawnTimer += Time.deltaTime;

        if (respawnTimer >= respawnTime)
        {
            Instantiate(playerPrefab, currentRespawnPoint.transform.position, Quaternion.identity);
            player = GameObject.FindGameObjectWithTag("Player");
            playerScript = player.GetComponent<Player>();
            Time.timeScale = 1.0f;
            playerScript.timeFrozen = false;
            respawnTimer = 0.0f;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0;
    }
}