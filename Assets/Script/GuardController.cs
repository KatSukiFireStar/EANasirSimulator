using System;
using Unity.VisualScripting;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private float _recognitionDistance = 5;

    [SerializeField]
    private float _recognitionAngle = 15;
    
    private void Update()
    {
        float angle = Mathf.Acos(Vector3.Dot(Vector3.Normalize(transform.up), Vector3.Normalize(transform.position - playerPos.position)));
        angle = angle * 180 / Mathf.PI;
        if (angle < _recognitionAngle)
        {
            if (Vector3.Distance(transform.position, playerPos.position) < _recognitionDistance)
            {
                Trigger(playerPos.position);
            }
        }
    }

    public void Trigger(Vector2 pos)
    {
        Debug.Log("Je te vois");
    }
}
