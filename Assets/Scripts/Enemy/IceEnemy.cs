using UnityEngine;

public class IceEnemy : MonoBehaviour,IEnemy
{
    
    public GameObject deathEffect;

    PlayerMeshController pmc;
    MainManager main;

    private void Start()
    {
        pmc = FindAnyObjectByType<PlayerMeshController>();
        main = FindAnyObjectByType<MainManager>();
    }

    public void Die()
    {
        GlobalClass.Score += 350;
        pmc.audioSource.PlayOneShot(main.EnemyDie);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

       
}

