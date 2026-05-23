using System.Collections;
using System.Collections.Generic;
using EANasir.Interface;
using Event;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

namespace EANasir.Guard {
    public class GuardController : MonoBehaviour, IAttackable {
        private NavMeshAgent m_agent;
        private Coroutine m_lostCoroutine = null;
        [SerializeField] private List<Vector3> m_patrolPoints = new();  // TODO : Replace with GameObject instead of Vector3
        [SerializeField] private Transform m_playerPos;

        [SerializeField] private float m_recognitionDistance = 5;
        [SerializeField] private float m_recognitionAngle = 15;
        [SerializeField] private float m_soundDistance = 10;
        [SerializeField] private float m_lossOfSight = 0;

        private int m_nextPatrolPoint = 0;
        private bool m_followingPlayer = false;

        /// </summary>
        /// Init NavMeshAgent, player attack, patrol points & the guard line of sight
        /// </summary>
        private void Awake() {
            EventManager.AddListener<Vector3>("PlayerAttack", PlayerAttack);
            m_agent = GetComponent<NavMeshAgent>();

            // TODO Remove
            LineRenderer lr = GetComponent<LineRenderer>();
            for ( int i = 0; i < lr.positionCount; i++ ) {
                var pos = lr.GetPosition(i);
                pos.z = 0;
                m_patrolPoints.Add(pos);
            }
            Destroy(lr);

            Light2D light = GetComponentInChildren<Light2D>();
            light.pointLightOuterRadius = m_recognitionDistance;
            light.pointLightOuterAngle = m_recognitionAngle;
            GoToNextPatrolPoint();
        }

        /// </summary>
        /// Check if the player is in the los (line of sight) else patrol
        /// CheckForPlayer must be <<<< compared to m_agent.remainingDistance <= m_agent.stoppingDistance
        /// </summary>
        private void Update() {
            // If we see the player or we are chasing it
            if ( CheckForPlayer() || m_followingPlayer ) {  
                m_followingPlayer = true;

                // If we are in a coroutine, stop it
                if ( m_lostCoroutine != null ) {    
                    StopCoroutine(nameof(LostPlayerRoutine));
                    m_lostCoroutine = null;
                }

                // Follow player
                Trigger(m_playerPos.position);  
            }

            // If we are following the player
            if ( m_followingPlayer ) {  
                float agentPlayerDist = Vector3.Distance( transform.position, m_playerPos.position );

                // Check that it is not out of bounds (TODO : refacto with a timer maybe ?)
                if ( agentPlayerDist > m_lossOfSight ) {   
                    m_followingPlayer = false;
                }
            }

            // If we reach our waypoint & we are not following the player
            if ( !m_followingPlayer && m_agent.remainingDistance <= m_agent.stoppingDistance ) {
                if ( m_lostCoroutine == null ) {
                    m_lostCoroutine = StartCoroutine(nameof(LostPlayerRoutine));
                }
            }
        }

        /// </summary>
        /// Guard routine to attack the player if in bound
        /// </summary>
        private void PlayerAttack(Vector3 _target) {
            if ( Vector3.Distance(transform.position, _target) < m_soundDistance ) {
                Trigger(_target);
            }
        }

        /// </summary>
        /// Guard routine when it loose los (line of sight) of the player 
        /// </summary>
        private IEnumerator LostPlayerRoutine() {
            float nextAngle = 0f;
            Quaternion rotation = transform.rotation;
            Quaternion defaultRotation = transform.rotation;

            Quaternion endAngle = rotation * Quaternion.Euler(0f, 0f, 90f);
            while ( nextAngle < 0.75f ) {
                transform.rotation = Quaternion.Lerp(rotation, endAngle, nextAngle / 0.75f);
                nextAngle += Time.deltaTime;
                yield return null;
            }

            nextAngle = 0f;
            rotation = transform.rotation;
            endAngle = rotation * Quaternion.Euler(0f, 0f, 180f);
            while ( nextAngle < 1.5f ) {
                transform.rotation = Quaternion.Lerp(rotation, endAngle, nextAngle / 1.5f);
                nextAngle += Time.deltaTime;
                yield return null;
            }
            
            // TODO : Need to do this with a Lerp for smoother transition
            // We may have a bug with navmesh but easily solvable by forcing agent to face next waypoint - emartinez
            transform.rotation = defaultRotation;
            m_lostCoroutine = null; // Coroutine manage its life cycle

            // Go to next point
            GoToNextPatrolPoint();
        }

        /// </summary>
        /// Guard patroling routine
        /// Each time it reach a waypoint it check for the next one
        /// </summary>
        private void GoToNextPatrolPoint() {
            if ( m_agent.SetDestination( m_patrolPoints[ m_nextPatrolPoint ] ) ) {
                m_nextPatrolPoint = ( m_nextPatrolPoint + 1 ) % m_patrolPoints.Count;
            } else {
                Debug.Log( "Error during next patrol point choosen" );
            }
        }

        /// <summary>
        /// Check if the player is in the "light" 
        /// </summary>
        private bool CheckForPlayer() {
            float angle = Mathf.Acos(Vector3.Dot(-Vector3.Normalize(transform.up),
            Vector3.Normalize(transform.position - m_playerPos.position)));
            angle = angle * 180 / Mathf.PI;
            if ( angle < m_recognitionAngle ) {
                if ( Vector3.Distance(transform.position, m_playerPos.position) < m_recognitionDistance ) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Trigger the "see the player comportement"
        /// </summary>
        public void Trigger(Vector2 _triggerPos) {
            m_agent.SetDestination(new(_triggerPos.x, _triggerPos.y, transform.position.z));
        }

        public void IsAttacked(float damage) {
            Debug.Log("Non aled on m'attaque");
        }
    }

}