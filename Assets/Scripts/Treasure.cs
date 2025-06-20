using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Treasure : MonoBehaviour
{
    public PickableType Type;
    public float ExpireTime = 0f;
    public SpriteRenderer Renderer;

    MainManager manager;
    bool empty = false;
    Coroutine expire, tick;

    private void Start()
    {
        manager = FindAnyObjectByType<MainManager>();
    }

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
        var uPos = Renderer.transform.position + Vector3.up * 0.6f;

        // Goes Up
        while (timer < 0.1f)
        {
            Renderer.transform.position = Vector3.LerpUnclamped(nPos, uPos, timer / 0.1f);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Renderer.transform.position = uPos;

        CheckExpire();

        switch (Type)
        {
            case PickableType.ConeIceCream:
                SummonTreasure();
                break;
        }

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

        switch (Type)
        {
            case PickableType.Donut:
            case PickableType.Cupcake:
            case PickableType.BowlIceCream:
                SummonTreasure();
                break;
        }

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
                var cic = Instantiate(manager.ConeIceCream, transform);
                cic.transform.localPosition = Renderer.transform.localPosition; cic.transform.localRotation = Quaternion.identity;
                cic.AddComponent<TreasureElement>().Initilize(PickableType.ConeIceCream);
                break;

            case PickableType.Donut:
                var d = Instantiate(manager.Donut, transform);
                d.transform.localPosition = Renderer.transform.localPosition; d.transform.localRotation = Quaternion.identity;
                d.AddComponent<TreasureElement>().Initilize(PickableType.Donut);
                break;

            case PickableType.Cupcake:
                var c = Instantiate(manager.Cupcake, transform);
                c.transform.localPosition = Renderer.transform.localPosition; c.transform.localRotation = Quaternion.identity;
                c.AddComponent<TreasureElement>().Initilize(PickableType.Cupcake);
                break;

            case PickableType.BowlIceCream:
                var boi = Instantiate(manager.BowlIceCream, transform);
                boi.transform.localPosition = Renderer.transform.localPosition; boi.transform.localRotation = Quaternion.identity;
                boi.AddComponent<TreasureElement>().Initilize(PickableType.BowlIceCream);
                break;
        }
    }

}
