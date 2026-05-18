using Event;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D _other)
	{
		if (_other.TryGetComponent(out IAttackable _obj))
		{
			EventManager.InvokeEvent("AttackTriggerBox", _obj);
		}
		if (_other.TryGetComponent(out IInteractable _inter))
		{
			EventManager.InvokeEvent("InteractTriggerBox", _inter);
		}
	}

	private void OnTriggerExit2D(Collider2D _other)
	{
        if (_other.TryGetComponent(out IAttackable _))
		{
			EventManager.InvokeEvent("RemoveTriggerBox");
		}
		if (_other.TryGetComponent(out IInteractable _))
		{
			EventManager.InvokeEvent("RemoveInteractTriggerBox");
		}
	}
}
