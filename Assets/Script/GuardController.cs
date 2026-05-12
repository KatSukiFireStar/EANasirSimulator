using System;
using System.Collections;
using Event;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private float _recognitionDistance = 5;

    [SerializeField]
    private float _recognitionAngle = 15;

    [SerializeField]
    private float soundDistance = 10;
    
    private NavMeshAgent _agent;
    private Coroutine LostCoroutine;
    private bool agentMoving = false;

    private void Awake()
    {
        //Rotate every 5 second - CHANGE IT LATER
        //InvokeRepeating("Rotate", 0f, 5f);
        _agent = GetComponent<NavMeshAgent>();
        
        EventManager.AddListener<Vector3>("PlayerAttack", PlayerAttack);
    }
    
    private void Update()
    {
        if (CheckForPlayer())
        {
            if(LostCoroutine != null)
                StopCoroutine(LostCoroutine);
            Trigger(playerPos.position);
        }

        if (agentMoving && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            agentMoving = false;
            LostCoroutine = StartCoroutine("LostPlayerRoutine");
        }
    }

    /// <summary>
    /// Rotate the guard. Use the forward axis and rotate with angle.
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    private void Rotate(float angle = 90)
    {
        transform.Rotate(Vector3.forward, angle);
    }

    private void PlayerAttack(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < soundDistance)
        {
            Trigger(target);
        }
    }

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
    }

    /// <summary>
    /// Check if the player is in the "light" 
    /// </summary>
    private bool CheckForPlayer()
    {
        float angle = Mathf.Acos(Vector3.Dot(-Vector3.Normalize(transform.up), Vector3.Normalize(transform.position - playerPos.position)));
        angle = angle * 180 / Mathf.PI;
        if (angle < _recognitionAngle)
        {
            if (Vector3.Distance(transform.position, playerPos.position) < _recognitionDistance)
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
    public void Trigger(Vector2 pos)
    {
        _agent.SetDestination(new(pos.x, pos.y, transform.position.z));
        agentMoving = true;
        Debug.Log("Je te vois");
    }
}
