using UnityEngine;

public class IceEnemy : MonoBehaviour
{

    public GameObject deathEffect;
    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }


}