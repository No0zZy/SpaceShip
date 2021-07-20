using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Velocity;
    public float Velocity => m_Velocity;
    [SerializeField] private float m_Lifetime;

    [SerializeField] protected float m_Damage;

    [SerializeField] private ImpactEffect m_ImpactEffectPrefab;
    private float m_LifeTimer;


    [SerializeField] private AudioClip m_ShootSound;
    public AudioClip ShootSound => m_ShootSound;

    [SerializeField] private AudioClip m_HitSound;
    public AudioClip HitSound => m_HitSound;

    [SerializeField] private AudioController m_Audio;

    [SerializeField] private Destructible m_Parent;

    private Destructible m_FollowTarget;

    private void OnEnable()
    {
        m_LifeTimer = 0;
        if(Player.Instance.PlayerShip.SelectedTarget)
            m_FollowTarget = Player.Instance.PlayerShip.SelectedTarget;
    }

    public void SetParent(Destructible parent)
    {
        m_Parent = parent;
    }

    [SerializeField] private float m_ProjectileRotateSpeed;

    private void Update()
    {
        if (m_LifeTimer > m_Lifetime)
            OnLifetimeEnd();

        if (m_FollowTarget && m_Parent == Player.Instance.PlayerShip && Player.Instance.IsAimHelp)
            transform.forward = Vector3.Slerp(
                transform.forward,
                m_FollowTarget.transform.position - transform.position,
                m_ProjectileRotateSpeed * Time.deltaTime);

        float len = m_Velocity * Time.deltaTime;
        Vector3 step = transform.forward * len;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, len))
        {
            m_LifeTimer = m_Lifetime;
            transform.position = hit.point;

            if (m_ImpactEffectPrefab)
            {
                var effect = PoolManager.Instance.Spawn(m_ImpactEffectPrefab.gameObject);
                effect.transform.position = hit.point;
            }
            var destr = hit.collider.transform.root.GetComponent<Destructible>();
            if (destr)
                if(destr.TeamId != m_Parent.TeamId ) 
                    ApplyHit(hit);
            OnLifetimeEnd();
            return;
        }

        transform.position += step;
        m_LifeTimer += Time.deltaTime;
    }

    protected virtual void ApplyHit(RaycastHit hit)
    {
        transform.position = hit.point;

        m_Audio.PlayAudioClip(m_HitSound);

        if (m_ImpactEffectPrefab)
        {
            var effect = PoolManager.Instance.Spawn(m_ImpactEffectPrefab.gameObject);
            effect.transform.position = hit.point;
        }

        var destructible = hit.collider.transform.root.GetComponent<Destructible>();
        if (destructible)
        {
            destructible.ApplyDamage(m_Damage);
        }
    }

    private void OnLifetimeEnd()
    {
        m_FollowTarget = null;
        PoolManager.Instance.Unspawn(gameObject);
    }
}
