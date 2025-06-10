using UnityEngine;

public enum PickableType
{
    ConeIceCream = 1,
    Donut = 2,
    BowlIceCream = 3,
    WaterDrop=4,

    Other = 0
}

public class Pickable : MonoBehaviour
{
    public string Name;
    public string Id;
    public GameObject DestroyRoot;
    public PickableType Type;

    public Pickable Data()
    {
        Pickable p = new Pickable();
        p.Name = Name;
        p.Id = Id;
        p.Type = Type;
        return p;
    }
}
