using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class BoxPlatform : MonoBehaviour
{
    [Serializable]
    public class PBoxProperties
    {
        public GameObject gobj;
        public float mass;
    }

    [Serializable]
    public class PBoxStates
    {
        public float maxMass;
        public Transform locationOBJ;
        public Vector3 location;
    }

    public float totalMassOnPlatform = 0f;
    public float TotalMass = 0f;
    public List<PBoxProperties> Boxes;
    public List<PBoxStates> States;

    Coroutine coroutine;

    [Space]

    public float currentMass = 0f;
    
    public float requiredMass = 5f;
    public float dropDistance = 2f;
    public float dropSpeed = 1f;
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
    }

    void FixedUpdate()
    {
        currentMass = GetMassOnTop();

        if (currentMass >= requiredMass)
        {
            // Move down, but limit to max drop
            Vector2 targetPosition = new Vector2(startPosition.x, startPosition.y - dropDistance);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, dropSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Move back up to original position
            transform.position = Vector2.MoveTowards(transform.position, startPosition, dropSpeed * Time.fixedDeltaTime);
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
