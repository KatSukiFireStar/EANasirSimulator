using System.Collections.Generic;
using EANasir.Interface;
using EANasir.Object;
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EANasir.Player
{
	[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
	public class PlayerController : MonoBehaviour
	{
		private InputActions m_input;
		private Rigidbody2D m_rb;

		private Vector2 m_move;
		private Animator m_animator;

		private IAttackable m_attackedObject;
		private IInteractable m_interactableObject;

        [SerializeField] private float m_speed = 10f;
        [SerializeField] private float m_damage;

        private float m_calculateSpeed = 0;
        
		[SerializeField] private float m_maxCopperQuantity = 20f;
		[SerializeField] private float m_copperQuantity = 0f;
		
		private List<QuestObjectSO> m_questObjects = new();

		/// <summary>
		/// Init rb, player animator and other params
		/// </summary>
		private void Awake()
		{
            m_rb = GetComponent<Rigidbody2D>();
            m_animator = GetComponent<Animator>();

            m_calculateSpeed = m_speed;

            m_input = new InputActions();
            m_input.Player.Enable();
            m_input.Player.Attack.performed += AttackOnPerformed;
			m_input.Player.Interact.performed += InteractOnPerformed;

			EventManager.AddListener<IAttackable>("AttackTriggerBox", AttackTriggerBox);
			EventManager.AddListener("RemoveTriggerBox", RemoveTriggerBox);
			EventManager.AddListener<IInteractable>("InteractTriggerBox", InteractTriggerBox);
			EventManager.AddListener("RemoveInteractTriggerBox", RemoveInteractTriggerBox);
			
			EventManager.AddListener<QuestObjectSO>("AddQuestObjectToInventory", AddQuestObjectToInventory);
			
			EventManager.AddListener("EndLevel", EndLevel);
		}

        public void ReSpawnToCheckPoint() {
			// Could be cool to add a fade to black or idk just a big pop up that show and you can restart the game really
			//  fast like in Hotline Miami
			Debug.Log( "You have been caught, returning to checkpoint" );
			transform.position = GameObject.Find("SpawnPoint").transform.position; // System.SpawnPoint >()
        }

#region Events
        /// <summary>
        /// Add copper and change calculate speed depending of copper quantity
        /// </summary>
        private void InteractOnPerformed(InputAction.CallbackContext _)
		{
			m_copperQuantity += m_interactableObject?.Interact() ?? 0;
			m_calculateSpeed = m_speed;

			if (m_copperQuantity <= m_maxCopperQuantity * 0.5f)
			{
				// No changement
			}
			else if (m_copperQuantity > m_maxCopperQuantity * 0.5f && m_copperQuantity <= m_maxCopperQuantity * 0.75f)
			{
				m_calculateSpeed *= 0.9f;
			}
			else if (m_copperQuantity > m_maxCopperQuantity * 0.75f && m_copperQuantity <= m_maxCopperQuantity)
			{
				m_calculateSpeed *= 0.8f;
			}
			else
			{
				float mult = -(m_copperQuantity / m_maxCopperQuantity) + 1f + 0.8f;
				m_calculateSpeed *= mult < 0 ? 0f : mult;
			}
		}

		private void AttackOnPerformed(InputAction.CallbackContext _)
		{
			EventManager.InvokeEvent("PlayerAttack", transform.position);
			m_animator.SetTrigger("Attack");
			m_attackedObject?.IsAttacked(m_damage);
		}

		// Trigger box behavior
		private void AttackTriggerBox(IAttackable _obj) { m_attackedObject = _obj; }
		private void RemoveTriggerBox() { m_attackedObject = null; }
		private void InteractTriggerBox(IInteractable _obj) { m_interactableObject = _obj; }
		private void RemoveInteractTriggerBox() { m_interactableObject = null; }

		private void AddQuestObjectToInventory(QuestObjectSO _obj)
		{
			m_questObjects.Add(_obj);
			
			//Add the object to UI
		}

		private void EndLevel()
		{
			Debug.Log("Game Over");
			m_copperQuantity = 0;
			m_calculateSpeed = m_speed;
			m_questObjects = new();
		}
		
#endregion

		/// <summary>
		/// update m_move depending on player control
		/// </summary>
		private void Update() { m_move = m_input.Player.Move.ReadValue<Vector2>(); }

		private void FixedUpdate()
		{
			m_rb.MovePosition(m_rb.position + m_move * (m_calculateSpeed * Time.deltaTime));
			float normU = Mathf.Sqrt(Mathf.Pow(m_move.x, 2) + Mathf.Pow(m_move.y, 2));

			Vector2 normMove = Vector2.Normalize(new Vector2(m_move.x, 0));

			if (normU > 0.1f)
			{
				float a = Mathf.Acos(-m_move.y / normU) * 180 / Mathf.PI;
				if (normMove.x != 0)
					a *= normMove.x;
				m_rb.rotation = a;
			}
		}
	}
}