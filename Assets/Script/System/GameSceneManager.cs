using Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EANasir.System
{
	public class GameSceneManager : MonoBehaviour
	{
		[SerializeField]
		private string m_hubName;
		
		private void Awake()
		{
			DontDestroyOnLoad(this);
			EventManager.AddListener("EndLevel", EndLevel);
			EventManager.AddListener<string>("StartLevel", StartLevel);
		}

		private void EndLevel()
		{
			StartLevel(m_hubName);
		}

		private void StartLevel(string levelName)
		{
			SceneManager.LoadScene(levelName);
		}
		
		
	}
}
