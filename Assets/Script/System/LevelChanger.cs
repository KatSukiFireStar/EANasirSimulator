using System;
using EANasir.Player;
using Event;
using UnityEngine;

namespace EANasir.System
{
	public class LevelChanger : MonoBehaviour
	{
		[SerializeField]
		private string m_levelName;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent<PlayerController>(out var _))
				EventManager.InvokeEvent<string>("StartLevel", m_levelName);
		}
	}
}
