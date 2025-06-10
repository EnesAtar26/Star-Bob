using UnityEngine;
using UnityEngine.Events;

public class BtnPressScript : MonoBehaviour
{
    public SpriteRenderer buttonSprite;
    public Color pressedColor = Color.green;
    public Color defaultColor = Color.red;

    public UnityEvent OnButtonPressed;
    public UnityEvent OnButtonReleased;

    private int collisionCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(buttonSprite == null)
        {
            buttonSprite = GetComponent<SpriteRenderer>();
        }
        if (buttonSprite != null) 
        {
            buttonSprite.color = defaultColor;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Pushable"))
        {
            collisionCount++;
            if (collisionCount == 1)
            {
                PressButton();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Pushable"))
        {
            collisionCount--;
            if (collisionCount == 0) // Tüm nesneler çýktýðýnda
            {
                //ReleaseButton();
            }
        }
    }

    void PressButton()
    {
        if (buttonSprite != null)
        {
            buttonSprite.color = pressedColor; // Rengi yeþile çevir
        }
        OnButtonPressed.Invoke(); // Olayý tetikle
    }

    void ReleaseButton()
    {
        if (buttonSprite != null)
        {
            buttonSprite.color = defaultColor; // Rengi varsayýlana çevir
        }
        OnButtonReleased.Invoke(); // Olayý tetikle
    }
}
