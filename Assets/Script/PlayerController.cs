using System;
using Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
	[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
	public class PlayerController : MonoBehaviour
	{
		private InputActions _input;
		private Rigidbody2D _rb;

		[SerializeField]
		private float speed = 10;

		[SerializeField]
		private float damage;

		private Vector2 move;
		private Animator _animator;

		private IAttackable _attackedObject;
		private IInteractable _interactableObject;

		[SerializeField]
		private int maxCopperQuantity = 20;
		[SerializeField]
		private int _copperQuantity = 0;

		private void Awake()
		{
			_input = new InputActions();
			_input.Player.Enable();

			_rb = GetComponent<Rigidbody2D>();

			_input.Player.Attack.performed += AttackOnperformed;
			_input.Player.Interact.performed += InteractOnperformed;
			_animator = GetComponent<Animator>();

			EventManager.AddListener<IAttackable>("AttackTriggerBox", AttackTriggerBox);
			EventManager.AddListener("RemoveTriggerBox", RemoveTriggerBox);
			EventManager.AddListener<IInteractable>("InteractTriggerBox", InteractTriggerBox);
			EventManager.AddListener("RemoveInteractTriggerBox", RemoveInteractTriggerBox);
		}

#region Events

		private void InteractOnperformed(InputAction.CallbackContext obj)
		{
			_copperQuantity += _interactableObject?.Interact() ?? 0;
		}

		private void AttackOnperformed(InputAction.CallbackContext obj)
		{
			EventManager.InvokeEvent("PlayerAttack", transform.position);
			_animator.SetTrigger("Attack");
			_attackedObject?.IsAttacked(damage);
		}

		private void AttackTriggerBox(IAttackable obj)
		{
			_attackedObject = obj;
		}

		private void RemoveTriggerBox()
		{
			_attackedObject = null;
		}

		private void InteractTriggerBox(IInteractable obj)
		{
			_interactableObject = obj;
		}

		private void RemoveInteractTriggerBox()
		{
			_interactableObject = null;
		}

#endregion

		private void Update()
		{
			move = _input.Player.Move.ReadValue<Vector2>();
		}

		private void FixedUpdate()
		{
			float calculateSpeed = speed;

			if (_copperQuantity <= maxCopperQuantity * 0.5f)
			{
				
			}
			else if (_copperQuantity > maxCopperQuantity * 0.5f && _copperQuantity <= maxCopperQuantity * 0.75f)
			{
				calculateSpeed *= 0.9f;
			}
			else if (_copperQuantity > maxCopperQuantity * 0.75f && _copperQuantity <= maxCopperQuantity)
			{
				calculateSpeed *= 0.8f;
			}
			else
			{
				float mult = -((float)_copperQuantity / (float)maxCopperQuantity) + 1f + 0.8f;
				calculateSpeed *= mult < 0 ? 0f : mult;
			}
			
			Debug.Log(calculateSpeed);
			
			_rb.MovePosition(_rb.position + move * (calculateSpeed * Time.deltaTime));
			float normU = Mathf.Sqrt(Mathf.Pow(move.x, 2) + Mathf.Pow(move.y, 2));

			Vector2 normMove = Vector2.Normalize(new Vector2(move.x, 0));

			if (normU > 0.1f)
			{
				float a = Mathf.Acos(-move.y / normU) * 180 / Mathf.PI;
				if (normMove.x != 0)
					a *= normMove.x;
				_rb.rotation = a;
			}
		}
	}
}