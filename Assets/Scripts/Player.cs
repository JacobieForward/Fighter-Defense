using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    // This script handles every piece of player input
    private int health;
    private int energy;
    public int maxHealth;
    public int maxEnergy;
    private float turnSpeed;
    private float thrustSpeed;
    public GameObject projectile;
    public GameObject turret;
    public GameObject mine;
    public GameObject missile;

    private float shootTimer;
    public float roundsPerSecond;
    private float energyTimer;
    public float energyPerSecond;
    private int turretCost;
    private int mineCost;

    private ParticleSystem afterburnerParticles;
    private AudioSource[] audioSources;
    private AudioSource afterburnerSound;
    private AudioSource needmorepointsSound;

    public List<GameObject> debrisList;

    private void Awake()
    {
        afterburnerParticles = gameObject.GetComponent<ParticleSystem>();
        afterburnerParticles.Pause();
    }

    private void Start()
    {
        // TODO: Remove thrustspeed and turnspeed declarations out of update when the movement system is designed
        health = maxHealth;
        energy = maxEnergy;

        shootTimer = 0.0f;
        energyTimer = 0.0f;
        turretCost = 100;
        mineCost = 25;
        
        audioSources = gameObject.GetComponents<AudioSource>();
        afterburnerSound = audioSources[0];
        needmorepointsSound = audioSources[1];
        afterburnerSound.Stop();
        needmorepointsSound.Stop();
    }

    private void FixedUpdate()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        thrustSpeed = 20;
        turnSpeed = 180;

        // The player moves backwards at reduced speed
        if (inputVertical < 0)
        {
            inputVertical /= 2;
        }

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.position += transform.up * inputVertical * Time.deltaTime * (thrustSpeed * 2);
            transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * (turnSpeed / 2));
            afterburnerParticles.Play();
            if (!afterburnerSound.isPlaying)
            {
                afterburnerSound.Play();
            }

        }
        else
        {
            transform.position += transform.up * inputVertical * Time.deltaTime * thrustSpeed;
            transform.Rotate(new Vector3(0.0f, 0.0f, -inputHorizontal) * Time.deltaTime * turnSpeed);
            afterburnerParticles.Stop();
            afterburnerSound.Stop();
        }
    }

    void Update() {
        if (health <= 0)
        {
            Death();
        }


        
        if (energy < maxEnergy) {
            energyTimer += Time.deltaTime;
            if (energyTimer >= energyPerSecond) {
                AddEnergy(1);
                energyTimer = 0.0f;
            }
        }
        shootTimer += Time.deltaTime;
        if (Input.GetKey("space") && energy > 0 && shootTimer >= roundsPerSecond) {
            //Fire the player's primary weapon
            GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(projectileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            energy -= 1;
            shootTimer = 0.0f;
        }

        if (Input.GetKeyUp("t")) {
            //Spawn a mine
            if (Manager.instance.CheckPoints(mineCost))
            {
                Instantiate(mine, transform.position, transform.rotation);
            }
            else
            {
                needmorepointsSound.Play();
            }
        }

        if (Input.GetKey("return") && energy > 11 && shootTimer >= roundsPerSecond)
        {
            //Fire the player's secondary weapon
            GameObject missileInstance = Instantiate(missile, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(missileInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            energy -= 10;
            shootTimer = 0.0f;
        }

        if (Input.GetKeyUp("f"))
        {
            //Spawn a turret under the player
            if (Manager.instance.CheckPoints(turretCost))
            {
                GameObject turretInstance = Instantiate(turret, transform.position, transform.rotation);
            } else
            {
                needmorepointsSound.Play();
            }

        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy Projectile") {
            health -= 1;
            Destroy(other.gameObject);
            Manager.instance.ScreenFlashRed();
        }
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy>().followPlayer) {
            health -= 1;
            Manager.instance.ScreenFlashRed();
        }
        if (other.gameObject.tag == "Pickup") {
            if (other.gameObject.GetComponent<Pickup>().energy) {
                AddEnergy(20); // Should have started using constants
                
            }
            else if (other.gameObject.GetComponent<Pickup>().health) {
                AddHealth(4);
            }
        }
    }

    void AddEnergy(int amount) {
        if ((amount + energy) > maxEnergy) {
            energy = maxEnergy;
        } else
        {
            energy += amount;
        }
    }

    void AddHealth(int amount) {
        if ((amount + health) > maxHealth)
        {
            health = maxHealth;
        } else
        {
            health += amount;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int value)
    {
        health = value;
    }

    public int GetEnergy()
    {
        return energy;
    }

    // Straight copy pasted from the Player script. An issue of not using inheritance at all in this project
    void SpawnDebris()
    {
        if (debrisList.Count != 0)
        {
            List<GameObject> tempDebrisList = debrisList;
            GameObject debris = tempDebrisList[0];
            for (int i = 0; i <= debrisList.Count; i++)
            {
                if (tempDebrisList.Count == 0)
                {
                    debris = tempDebrisList[0];
                }
                else
                {
                    debris = tempDebrisList[Random.Range(0, tempDebrisList.Count - 1)];
                }
                if (debris != null)
                {
                    Instantiate(debris, transform.position, transform.rotation);
                }
                tempDebrisList.Remove(debris);
            }
        }
        else
        {
            return;
        }
    }

    public void Death()
    {
        SpawnDebris();
        Manager.instance.PlayerDied();
        Destroy(gameObject);
    }
}