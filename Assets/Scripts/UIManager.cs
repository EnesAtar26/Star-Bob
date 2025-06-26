using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public UnityEngine.UI.Image Heart1, Heart2, Heart3;
    public TMP_Text Skor, Zaman;

    public Sprite FullHeart, EmptyHeart;

    PlayerController p;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        p = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (p == null)
        {
            Heart1.sprite = EmptyHeart;
            Heart2.sprite = EmptyHeart;
            Heart3.sprite = EmptyHeart;
        }
        else
            switch (p.Improvement)
            {
                case BobImprovements.None:
                    Heart1.sprite = FullHeart;
                    Heart2.sprite = EmptyHeart;
                    Heart3.sprite = EmptyHeart;
                    break;

                case BobImprovements.Armor:
                    Heart1.sprite = FullHeart;
                    Heart2.sprite = FullHeart;
                    Heart3.sprite = EmptyHeart;
                    break;

                default:
                    Heart1.sprite = FullHeart;
                    Heart2.sprite = FullHeart;
                    Heart3.sprite = FullHeart;
                    break;
            }

        var min = (int)(GlobalClass.CurrentTimeSeconds / 60f);
        var hour = min / 60;
        Zaman.text = hour + ":" + min + ":" + ((int)GlobalClass.CurrentTimeSeconds);
        Skor.text = GlobalClass.Score.ToString();
    }
}
