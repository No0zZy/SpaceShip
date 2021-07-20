using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHudController : MonoBehaviour
{
    [SerializeField] private TargetBox m_TargetBoxPrefab;
    [SerializeField] private Player m_Player;
    public SpaceShip PlayerShip { get { return m_Player.PlayerShip; } }
    public static TargetHudController Instance { get; private set; }

    [SerializeField] private LeadBox m_LeadBox;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_LeadBox.gameObject.SetActive(false);
    }

    public void SetTarget(Destructible newTarget)
    {
        PlayerShip.SelectedTarget = newTarget;

        if(!newTarget.GetComponent<Gate>())
            m_LeadBox.gameObject.SetActive(true);
    }

    public TargetBox SpawnTargetBox(Destructible destructibleObject)
    {
        var e = Instantiate(m_TargetBoxPrefab.gameObject);
        e.transform.SetParent(transform.parent);
        TargetBox t = e.GetComponent<TargetBox>();
        t.Target = destructibleObject;
        if ((Destructible)PlayerShip == destructibleObject)
            t.gameObject.SetActive(false);
        return t;
    }

    private void Update()
    {
        if (!PlayerShip)
            return;

        if (PlayerShip.SelectedTarget && m_LeadBox.IsLock)
            PlayerShip.WorldAimPoint = m_LeadBox.LeadPosition;
        else
            PlayerShip.WorldAimPoint = DefaultMouseAimPoint;
    }

    private const float DefaultAimDistance = 5000f;
    private Vector3 DefaultMouseAimPoint
    {
        get
        {
            //empty space
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 aimPoint = mouseRay.GetPoint(DefaultAimDistance);

            //collider
            RaycastHit hit;
            if(Physics.Raycast(mouseRay, out hit, DefaultAimDistance))
            {
                //we dont point at player ship
                SpaceShip potentialPlayerShip =  hit.collider.transform.root.GetComponent<SpaceShip>();

                if(PlayerShip && PlayerShip == potentialPlayerShip)
                {
                    return aimPoint;
                }

                return hit.point;
            }

            return aimPoint;
        }
    }
}
