using System.Collections;
using System.Collections.Generic;
using EANasir.Interface;
using Event;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace EANasir.Guard
{
	public class GuardController : MonoBehaviour, IAttackable
	{
		private NavMeshAgent m_agent;
		private Coroutine m_lostCoroutine;

		[SerializeField]
		private List<Vector3> m_patrolPoints = new();

		[SerializeField]
		private Transform m_playerPos;

		[SerializeField]
		private float m_recognitionDistance = 5;

		[SerializeField]
		private float m_recognitionAngle = 15;

		[SerializeField]
		private float m_soundDistance = 10;

		private bool m_agentMoving = false;
		private int m_nextPatrolPoint = 0;

		/// </summary>
		/// Init NavMeshAgent, player attack, patrol points & the guard line of sight
		/// </summary>
		private void Awake()
		{
			m_agent = GetComponent<NavMeshAgent>();

			EventManager.AddListener<Vector3>("PlayerAttack", PlayerAttack);

			LineRenderer lr = GetComponent<LineRenderer>();
			for (int i = 0; i < lr.positionCount; i++)
			{
				var pos = lr.GetPosition(i);
				pos.z = 0;
				m_patrolPoints.Add(pos);
			}

			Destroy(lr);

			Light2D light = GetComponentInChildren<Light2D>();
			light.pointLightOuterRadius = m_recognitionDistance;
			light.pointLightOuterAngle = m_recognitionAngle;
		}

		/// </summary>
		/// Start patroling routine
		/// </summary>
		private void Start()
		{
			GoToNextPatrolPoint();
		}

		/// </summary>
		/// Check if the player is in the los (line of sight) else patrol
		/// </summary>
		private void Update()
		{
			if (CheckForPlayer())
			{
				if (m_lostCoroutine != null)
					StopCoroutine(m_lostCoroutine);
				Trigger(m_playerPos.position);
			}

			if (m_agentMoving && m_agent.remainingDistance <= m_agent.stoppingDistance)
			{
				m_agentMoving = false;
				m_lostCoroutine = StartCoroutine("LostPlayerRoutine");
			}
		}

		/// <summary>
		/// Rotate the guard. Use the forward axis and rotate with angle.
		/// </summary>
		/// <param name="_angle">Angle in degrees</param>
		private void Rotate(float _angle = 90)
		{
			transform.Rotate(Vector3.forward, _angle);
		}

		/// </summary>
		/// Guard routine to attack the player if in bound
		/// </summary>
		private void PlayerAttack(Vector3 _target)
		{
			if (Vector3.Distance(transform.position, _target) < m_soundDistance)
			{
				Trigger(_target);
			}
		}

		/// </summary>
		/// Guard routine when it loose los (line of sight) of the player 
		/// </summary>
		private IEnumerator LostPlayerRoutine()
		{
			float nextAngle = 0f;
			Quaternion rotation = transform.rotation;

			Quaternion endAngle = rotation * Quaternion.Euler(0f, 0f, 90f);
			while (nextAngle < 0.75f)
			{
				transform.rotation = Quaternion.Lerp(rotation, endAngle, nextAngle / 0.75f);
				nextAngle += Time.deltaTime;
				yield return null;
			}

			nextAngle = 0f;
			rotation = transform.rotation;
			endAngle = rotation * Quaternion.Euler(0f, 0f, 180f);
			while (nextAngle < 1.5f)
			{
				transform.rotation = Quaternion.Lerp(rotation, endAngle, nextAngle / 1.5f);
				nextAngle += Time.deltaTime;
				yield return null;
			}

			// Go to next point
			GoToNextPatrolPoint();
		}

		/// </summary>
		/// Guard patroling routine
		/// </summary>
		private void GoToNextPatrolPoint()
		{
			m_agent.SetDestination(m_patrolPoints[m_nextPatrolPoint]);
			m_nextPatrolPoint = (m_nextPatrolPoint + 1) % m_patrolPoints.Count;
			m_agentMoving = true;
		}

		/// <summary>
		/// Check if the player is in the "light" 
		/// </summary>
		private bool CheckForPlayer()
		{
			float angle = Mathf.Acos(Vector3.Dot(-Vector3.Normalize(transform.up),
			Vector3.Normalize(transform.position - m_playerPos.position)));
			angle = angle * 180 / Mathf.PI;
			if (angle < m_recognitionAngle)
			{
				if (Vector3.Distance(transform.position, m_playerPos.position) < m_recognitionDistance)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Trigger the "see the player comportement"
		/// </summary>
		public void Trigger(Vector2 _triggerPos)
		{
			m_agent.SetDestination(new(_triggerPos.x, _triggerPos.y, transform.position.z));
			m_agentMoving = true;
		}

		public void IsAttacked(float damage)
		{
			Debug.Log("Non aled on m'attaque");
		}
	}

}