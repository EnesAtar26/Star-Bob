using System.Collections;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public PickableType Type;
    public float ExpireTime = 0f;
    public SpriteRenderer Renderer;

    public GameObject IceCream;

    bool empty = false;
    Coroutine expire, tick;

    public bool TickTreasure()
    {
        if (!empty && tick == null) 
        {
            tick = StartCoroutine(Tick());
            return true;
        }
        return false;
    }

    IEnumerator StartExpire()
    {
        while (ExpireTime > 0f) 
        {
            ExpireTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        expire = null;
    }

    IEnumerator Tick()
    {
        float timer = 0f;
        var nPos = Renderer.transform.position;
        var dPos = Renderer.transform.position + Vector3.down * 0.2f;
        var uPos = Renderer.transform.position + Vector3.up * 0.8f;

        // Goes Up
        while (timer < 0.1f)
        {
            Renderer.transform.position = Vector3.LerpUnclamped(nPos, uPos, timer / 0.1f);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Renderer.transform.position = uPos;

        CheckExpire();
        SummonTreasure();

        // Goes Down
        timer = 0f;
        while (timer < 0.15f)
        {
            Renderer.transform.position = Vector3.LerpUnclamped(uPos, dPos, timer / 0.15f);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Renderer.transform.position = dPos;


        // Goes Normal
        timer = 0f;
        while (timer < 0.04f)
        {
            Renderer.transform.position = Vector3.LerpUnclamped(dPos, nPos, timer / 0.04f);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Renderer.transform.position = nPos;

        tick = null;
    }
    
    void CheckExpire()
    {
        if (ExpireTime <= 0f)
        {
            empty = true;
            Renderer.color = Color.white;
        }
        else if (expire == null)
            expire = StartCoroutine(StartExpire());
    }

    void SummonTreasure()
    {
        switch (Type)
        {
            case PickableType.ConeIceCream:

                break;
        }
    }

}
