using Event;
using UnityEngine;

namespace EANasir.System
{
	public class Timer : MonoBehaviour
	{
		[SerializeField]
		private float m_lvlTimer;

		private float m_timer;
		
		private void Update()
		{
			m_timer += Time.deltaTime;
			if (m_timer >= m_lvlTimer)
			{
				EventManager.InvokeEvent("EndLevel");
			}
		}
	}
}
