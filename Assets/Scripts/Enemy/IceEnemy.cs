using UnityEngine;

public class IceEnemy : MonoBehaviour,IEnemy
{

    public GameObject deathEffect;
    public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);

    }


}