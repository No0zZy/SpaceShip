using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShip : Destructible
{
    [Header("Space ship")]

    [SerializeField] private SpaceShipParameters m_SpaceShipParameters;
    public float MaxLinearVelocity => m_SpaceShipParameters.MaxLinearVelocity;

    [SerializeField] private Weapon[] m_Weapons;
    public Weapon[] Weapons => m_Weapons;

    [SerializeField] private Weapon m_ActiveWeapon;

    [SerializeField] private Transform m_ThirdPersonCameraPoint;
    public Transform ThirdPersonCameraPoint => m_ThirdPersonCameraPoint;

    public Weapon ActiveWeapon => m_ActiveWeapon;

    private int currentWeaponIndex;
    private Rigidbody m_Rigid;

    public Vector3 ControlThrust { get; set; }
    public Vector3 ControlTorque { get; set; }

    public Destructible SelectedTarget { get; set; }


    [SerializeField] private bool m_EnableEngineSound;
    [SerializeField] private AudioController m_EngineSound;

    [SerializeField] private float m_MinEnginePitch;
    [SerializeField] private float m_MaxEnginePitch;

    [SerializeField] private Transform m_ArrowPoint;
    public Transform ArrowPoint => m_ArrowPoint;

    public Vector3 WorldAimPoint
    {
        set
        {
            foreach (var v in m_ActiveWeapon.Turrets)
                v.WorldAimPoint = value;
        }
    }

    private void Awake()
    {
        m_Rigid = GetComponent<Rigidbody>();

        if (m_Weapons.Length > 0)
        {
            currentWeaponIndex = 0;
            m_ActiveWeapon = m_Weapons[currentWeaponIndex];
        }
    }


    override protected void Start()
    {
        base.Start();

        if (Player.Instance.PlayerShip == this)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                PlayerHudController.Instance.SpawnWeaponPanel(Weapons[i], i + 1);
            }
        }
    }

    private void FixedUpdate()
    {
        UpdateRigidbody();
    }

    private void Update()
    {
        if(m_EnableEngineSound)
            UpdateEnginePitch();
        else
            m_EngineSound.UpdatePitch(0f);
    }

    private void UpdateEnginePitch()
    {
        float percentSpeed = Mathf.Abs(LinearVelocity.magnitude / MaxLinearVelocity);
        float deltaPitch = m_MaxEnginePitch - m_MinEnginePitch;
        float newPitch = m_MinEnginePitch + percentSpeed * deltaPitch;

        m_EngineSound.UpdatePitch(newPitch);
    }


    public void NextWeapon(bool isPlayer)
    {
        if (m_Weapons.Length == 0)
            return;
        if(currentWeaponIndex + 1 <= m_Weapons.Length - 1 )
            m_ActiveWeapon = m_Weapons[++currentWeaponIndex];
        else
            m_ActiveWeapon = m_Weapons[currentWeaponIndex = 0];

        if (isPlayer)
            PlayerHudController.Instance.SetActiveWeaponPanelPosition(currentWeaponIndex);
    }

    public void PreviousWeapon(bool isPlayer)
    {
        if (m_Weapons.Length == 0)
            return;
        if (currentWeaponIndex - 1 >= 0)
            m_ActiveWeapon = m_Weapons[--currentWeaponIndex];
        else
            m_ActiveWeapon = m_Weapons[currentWeaponIndex = m_Weapons.Length - 1];

        if (isPlayer)
            PlayerHudController.Instance.SetActiveWeaponPanelPosition(currentWeaponIndex);
    }

    public void FireActiveWeapon()
    {
        foreach (var t in m_ActiveWeapon.Turrets)
            t.Fire(this);
    }

    private void UpdateRigidbody()
    {
        m_Rigid.AddRelativeForce(Time.fixedDeltaTime * m_SpaceShipParameters.ThrustForce * ControlThrust, ForceMode.Force);

        float DragCoeff = m_SpaceShipParameters.ThrustForce / m_SpaceShipParameters.MaxLinearVelocity;
        m_Rigid.AddForce(-m_Rigid.velocity * DragCoeff * Time.fixedDeltaTime, ForceMode.Force);

        m_Rigid.AddRelativeTorque(Time.fixedDeltaTime * m_SpaceShipParameters.TorqueForce * ControlTorque, ForceMode.Force);

        // angular velocity limit
        var W = m_Rigid.angularVelocity;

        W.x = Mathf.Clamp(W.x, -m_SpaceShipParameters.MaxAngularVelocity, m_SpaceShipParameters.MaxAngularVelocity);
        W.y = Mathf.Clamp(W.y, -m_SpaceShipParameters.MaxAngularVelocity, m_SpaceShipParameters.MaxAngularVelocity);
        W.z = Mathf.Clamp(W.z, -m_SpaceShipParameters.MaxAngularVelocity, m_SpaceShipParameters.MaxAngularVelocity);

        m_Rigid.angularVelocity = W;
    }

    

    public override Vector3 LinearVelocity => m_Rigid.velocity;
    public override Vector3 AverageTurretLaunchPosition
    {
        get
        {
            if (!m_ActiveWeapon || m_Weapons.Length == 0)
                return transform.position;

            Vector3 pos = Vector3.zero;

            foreach(var t in m_ActiveWeapon.Turrets)
            {
                pos += t.LaunchPosition;
            }

            return pos / m_ActiveWeapon.Turrets.Length;
        }
    }

    public override float AverageTurretLaunchVelocity
    {
        get
        {
            if (!m_ActiveWeapon || m_Weapons.Length == 0)
                return 0;

            float vel = 0;
            foreach (var t in m_ActiveWeapon.Turrets)
            {
                vel += t.ProjectilePrefab.Velocity;
            }

            return vel / m_ActiveWeapon.Turrets.Length;
        }
    }
}
