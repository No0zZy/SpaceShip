using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Turret[] m_Turrets;
    public Turret[] Turrets => m_Turrets;

    [SerializeField] private Sprite m_WeaponImage;
    public Sprite WeaponImage => m_WeaponImage;
}
