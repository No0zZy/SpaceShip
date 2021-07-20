using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text m_TextDistance;
    [SerializeField] private Text m_TextHitpoints;
    [SerializeField] private Text m_TextName;

    [SerializeField] private Color m_TargetColor;
    [SerializeField] private Color m_NonTargetColor;

    [SerializeField] private Color m_FriendlyColor;
    [SerializeField] private Color m_EnemyColor;

    [SerializeField] private Slider m_Healths;

    public Destructible Target { get; set; }

    public Vector3 m_LocalScale;

    private void Start()
    {
        m_LocalScale = transform.localScale;

        //if (Player.Instance.PlayerShip.TeamId == Target.TeamId)
        //    m_Healths.fillRect.GetComponent<Image>().color = m_FriendlyColor;
        //else
        //    m_Healths.fillRect.GetComponent<Image>().color = m_EnemyColor;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
            TargetHudController.Instance.SetTarget(Target);
    }

    private void Update()
    {
        m_TextName.text = Target.Name;
        m_TextHitpoints.text = ((int)Target.HitPoints).ToString();

        m_Healths.value = Target.HitPoints / Target.MaxHitPoints;

        Vector3 targetPosition = Target.transform.position;

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPosition);

        if (transform.position.z >= 0)
            transform.position = screenPosition;
        else
            transform.position = screenPosition + Vector3.up * 10000f ;

        transform.localScale = (TargetHudController.Instance.PlayerShip.SelectedTarget == Target) ? 
            m_LocalScale : m_LocalScale * 0.7f;

        GetComponent<Image>().color = (TargetHudController.Instance.PlayerShip.SelectedTarget == Target) ?
            m_TargetColor : m_NonTargetColor;
    }
}
