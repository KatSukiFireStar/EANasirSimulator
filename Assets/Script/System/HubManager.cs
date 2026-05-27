using System;
using System.Collections.Generic;
using EANasir.Object;
using Event;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubManager : MonoBehaviour
{
	private List<bool> m_defenseObjects = new();
	
	private GameObject m_defenseObjectPrefab;

	[SerializeField]
	private string m_hubName;
	
	
	private void Awake()
	{
		DontDestroyOnLoad(this);
		
		EventManager.AddListener<int>("BuildDefenseObject", BuildDefenseObject);
		
		SceneManager.activeSceneChanged += SceneManagerOnactiveSceneChanged;
		m_defenseObjectPrefab = GameObject.FindGameObjectWithTag("DefenseObject");
		for (int i = 0; i < m_defenseObjectPrefab.transform.childCount; i++)
		{
			m_defenseObjects.Add(false);
			m_defenseObjectPrefab.transform.GetChild(i).GetComponent<DefenseObject>().childNb = i;
		}
	}

	private void BuildDefenseObject(int _childNb)
	{
		m_defenseObjects[_childNb] = true;
	}
	
	private void SceneManagerOnactiveSceneChanged(Scene arg0, Scene arg1)
	{
		if (arg1.name == m_hubName)
		{
			m_defenseObjectPrefab = GameObject.FindGameObjectWithTag("DefenseObject");
		}
		
		if(m_defenseObjectPrefab == null)
			return;

		for (int i = 0; i < m_defenseObjectPrefab.transform.childCount; i++)
		{
			Transform child = m_defenseObjectPrefab.transform.GetChild(i);
			child.GetComponent<DefenseObject>().Rebuild(m_defenseObjects[i], i);
		}
	}
}
