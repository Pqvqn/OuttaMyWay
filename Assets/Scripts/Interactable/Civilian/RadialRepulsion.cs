using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialRepulsion : Repulsion
{
    public float magnitude = 3;
    public float closeCutoff = 0.1f;
    public float sqrRadius = 0f;
    public override Vector2 CalcRepulsion(Vector2 otherPos)
    {
        Vector2 dir = otherPos - (Vector2) transform.position;
        float sqrMag = dir.SqrMagnitude() - sqrRadius;
        if (sqrMag > closeCutoff)
        {
            sqrMag *= sqrMag;
            return magnitude * dir / sqrMag;
        }
        return Vector2.zero;
    }
}
