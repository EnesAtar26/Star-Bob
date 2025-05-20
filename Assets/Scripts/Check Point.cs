using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isChecked = false;
    public int order;
    public Transform spawn;

    public void Save()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        isChecked = true;
        if (GlobalClass.CurrentCheckPoint < order)
        {
            GlobalClass.CurrentCheckPoint = order;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isChecked)
            Save();
    }
}
