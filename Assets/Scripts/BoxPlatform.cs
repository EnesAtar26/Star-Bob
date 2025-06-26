using System;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlatform : MonoBehaviour
{
    [Serializable]
    public class PBoxStates
    {
        public float maxMass;
        public float dropSpeed;
        public Transform locationOBJ;
        public Vector3 location;
    }
    public List<PBoxStates> States;

    [Space]

    public float currentMass = 0f;

    public LayerMask objectLayer;
    public float CastUp;
    public Vector2 CastSize;
    public Vector2 CastOffset;

    private Vector2 startPosition;
    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = (Vector2)transform.position;

        foreach(PBoxStates ps in States)
        {
            ps.location = startPosition + (Vector2)ps.locationOBJ.localPosition;
        }
    }

    void FixedUpdate()
    {
        currentMass = GetMassOnTop();

        foreach(PBoxStates ps in States)
        {
            if (currentMass <= ps.maxMass)
            {
                transform.position = Vector2.MoveTowards(transform.position, ps.location, ps.dropSpeed * Time.fixedDeltaTime);
                break;
            }
        }
    }

    float GetMassOnTop()
    {
        float totalMass = 0f;

        Vector2 center = (Vector2)transform.position + CastOffset + Vector2.up * CastUp;
        Vector2 size = CastSize;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, objectLayer);

        foreach (Collider2D col in hits)
        {
            Rigidbody2D r = col.attachedRigidbody;
            if (r != null && r != rb)
            {
                totalMass += r.mass;
            }

            var mb = col.gameObject.GetComponent<MassBox>().TopBox;
            while (mb != null)
            {
                r = mb.GetComponent<Rigidbody2D>();
                if (r != null && r != rb)
                {
                    totalMass += r.mass;
                }
                mb = mb.GetComponent<MassBox>().TopBox;
            }
        }

        return totalMass;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the detection box
        Vector2 center = (Vector2)transform.position + CastOffset + Vector2.up * CastUp;
        Vector2 size = CastSize;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(center, size);
    }

}
