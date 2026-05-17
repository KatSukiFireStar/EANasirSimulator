using System;
using UnityEngine;

namespace Event
{
    public abstract class Emetteur : MonoBehaviour
    {
        public Action OnTrigger { get; set; }
        
        public bool Value { get; set; }

        public void Trigger()
        {
            Value = true;
            OnTrigger?.Invoke();
        }
    }
}