using System.Collections;
using CartoonFX;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public ParticleSystem IgniteFire, IgniteExplosion;
    public Vector2 SetVelocity;
    public bool X, Y;

    PlayerController bob;
    bool inside;
    Coroutine c;



    IEnumerator Ignite()
    {
        IgniteFire.Play();
        IgniteExplosion.GetComponent<CFXR_Effect>().cameraShake.enabled = true;
        yield return new WaitForSeconds(2);
        IgniteFire.Stop();
        IgniteExplosion.Play();
        
        if (inside) 
        {
            if (X) bob.GetComponent<Rigidbody2D>().linearVelocityX = SetVelocity.x;
            if (Y) bob.GetComponent<Rigidbody2D>().linearVelocityY = SetVelocity.y;
        }
        
        yield return new WaitForSeconds(5);
        c = null;
    }

    private void Start()
    {
        bob = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;

        if (c == null)
            c = StartCoroutine(Ignite());
            
        inside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            inside = false;
    }
}
