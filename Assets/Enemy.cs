using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public GameObject deathEffect;
    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
            
        }

       
    }

