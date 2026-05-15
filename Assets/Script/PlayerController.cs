using Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        private InputActions _input;
        private Rigidbody2D _rb;

        [SerializeField]
        private Vector2 speed;
    
        private void Awake()
        {
            _input = new InputActions();
            _input.Player.Enable();
            
            _rb = GetComponent<Rigidbody2D>();
            
            _input.Player.Attack.performed += AttackOnperformed;
        }

        private void AttackOnperformed(InputAction.CallbackContext obj)
        {
            EventManager.InvokeEvent("PlayerAttack", transform.position);
        }

        private void Update()
        {
            var move = _input.Player.Move.ReadValue<Vector2>();
            
            _rb.MovePosition(_rb.position + move * speed * Time.deltaTime);
        }
    }
}

