using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [Header("Pickup Sprites")]
    public GameObject ConeIceCream;
    public GameObject Donut;
    public GameObject BowlIceCream;
    public GameObject Cupcake;
    public GameObject WaterDrop;

    [Space]
    [Header("Pickup Sprites")]
    public GameObject Projectile_IceCream;
    public GameObject Projectile_Pizza;
    public GameObject Projectile_Destroy;

    [Space]
    [Header("Effects")]
    public GameObject Invicible;
    public GameObject Poof;
    public GameObject BrokenHeart;

    [Space]

    public LayerMask TreausureFlipLayers;
}