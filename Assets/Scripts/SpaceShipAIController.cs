using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpaceShip))]
public class SpaceShipAIController : MonoBehaviour
{
    [SerializeField] private bool m_EnableAI;

    public enum AIBehavior
    {
        Null,
        Patrol,
        Following,
        FindWay
    }

    [SerializeField] private AIBehavior m_AIBehavior;
    private AIBehavior tempAIBehavior;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationLinear;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_NavigationAngular;

    [SerializeField] private AIPointPatrol[] m_PatrolPoints;
    private int m_currentPatrolPointIndex;
    private AIPointPatrol m_currentPatrolPoint;


    private SpaceShip m_SpaceShip;

    [SerializeField] private Transform m_FollowTarget;
    [SerializeField] private float m_FollowDistance;

    private void Start()
    {
        if (m_PatrolPoints.Length > 0)
        {
            m_currentPatrolPointIndex = 0;
            m_currentPatrolPoint = m_PatrolPoints[m_currentPatrolPointIndex];
        }
        m_SpaceShip = GetComponent<SpaceShip>();
        InitActionTimers();
    }

    private void Update()
    {
        UpdateAI();
        UpdateActionTimers();
    }

    private void UpdateAI()
    {
        if (!m_EnableAI)
        {
            return;
        }

        switch (m_AIBehavior)
        {
            case AIBehavior.Null:
                break;

            case AIBehavior.Patrol:
                ActionFindNewMovePosition();
                ActionMoveToPosition();
                CheckForward(); 
                ActionFindNewAttackTarget();
                ActionFire();
                break;

            case AIBehavior.Following:
                ActionFindNewMovePosition();
                bool isOnFollowDistance = (m_FollowTarget.transform.position - transform.position).sqrMagnitude < m_FollowDistance * m_FollowDistance;
                if (!isOnFollowDistance)
                {
                    isBraking = false;
                    ActionMoveToPosition();
                }
                else
                    Braking();
                CheckForward();
                break;
            case AIBehavior.FindWay:
                if (isWayFind && !isBraking)
                    ActionMoveToPosition();
                else
                    Braking();
                CheckForward();
                break;
        }

        Debug.DrawLine(transform.position, m_MovePosition, Color.green);
    }

    private bool isBraking = false;
    [SerializeField] private float BrakingTime;
    /// <summary>
    /// Торможение
    /// </summary>
    private void Braking()
    {
        if (!isBraking)
        {
            isBraking = true;
            SetActionTimer(ActionTimerType.Braking, BrakingTime);
        }
        else
        {
            m_SpaceShip.ControlThrust = -Vector3.forward * m_NavigationLinear;
        }

        if (IsActionTimerFinished(ActionTimerType.Braking))
        {
            //Если в режиме фоловин выставлять флаг, то торможение будет обновляться, и он будет дергаться туда сюда,
            //вылетая за пределы дистанции следования и возвращаясь обратно (костыльно, лучше это обрабатывать в UpdateAI,
            //или вообще ввести состояние остановки)
            if (m_AIBehavior == AIBehavior.FindWay)
                isBraking = false;
            m_SpaceShip.ControlThrust = Vector3.zero;
        }

        m_SpaceShip.ControlTorque = ComputeAlignTorqueNormalized(m_MovePosition, transform) * m_NavigationAngular;
    }

    private Vector3 m_MovePosition;
    private void ActionMoveToPosition()
    {
        m_SpaceShip.ControlThrust = Vector3.forward * m_NavigationLinear;
        m_SpaceShip.ControlTorque = ComputeAlignTorqueNormalized(m_MovePosition, transform) * m_NavigationAngular;
        if (isWayFind && m_AIBehavior == AIBehavior.FindWay)
            switch (findWay)
            {
                case Way.Y:
                    if (m_SpaceShip.transform.position.y >= m_MovePosition.y)
                    {
                        OnWayPoint();
                    }
                    break;
                case Way.X:
                    if (m_SpaceShip.transform.position.x >= m_MovePosition.x)
                    {
                        OnWayPoint();
                    }
                    break;
                case Way.mY:
                    if (m_SpaceShip.transform.position.y <= m_MovePosition.y)
                    {
                        OnWayPoint();
                    }
                    break;
                case Way.mX:
                    if (m_SpaceShip.transform.position.x <= m_MovePosition.x)
                    {
                        OnWayPoint();
                    }
                    break;
            }
    }

