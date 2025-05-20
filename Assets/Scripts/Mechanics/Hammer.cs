using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float minAngle, maxAngle, speed;
    [Space]
    public bool reverseSpin = false;

    bool increaseAngle;

    private void Start()
    {
        increaseAngle = !reverseSpin;
    }

    void Update()
    {
        var a = transform.eulerAngles;
        int dir = increaseAngle ? 1 : -1;
        a.z += speed * dir * Time.deltaTime;

        transform.eulerAngles = a;

        if (increaseAngle && transform.eulerAngles.z >= maxAngle) increaseAngle = false;
        if (!increaseAngle && transform.eulerAngles.z <= minAngle) increaseAngle = true;
    }
}
