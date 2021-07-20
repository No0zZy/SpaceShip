using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeadBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool IsLock { get; private set; }
    public Vector3 LeadPosition { get; private set; }

    private void Update()
    {
        Destructible target = TargetHudController.Instance.PlayerShip.SelectedTarget;

        if (!target)
        {
            gameObject.SetActive(false);
            IsLock = false;
            return;
        }

        Vector3 launchPoint = TargetHudController.Instance.PlayerShip.AverageTurretLaunchPosition;
        float launchVelocity = TargetHudController.Instance.PlayerShip.AverageTurretLaunchVelocity;
        Vector3 targetPos = target.transform.position;
        Vector3 targetVelocity = target.LinearVelocity;
        Vector3 playerVelocity = TargetHudController.Instance.PlayerShip.LinearVelocity;

        LeadPosition = MakeLead(
            launchPoint,
            playerVelocity + (targetPos - launchPoint).normalized * launchVelocity,
            targetPos, 
            targetVelocity);

        transform.position = Camera.main.WorldToScreenPoint(LeadPosition);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsLock = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsLock = false;
    }

    public static Vector3 MakeLead( 
        Vector3 launchPoint, 
        Vector3 launchVelocity,
        Vector3 targetPos, 
        Vector3 targetVelocity)
    {
        Vector3 V = targetVelocity;
        Vector3 D = targetPos - launchPoint;
        float A = V.sqrMagnitude - launchVelocity.sqrMagnitude;
        float B = 2 * Vector3.Dot(D, V);
        float C = D.sqrMagnitude;

        if (A >= 0)
            return targetPos;

        float rt = Mathf.Sqrt(B * B - 4 * A * C);
        float dt1 = (-B + rt) / (2 * A);
        float dt2 = (-B - rt) / (2 * A);
        float dt = dt1 < 0 ? dt2 : dt1;
        return targetPos + V * dt;
    }
}
