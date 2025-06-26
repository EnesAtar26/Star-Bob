using System.Collections.Generic;
using UnityEngine;

public class BobSizeMultiplier : Pickable
{
    public List<Rigidbody2D> MassChanges;

    public float SizeMultiplier;
    public float MassChange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Rigidbody2D r in MassChanges)
            r.mass = MassChange;

        var pc = FindAnyObjectByType<PlayerController>();
        pc.transform.localScale *= SizeMultiplier;
        pc.groundCheckRadius *= SizeMultiplier;
        var pcMesh = FindAnyObjectByType<PlayerMeshController>();
        pcMesh.transform.localScale *= SizeMultiplier;
    }
}
