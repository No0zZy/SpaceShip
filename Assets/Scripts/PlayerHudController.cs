using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHudController : MonoBehaviour
{
    public static PlayerHudController Instance { get; private set; }

    [SerializeField] private Player m_Player;
    public SpaceShip PlayerShip { get { return m_Player.PlayerShip; } }

    [SerializeField] private Slider m_HitPointsSlider;
    [SerializeField] private Text m_HitPointsText;
    [SerializeField] private Text m_MaxHitPointsText;
    [SerializeField] private Text m_SpeedText;

    [SerializeField] private RectTransform[] m_WeaponPanelPoints;
    private List<WeaponPanel> m_WeaponPanels;
    [SerializeField] private WeaponPanel m_WeaponPanelPrefab;
    [SerializeField] private RectTransform m_ActiveWeaponPanel;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }



    private void Update()
    {
        if (!PlayerShip)
            return;
        float percentHP = PlayerShip.HitPoints / PlayerShip.MaxHitPoints;
        m_HitPointsSlider.value = percentHP;
        m_HitPointsText.text = PlayerShip.HitPoints.ToString();
        m_MaxHitPointsText.text = PlayerShip.MaxHitPoints.ToString();
        m_SpeedText.text = ((int)PlayerShip.LinearVelocity.magnitude).ToString();
    }

    public void SetActiveWeaponPanelPosition(int pointIndex)
    {
        m_ActiveWeaponPanel.position = m_WeaponPanelPoints[pointIndex].position;
    }

    public void SpawnWeaponPanel(Weapon weapon, int num)
    {
        if (m_WeaponPanels == null)
        {
            m_WeaponPanels = new List<WeaponPanel>();
        }
        var w = Instantiate(m_WeaponPanelPrefab.gameObject, transform.parent);
        WeaponPanel wp = w.GetComponent<WeaponPanel>();
        wp.ActivatePanel(num.ToString(), weapon.WeaponImage);
        wp.GetComponent<RectTransform>().position = m_WeaponPanelPoints[m_WeaponPanels.Count].position;
        m_WeaponPanels.Add(wp);
    }
}
