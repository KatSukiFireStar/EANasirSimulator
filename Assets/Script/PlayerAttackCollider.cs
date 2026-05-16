using System;
using Event;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent(out IAttackable obj))
		{
			EventManager.InvokeEvent("AttackTriggerBox", obj);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.TryGetComponent(out IAttackable obj))
		{
			EventManager.InvokeEvent("RemoveTriggerBox");
		}
	}
}
