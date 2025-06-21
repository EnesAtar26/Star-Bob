using System.Collections.Generic;
using UnityEngine;

public class PSmoothie : Pickable
{
    public List<Rigidbody2D> MassChanges;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Rigidbody2D r in MassChanges)
            r.mass = 0.05f;

        var pc = FindAnyObjectByType<PlayerController>();
        pc.transform.localScale *= 2;
        pc.groundCheckRadius *= 2;
        var pcMesh = FindAnyObjectByType<PlayerMeshController>();
        pcMesh.transform.localScale *= 2;
    }
}
