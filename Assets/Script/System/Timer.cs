using System;
using Event;
using TMPro;
using UnityEngine;

namespace EANasir.System
{
	public class Timer : MonoBehaviour
	{
		[SerializeField]
		private float m_lvlTimer;

		[SerializeField]
		private TextMeshProUGUI m_text;

		private float m_timer;

		private void Awake()
		{
			m_timer = m_lvlTimer;
		}

		private void Update()
		{
			m_timer -= Time.deltaTime;
			if (m_timer <= 0)
			{
				EventManager.InvokeEvent("EndLevel");
				return;
			}
			
			m_text.text = m_timer.ToString("0.00");
		}
	}
}
