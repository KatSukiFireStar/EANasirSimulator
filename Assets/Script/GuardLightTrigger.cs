using System;
using Player;
using UnityEngine;

public class GuardLightTrigger : MonoBehaviour
{
	private GuardController _guard;

	private void Awake()
	{
		_guard = GetComponentInParent<GuardController>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent(out PlayerController player))
		{
			_guard.Trigger(player.transform.position);
		}
	}
}
