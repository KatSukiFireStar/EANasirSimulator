using System;
using UnityEngine;

public class CreateManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_HubManagerPrefab;

    [SerializeField]
    private GameObject m_SceneManagerPrefab;
    
    private void Awake()
    {
        if(!GameObject.FindGameObjectWithTag("HubManager"))
            Instantiate(m_HubManagerPrefab);
        
        if(!GameObject.FindGameObjectWithTag("GameSceneManager"))
            Instantiate(m_SceneManagerPrefab);
            
        Destroy(gameObject);
    }
}
