using System.Linq;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public string Key;
    public GameObject DestroyRoot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.GetComponent<PlayerController>().Inventory.Any(x => x.Name == Key))
        {
            GlobalClass.Unlocks.Add(Key);
            Destroy(DestroyRoot);
        }
    }
}
