using UnityEngine;

public class Projectile : MonoBehaviour {
    public float speed;
    private float whenInstantiated;

    private void Awake() {
        whenInstantiated = Time.timeSinceLevelLoad;
    }

    void Update() {
        transform.position += transform.up * Time.deltaTime * speed;
        float timeSinceInstantiated = Time.timeSinceLevelLoad - whenInstantiated;
        if (timeSinceInstantiated > 1.5f) {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (gameObject.name.Contains("Missile"))
        {
            GameObject explosionInstance = Instantiate(Manager.instance.explosionPrefab, transform.position, transform.rotation);
            Nova explosionScript = explosionInstance.GetComponent<Nova>();
            explosionScript.SetTime(1.0f);
            explosionScript.SetGrowth(15.0f);
        }
    }
}