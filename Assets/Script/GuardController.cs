using System.Collections;
using System.Collections.Generic;
using Event;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class GuardController : MonoBehaviour, IAttackable
{
    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private float m_recognitionDistance = 5;

    [SerializeField]
    private float m_recognitionAngle = 15;

    [SerializeField]
    private float m_soundDistance = 10;
    
    private NavMeshAgent m_agent;
    private Coroutine LostCoroutine;
    private bool m_agentMoving = false;
    [SerializeField]
    private List<Vector3> patrolPoints = new();
    private int m_nextPatrolPoint = 0;

    // Init NavMeshAgent, player attack, patrol points & the guard line of sight
    private void Awake()
    {
        m_agent = GetComponent<NavMeshAgent>();
        
        EventManager.AddListener<Vector3>("PlayerAttack", PlayerAttack);

        LineRenderer lr = GetComponent<LineRenderer>();
        for (int i = 0; i < lr.positionCount; i++)
        {
            var pos = lr.GetPosition(i);
            pos.z = 0;
            patrolPoints.Add(pos);
        }
        Destroy(lr);

        Light2D light = GetComponentInChildren<Light2D>();
        light.pointLightOuterRadius = m_recognitionDistance;
        light.pointLightOuterAngle = m_recognitionAngle;
    }

    // Start patroling routine
    private void Start()
    {
        GoToNextPatrolPoint();
    }

    // Check if the player is in the los (line of sight) else patrol
    private void Update()
    {
        if (CheckForPlayer())
        {
            if (LostCoroutine != null)
                StopCoroutine(LostCoroutine);
            Trigger(playerPos.position);
        }

        if (m_agentMoving && m_agent.remainingDistance <= m_agent.stoppingDistance)
        {
            m_agentMoving = false;
            LostCoroutine = StartCoroutine("LostPlayerRoutine");
        }
    }

    /// <summary>
    /// Rotate the guard. Use the forward axis and rotate with angle.
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    private void Rotate(float _angle = 90)
    {
        transform.Rotate(Vector3.forward, _angle);
    }

    // Guard routine if the player attacks it
    private void PlayerAttack(Vector3 _target)
    {
        if (Vector3.Distance(transform.position, _target) < m_soundDistance)
        {
            Trigger(_target);
        }
    }

    // Guard routine when it loose los (line of sight) of the player 
    private IEnumerator LostPlayerRoutine()
    {
        float nextAngle = 0f;
        Quaternion rotation = transform.rotation;

        Quaternion endAngle = rotation * Quaternion.Euler(0f, 0f, 90f);
        while (nextAngle < 0.75f)
        {
            transform.rotation = Quaternion.Lerp(rotation, endAngle, nextAngle / 0.75f);
            nextAngle += Time.deltaTime;
            yield return null;
        }

        nextAngle = 0f;
        rotation = transform.rotation;
        endAngle = rotation * Quaternion.Euler(0f, 0f, 180f);
        while (nextAngle < 1.5f)
        {
            transform.rotation = Quaternion.Lerp(rotation, endAngle, nextAngle / 1.5f);
            nextAngle += Time.deltaTime;
            yield return null;
        }

        //Go to next point
        GoToNextPatrolPoint();
    }

    // Guard patroling routine
    private void GoToNextPatrolPoint()
    {
        m_agent.SetDestination(patrolPoints[m_nextPatrolPoint]);
        m_nextPatrolPoint = (m_nextPatrolPoint + 1) % patrolPoints.Count;
        m_agentMoving = true;
    }

    /// <summary>
    /// Check if the player is in the "light" 
    /// </summary>
    private bool CheckForPlayer()
    {
        float angle = Mathf.Acos(Vector3.Dot(-Vector3.Normalize(transform.up), Vector3.Normalize(transform.position - playerPos.position)));
        angle = angle * 180 / Mathf.PI;
        if (angle < m_recognitionAngle)
        {
            if (Vector3.Distance(transform.position, playerPos.position) < m_recognitionDistance)
            {
                return true;
            }
        }

        return false;
    }
    
    /// <summary>
    /// Trigger the "see the player comportement"
    /// </summary>
    /// <param name="pos">Position of trigger</param>
    public void Trigger(Vector2 _triggerPos)
    {
        m_agent.SetDestination(new(_triggerPos.x, _triggerPos.y, transform.position.z));
        m_agentMoving = true;
    }

    public void IsAttacked(float damage)
    {
        Debug.Log("Non aled on m'attaque");
    }
}
