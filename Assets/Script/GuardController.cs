using System;
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
    
    private NavMeshAgent _agent;

    private void Awake()
    {
        //Rotate every 5 second - CHANGE IT LATER
        //InvokeRepeating("Rotate", 0f, 5f);
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (CheckForPlayer())
        {
            Trigger(playerPos.position);
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

    /// <summary>
    /// Check if the player is in the "light" 
    /// </summary>
    private bool CheckForPlayer()
    {
        float angle = Mathf.Acos(Vector3.Dot(Vector3.Normalize(transform.up), Vector3.Normalize(transform.position - playerPos.position)));
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
        Debug.Log("Je te vois");
    }
}
