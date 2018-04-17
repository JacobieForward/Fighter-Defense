using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public int health;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy Projectile")
        {
            health -= 1;
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponent<Enemy>().followPlayer)
        {
            health -= 1;
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            
            //GAME OVER
        }
    }
}
