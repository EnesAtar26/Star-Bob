using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject bob;

    void Start()
    {
        bob = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (bob == null)
        {
            return;
        }

        // Kameranmýn derinliðinin etkilenmemesi için bu 3 satýr var. Sadece x ve y Bob'un pozisyonuna eþitlenmeli
        // Ayrýca kamera Bob'dan biraz yukarýda olmalý
        var p = bob.transform.position;
        p.z = transform.position.z;
        p.y += 2f;


        transform.position = p;
    }
}
