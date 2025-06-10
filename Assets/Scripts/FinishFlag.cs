using System.Threading;
using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    public GameObject Flag;

    Vector3 start, end;

    bool finish = false;
    float timer = 3f;


    private void Start()
    {
        start = Flag.transform.position;
        end = Flag.transform.position + Vector3.down * 2f;
    }

    private void Update()
    {
        if (!finish)
            return;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Done();
            return;
        }
        Flag.transform.position = Vector3.Lerp(start, end, (3f - timer) / 1f);
    }

    void Done()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") finish = true;
    }
}
