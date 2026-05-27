using System;
using EANasir.Interface;
using EANasir.Player;
using UnityEngine;

namespace EANasir.Object
{
	[RequireComponent(typeof(SpriteRenderer))]
	public class DefenseObject : MonoBehaviour, IInteractable
	{
		[SerializeField]
		private float m_cost;

		[SerializeField]
		private Sprite m_completeSPrite;

		private SpriteRenderer m_renderer;
		private bool m_isComplete = false;

		private void Awake()
		{
			m_renderer = GetComponent<SpriteRenderer>();
		}

		/// <summary>
		/// If not already build and player has enough money, build the object
		/// </summary>
		/// <param name="_player"></param>
		/// <returns></returns>
		public float Interact(PlayerController _player)
		{
			if (!m_isComplete && _player.savedCopperQuantity < m_cost)
				return 0f;
			
			m_isComplete = true;
			m_renderer.sprite = m_completeSPrite;
			_player.savedCopperQuantity -= m_cost;
			return 0f;
		}
	}
}
