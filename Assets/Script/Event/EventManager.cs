using System;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{
    public static class EventManager
    {
        
        private static Dictionary<string, Delegate> actions = new();


        public static void AddListener<T>(string name, Action<T> action)
        {
            if (!actions.ContainsKey(name))
                actions[name] = action;
            else
                actions[name] = Delegate.Combine(actions[name], action);
            
            Debug.Log($"<color=#00FF00>Add action for event {name}</color>");
        }
        
        public static void AddListener(string name, Action action)
        {
            if (!actions.ContainsKey(name))
                actions[name] = action;
            else
                actions[name] = Delegate.Combine(actions[name], action);
            
            Debug.Log($"<color=#00FF00>Add action for event {name}</color>");
        }

        public static void RemoveListener<T>(string name, Action<T> action)
        {
            if (!actions.ContainsKey(name)) 
                return;
            
            actions[name] = Delegate.Remove(actions[name], action);
            Debug.Log($"<color=#FF00FF>Remove action for event {name}</color>");
        }
        
        public static void RemoveListener(string name, Action action)
        {
            if (!actions.ContainsKey(name)) 
                return;
            
            actions[name] = Delegate.Remove(actions[name], action);
            Debug.Log($"<color=#FF00FF>Remove action for event {name}</color>");
        }

        public static void InvokeEvent(string name)
        {
            if (!actions.ContainsKey(name))
                return;

            (actions[name] as Action)?.Invoke();
            Debug.Log($"<color=#FFFF00>Invoke event {name}</color>");
        }

        public static void InvokeEvent<T>(string name, T arg)
        {
            if (!actions.ContainsKey(name))
                return;
            
            (actions[name] as Action<T>)?.Invoke(arg);
            Debug.Log($"<color=#FFFF00>Invoke event {name} with arg {arg}</color>");
        }
    }
}