    private void OnWayPoint()
    {
        m_AIBehavior = tempAIBehavior;
        isWayFind = false;

    }

    private const float MaxAngle = 45.0f;
    private static Vector3 ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform source)
    {
        Vector3 torque = Vector3.zero;

        Vector3 localTargetPosition = source.InverseTransformPoint(targetPosition);

        //Left & Right
        {
            var lp = localTargetPosition;
            lp.y = 0;
            float angle = Vector3.SignedAngle(lp, Vector3.forward, Vector3.up);
            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;
            torque -= Vector3.up * angle;
        }

        //Up & Down
        {
            var lp = localTargetPosition;
            lp.x = 0;
            float angle = Vector3.SignedAngle(lp, Vector3.forward, Vector3.right);
            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;
            torque -= Vector3.right * angle;
        }

        return torque;
    }


    [SerializeField] private float m_TimeAtPatrolPoint;
    [SerializeField] private float m_RandomizeAlignTime;

    private bool isOnPatrolPoint = false;

    private void ActionFindNewMovePosition()
    {
        switch (m_AIBehavior)
        {
            case AIBehavior.Patrol:
                if(m_SpaceShip.SelectedTarget != null)
                {
                    m_MovePosition = m_SpaceShip.SelectedTarget.transform.position;
                }
                else
                if (m_currentPatrolPoint)
                {
                    bool isInsidePatrolZone = (m_currentPatrolPoint.transform.position - transform.position).sqrMagnitude < m_currentPatrolPoint.Radius * m_currentPatrolPoint.Radius;

                    if (isInsidePatrolZone)
                    {
                        if (!isOnPatrolPoint)
                        {
                            SetActionTimer(ActionTimerType.PatrolPoint, m_TimeAtPatrolPoint);
                            isOnPatrolPoint = true;
                        }
                        if (IsActionTimerFinished(ActionTimerType.PatrolPoint))
                        {
                            isOnPatrolPoint = false;
                            if (m_currentPatrolPointIndex + 1 < m_PatrolPoints.Length)
                                m_currentPatrolPoint = m_PatrolPoints[++m_currentPatrolPointIndex];
                            else
                                m_currentPatrolPoint = m_PatrolPoints[m_currentPatrolPointIndex = 0];
                        }

                        if (IsActionTimerFinished(ActionTimerType.RandomizeDirection))
                        {
                            Vector3 newPoint = UnityEngine.Random.onUnitSphere * m_currentPatrolPoint.Radius + m_currentPatrolPoint.transform.position;
                            m_MovePosition = newPoint;
                            SetActionTimer(ActionTimerType.RandomizeDirection, m_RandomizeAlignTime);
                        }
                    }
                    else
                    {
                        m_MovePosition = m_currentPatrolPoint.transform.position;
                    }
                }
                break;
            case AIBehavior.Following:
                if (m_FollowTarget)
                {
                    m_MovePosition = m_FollowTarget.transform.position;
                }
                break;
        }
    }

    private bool isWayFind = false;
    /// <summary>
    /// Время, на которое умножается скорость, чтобы узнать дистанцию
    /// </summary>
    [SerializeField] private float m_TimeDistance;
    [SerializeField] private float m_StepOfRay;
    private float currentStep;

    private float tempDistance;
    private float moveDistance;

    private void CheckForward()
    {
        RaycastHit hit;
        var checkDistance = m_SpaceShip.LinearVelocity.magnitude * m_TimeDistance;
        if (Physics.Raycast(transform.position, transform.forward, out hit, checkDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.red);
            if (m_AIBehavior != AIBehavior.FindWay)
            {
                if (hit.transform.GetComponent<SpaceShip>())
                    return;

                tempDistance = checkDistance * 1.5f; // если под углом рейкаст хитнул, конечно это не особо поможет с огромными объектами, но все же
                moveDistance = hit.distance * 0.8f;
                Braking();
                tempAIBehavior = m_AIBehavior;
                m_AIBehavior = AIBehavior.FindWay;
                isWayFind = false;
                currentStep = m_StepOfRay;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * checkDistance, Color.yellow);
        }

        if (!isWayFind && m_AIBehavior == AIBehavior.FindWay)
        {
            if (CheckAround(transform.up))
            {
                findWay = Way.Y;
                return;
            }
            if (CheckAround(transform.right))
            {
                findWay = Way.X;
                return;
            }
            if (CheckAround(-transform.up))
            {
                findWay = Way.mY;
                return;
            }
            if (CheckAround(-transform.right))
            {
                findWay = Way.mX;
                return;
            }
        }

    }

    private enum Way
    {
        Y,
        X,
        mY,
        mX
    }
    private Way findWay;

    private bool CheckAround(Vector3 way)
    {
        RaycastHit hit;
        if (!Physics.Raycast(transform.position + way * currentStep, transform.forward, out hit, tempDistance))
        {
            Debug.DrawRay(transform.position, transform.forward * tempDistance, Color.yellow);
            if (!Physics.Raycast(transform.position + way * (currentStep + m_StepOfRay), transform.forward, out hit, tempDistance))
            {
                Debug.DrawRay(transform.position, transform.forward * tempDistance, Color.yellow);
                m_MovePosition = transform.position + way * (currentStep + m_StepOfRay) + transform.forward * moveDistance;
                isWayFind = true;
                return true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * tempDistance, Color.red);
                currentStep += m_StepOfRay;
                return false;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * tempDistance, Color.red);
            currentStep += m_StepOfRay;
            return false;
        }
    }

    private enum ActionTimerType
    {
        Null,

        PatrolPoint,
        RandomizeDirection,
        Fire,
        FindNewTarget,
        Braking,

        MaxValues
    }

    private float[] m_ActionTimers;

    private void InitActionTimers()
    {
        m_ActionTimers = new float[(int)ActionTimerType.MaxValues];
    }

    private void UpdateActionTimers()
    {
        for (int i = 0; i < m_ActionTimers.Length; i++)
        {
            if (m_ActionTimers[i] > 0)
                m_ActionTimers[i] -= Time.deltaTime;
        }
    }

    private void SetActionTimer(ActionTimerType e, float time)
    {
        m_ActionTimers[(int)e] = time;
    }

    private bool IsActionTimerFinished(ActionTimerType e)
    {
        return m_ActionTimers[(int)e] <= 0;
    }

    private void ActionFindNewAttackTarget()
    {
        m_SpaceShip.SelectedTarget = FindNearestDestructibleTarget();
    }

    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_Accuracy;
    [SerializeField] private float m_MaxScatterDistance;

    [SerializeField] private float m_RateFireTime;

    private void ActionFire()
    {
        if (!m_SpaceShip.SelectedTarget)
            return;
        Vector3 launchPoint = TargetHudController.Instance.PlayerShip.AverageTurretLaunchPosition;
        float launchVelocity = TargetHudController.Instance.PlayerShip.AverageTurretLaunchVelocity;
        Vector3 targetPos = m_SpaceShip.SelectedTarget.transform.position;
        Vector3 targetVelocity = m_SpaceShip.SelectedTarget.LinearVelocity;
        Vector3 playerVelocity = TargetHudController.Instance.PlayerShip.LinearVelocity;

        var scatter = Random.Range(-m_MaxScatterDistance, m_MaxScatterDistance) * (1 - m_Accuracy);
        Vector3 scatterVector = new Vector3(scatter, scatter, scatter);

        m_SpaceShip.WorldAimPoint = LeadBox.MakeLead(
            launchPoint,
            playerVelocity + (targetPos - launchPoint).normalized * launchVelocity,
            targetPos,
            targetVelocity) + scatterVector ;


        if(IsActionTimerFinished(ActionTimerType.Fire))
        {
            m_SpaceShip.FireActiveWeapon();

            SetActionTimer(ActionTimerType.Fire, UnityEngine.Random.Range(0.0f, m_RateFireTime));
        }
    }

    private Destructible FindNearestDestructibleTarget()
    {
        float dist2 = -1;

        Destructible potentialTarget = null;

        foreach (var v in Destructible.AllDestructibles)
        {
            if (v.GetComponent<SpaceShip>() == m_SpaceShip)
                continue;

            if (Destructible.TeamIdNeutral == v.TeamId)
                continue;

            if (m_SpaceShip.TeamId == v.TeamId)
                continue;

            float d2 = (m_SpaceShip.transform.position - v.transform.position).sqrMagnitude;

            if(dist2 < 0 || d2 < dist2)
            {
                potentialTarget = v;
                dist2 = d2;
            }
        }

        return potentialTarget;
    }    

    public void SetPatrolBehavior(AIPointPatrol[] points)
    {
        m_AIBehavior = AIBehavior.Patrol;
        m_PatrolPoints = points;
    }

    public void SetAccuracy(float accur)
    {
        m_Accuracy = Mathf.Clamp01(accur); 
    }
}