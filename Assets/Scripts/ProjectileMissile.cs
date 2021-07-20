using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMissile : Projectile
{
    [SerializeField] private float m_ExposiveRadius;

    protected override void ApplyHit(RaycastHit hit)
    {
        var cols = Physics.OverlapSphere(hit.point, m_ExposiveRadius);
        foreach (var c in cols)
        {
            var destructible = c.transform.root.GetComponent<Destructible>();
            if (destructible)
            {
                destructible.ApplyDamage(m_Damage);
            }
        }
    }
}
