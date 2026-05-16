using System;
using Event;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.name + " est dans ma boite de collision");
		
		EventManager.InvokeEvent("AttackTriggerBox", other.gameObject);
	}
}
