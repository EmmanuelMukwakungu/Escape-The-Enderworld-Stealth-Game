using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAi : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private NavMeshAgent _agent;
  [SerializeField] private Transform _PlayerTransform;
  [SerializeField] private Transform _FireTransform;
  [SerializeField] private GameObject _Projectile;

  [Header("Layers")]
  [SerializeField] private LayerMask _terrainLayer;
  [SerializeField] private LayerMask _playerLayerMask;

  [Header("Patrol Settings")]
  [SerializeField] private float _patrolRadius = 10f;
  Vector3 _currentPatrolPosition;
  private bool hasPatrolPoint;

  [Header("Attack Settings")]
  [SerializeField] private float attackCoolDown = 1f;
  private bool isOnAttackCooldown;
  [SerializeField] private float forwardShotForce = 10f;
  [SerializeField] private float verticalShotForce = 5f;
   
  [Header("Detection Settings")]
  [SerializeField] float visionRange = 200f;
  [SerializeField] float engagementRange = 10f;

  private bool isPlayerVisible;
  private bool isPlayerInRange;

  private void Awake()
  {
    if (_PlayerTransform == null)
    {
      GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
      if (playerObj != null)
      {
       _PlayerTransform = playerObj.transform;
      }
    }

    if (_agent == null)
    {
      _agent = GetComponent<NavMeshAgent>();
    }
  }

  private void Update()
  {
    PlayerDecteted();
    UpdateEnemyBehaviour();
  }

  private void FireProjectile()
  {
    if(_Projectile == null || _FireTransform == null) return;
    
    Rigidbody projectileRB = Instantiate(_Projectile, _FireTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
    projectileRB.AddForce(transform.forward * forwardShotForce, ForceMode.Impulse);
    //projectileRB.AddForce(_FireTransform.forward * forwardShotForce, ForceMode.Impulse);
    projectileRB.AddTorque(transform.up * verticalShotForce, ForceMode.Impulse);
    
    Destroy(projectileRB.gameObject, 3f);
  }
  
  private void PerformPatrol()
  {
    if (!hasPatrolPoint)
    {
      FindPatrolPoint();
    }

    if (hasPatrolPoint)
    {
      _agent.SetDestination(_currentPatrolPosition);
    }

    if (Vector3.Distance(transform.position, _currentPatrolPosition) < 1f)
    {
      hasPatrolPoint = false;
    }
  }

  private void PerformChase()
  {
    if (_PlayerTransform != null)
    {
      _agent.SetDestination(_PlayerTransform.position);
    }
  }

  private void PerformAttack()
  {
    _agent.SetDestination(transform.position);

    if (_PlayerTransform != null)
    {
      transform.LookAt(_PlayerTransform);
    }

    if (!isOnAttackCooldown)
    {
      FireProjectile();
      StartCoroutine(AttackCooldownRoutine());
    }
  }

  private void UpdateEnemyBehaviour()
  {
    if (!isPlayerVisible && !isPlayerInRange)
    {
      PerformPatrol();
    }
    else if (isPlayerVisible && !isPlayerInRange)
    {
      PerformChase();
    }
    else if (isPlayerVisible && isPlayerInRange)
    {
      PerformAttack();
    }
  }

  private IEnumerator AttackCooldownRoutine()
  {
    isOnAttackCooldown = true;
    yield return new WaitForSeconds(attackCoolDown);
    isOnAttackCooldown = false;
  }

  private void FindPatrolPoint()
  {
    float randomX = Random.Range(-_patrolRadius, _patrolRadius);
    float randomZ = Random.Range(-_patrolRadius, _patrolRadius);
    
    Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    if (Physics.Raycast(potentialPoint, -transform.up, 2f, _terrainLayer))
    {
      _currentPatrolPosition = potentialPoint;
      hasPatrolPoint = true;
    }
  }


  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, engagementRange); //Engagement Range
    
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, visionRange);// Vision Range
  }

  private void PlayerDecteted()
  {
    isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, _playerLayerMask);
    isPlayerInRange= Physics.CheckSphere(transform.position, engagementRange, _playerLayerMask);
  }
}
