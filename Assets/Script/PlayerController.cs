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
        private Vector2 speed;
        [SerializeField]
        private float damage;
        
        
        private Vector2 move;
        private Animator _animator;
        
        private IAttackable _attackedObject;
    
        private void Awake()
        {
            _input = new InputActions();
            _input.Player.Enable();
            
            _rb = GetComponent<Rigidbody2D>();
            
            _input.Player.Attack.performed += AttackOnperformed;
            _animator = GetComponent<Animator>();
            
            EventManager.AddListener<IAttackable>("AttackTriggerBox", AttackTriggerBox);
            EventManager.AddListener("RemoveTriggerBox", RemoveTriggerBox);
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

        private void Update()
        {
            move = _input.Player.Move.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            _rb.MovePosition(_rb.position + move * speed * Time.deltaTime);
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

