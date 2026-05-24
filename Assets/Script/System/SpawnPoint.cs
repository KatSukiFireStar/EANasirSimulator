using System;
using UnityEngine;

namespace EANasir.System
{
	public class SpawnPoint : MonoBehaviour
	{
		public static SpawnPoint Instance { get; private set; }
		
		public Vector3 m_position { get; private set; }

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
				m_position = transform.position;
			}
			else
				Destroy(gameObject);
		}
	}
}
