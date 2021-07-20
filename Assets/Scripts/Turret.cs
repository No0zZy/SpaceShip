using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform m_LaunchPoint;
    public Vector3 LaunchPosition => m_LaunchPoint.position;

    [SerializeField] private float m_RateOfFire;
    [SerializeField] private float m_TurretAngle;
    [SerializeField] private Projectile m_ProjectilePrefab;
    public Projectile ProjectilePrefab => m_ProjectilePrefab;
    private float m_RefireTime;


    [SerializeField] private AudioController m_Audio;

    public Vector3 WorldAimPoint { get; set; }

    public void Fire(SpaceShip parent)
    {
        if (m_RefireTime > 0)
            return;


        m_RefireTime = m_RateOfFire;
        if (Vector3.Angle((WorldAimPoint - transform.position), m_LaunchPoint.forward) <= m_TurretAngle)
        {
            m_Audio.PlayAudioClip(m_ProjectilePrefab.ShootSound);
            LaunchProjectile(parent);
        }
    }

    private void LaunchProjectile(Destructible parent)
    {
        var projectile = PoolManager.Instance.Spawn(m_ProjectilePrefab.gameObject);
        projectile.GetComponent<Projectile>().SetParent(parent);
        projectile.transform.position = m_LaunchPoint.position;
        projectile.transform.forward = (WorldAimPoint - m_LaunchPoint.position).normalized;
    }

    private void Update()
    {
        if (m_RefireTime > 0)
            m_RefireTime -= Time.deltaTime;
    }
}
