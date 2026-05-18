using EANasir.Interface;
using Event;
using UnityEngine;

namespace EANasir.Object
{
	public class QuestObject : MonoBehaviour, IInteractable
	{
		[SerializeField]
		private QuestObjectSO m_questObject;
		
		public float Interact()
		{
			EventManager.InvokeEvent("AddQuestObjectToInventory", m_questObject);
			Destroy(gameObject);
			return 0;
		}
	}
}
