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
}