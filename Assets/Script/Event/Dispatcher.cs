using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Event
{
	public enum LogicalType
	{
		AND,
		OR,
		NOT,
		NOR,
		NAND,
		XOR,
		TRUE
	}

	public class Dispatcher : MonoBehaviour
	{
		[HideInInspector]
		public LogicalType logicalDoor;
		[HideInInspector]
		public List<Emetteur> emetteurs = new List<Emetteur>();
		[HideInInspector]
		public Emetteur emetteur;
		[HideInInspector]
		public UnityEvent events;

		private void Start()
		{
			foreach (var e in emetteurs)
			{
				e.OnTrigger += Logic;
				e.Value = false;
			}

			if (emetteur)
			{
				emetteur.OnTrigger += Logic;
				emetteur.Value = false;
			}
		}

		private void Logic()
		{
			bool value = false;

			switch (logicalDoor)
			{
				case LogicalType.AND:
					value = true;
					foreach (var e in emetteurs)
						value &= e.Value;
					break;
				case LogicalType.OR:
					foreach (var e in emetteurs)
						value |= e.Value;
					break;
				case LogicalType.NOT:
					value = !emetteur.Value;
					break;
				case LogicalType.TRUE:
					value = emetteur.Value;
					break;
				case LogicalType.XOR:
					foreach (var e in emetteurs)
						value ^= e.Value;
					break;
				case LogicalType.NOR:
					value = true;
					foreach (var e in emetteurs)
					{
						if (e.Value)
						{
							value = false;
							break;
						}
					}
					break;
				case LogicalType.NAND:
					foreach (var e in emetteurs)
					{
						if (!e.Value)
						{
							value = true;
							break;
						}
					}
					break;
			}
			
			if(value)
				events?.Invoke();
		}
	}
#if UNITY_EDITOR
	[CustomEditor(typeof(Dispatcher)), CanEditMultipleObjects]
	public class DispatcherEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			Dispatcher dispatcher = (Dispatcher)target;
			
			dispatcher.logicalDoor = (LogicalType)EditorGUILayout.EnumPopup("Logical Door",dispatcher.logicalDoor);
			
			if(dispatcher.logicalDoor == LogicalType.NOT || dispatcher.logicalDoor == LogicalType.TRUE)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("emetteur"), true);
			else
				EditorGUILayout.PropertyField(serializedObject.FindProperty("emetteurs"), true);
				
			EditorGUILayout.PropertyField(serializedObject.FindProperty("events"), true);
			
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}
