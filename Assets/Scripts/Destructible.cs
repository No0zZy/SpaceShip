using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [Header("Destructible")]

    [SerializeField] private string m_ObjectName;
    public string Name => m_ObjectName;

    [SerializeField] private float m_MaxHitPoints;
    public float MaxHitPoints => m_MaxHitPoints;
    [SerializeField] private ImpactEffect m_ExplosionPrefab;
    private float m_HitPoints;
    public float HitPoints => m_HitPoints;

    [SerializeField] private int m_TeamId;
    public int TeamId => m_TeamId;

    public void SetTeamId(int team) => m_TeamId = team;

    public const int TeamIdNeutral = 0;

    #region All destructibles

    private static HashSet<Destructible> m_AllDestructibles;

    public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

    protected virtual void OnEnable()
    {
        if (m_AllDestructibles == null)
            m_AllDestructibles = new HashSet<Destructible>();

        m_AllDestructibles.Add(this);
    }
    protected virtual void OnDisable()
    {
        if (m_AllDestructibles != null)
            m_AllDestructibles.Remove(this);
    }

    #endregion

    public event System.Action Death;


    [SerializeField] private AudioController m_Audio;
    [SerializeField] private AudioClip m_ExplosionSound;

    protected virtual void Start()
    {
        isDead = false;
        m_HitPoints = m_MaxHitPoints;
        SpawnTargetBox();
    }

    private TargetBox m_TargetBox;

    private void SpawnTargetBox()
    {
        m_TargetBox = TargetHudController.Instance.SpawnTargetBox(this);
    }

    private void UnSpawnTargetBox()
    {
        Destroy(m_TargetBox.gameObject);
    }


    private bool isDead;

    public void ApplyDamage(float damage)
    {
        m_HitPoints -= damage;

        if(m_HitPoints <= 0 && !isDead)
        {
            isDead = true;
            Explode();
        }
    }

    protected virtual void Explode()
    {
        UnSpawnTargetBox();
        if (m_ExplosionPrefab)
        {
            var exp = PoolManager.Instance.Spawn(m_ExplosionPrefab.gameObject);
            exp.transform.position = transform.position;
        }
        if (m_ExplosionSound != null)
            m_Audio.PlayAudioClip(m_ExplosionSound);
        Death?.Invoke();

        OnDestroyed?.Invoke();

        Destroy(gameObject);
    }

    [SerializeField] private UnityEvent m_OnDestroyed;
    public UnityEvent OnDestroyed;

    public virtual Vector3 LinearVelocity => Vector3.zero;

    public virtual Vector3 AverageTurretLaunchPosition => transform.position;
    public virtual float AverageTurretLaunchVelocity => 0;
}
