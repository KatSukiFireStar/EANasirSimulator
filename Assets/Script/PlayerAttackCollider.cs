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
		else if (other.TryGetComponent(out IInteractable inter))
		{
			EventManager.InvokeEvent("InteractTriggerBox", obj);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.TryGetComponent(out IAttackable obj))
		{
			EventManager.InvokeEvent("RemoveTriggerBox");
		}
		else if (other.TryGetComponent(out IInteractable inter))
		{
			EventManager.InvokeEvent("RemoveInteractTriggerBox");
		}
	}
}
