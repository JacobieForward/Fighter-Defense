using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int health;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "Enemy Projectile")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {
            health -= 1;
        }
    }

    private void Update()
    {
        
    }
}
