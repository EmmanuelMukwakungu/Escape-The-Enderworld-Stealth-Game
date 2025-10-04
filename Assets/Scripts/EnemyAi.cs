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
  public PlayerHealth playerHP;

  [Header("Layers")]
  [SerializeField] private LayerMask _terrainLayer;
  [SerializeField] private LayerMask _obstructionleMask;
  [SerializeField] private LayerMask _playerLayerMask;

  [Header("Patrol Settings")]
  [SerializeField] private float _patrolRadius = 10f;
  Vector3 _currentPatrolPosition;
  private bool hasPatrolPoint;

  [Header("Attack Settings")]
  [SerializeField] private float attackCoolDown = 5f;
  private bool isOnAttackCooldown;
  [SerializeField] private float forwardShotForce = 10f;
  [SerializeField] private float verticalShotForce = 5f;
   
  [Header("Detection Settings")]
  [SerializeField] float visionRange = 200f;
  [Range(0, 360)]
  [SerializeField] float visionAngle = 90f;
  [SerializeField] float engagementRange = 10f;

  private bool isPlayerVisible;
  private bool isPlayerInRange;

  private void Awake()
  {
    playerHP = GetComponent<PlayerHealth>();
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
    Vector3 randomPoint = transform.position + new Vector3(Random.Range(- _patrolRadius, _patrolRadius), 0, 
      Random.Range(-_patrolRadius, _patrolRadius));

    if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 2f, NavMesh.AllAreas))
    {
      _currentPatrolPosition = hit.position;
      hasPatrolPoint = true;
    }
    /*float randomX = Random.Range(-_patrolRadius, _patrolRadius);
    float randomZ = Random.Range(-_patrolRadius, _patrolRadius);

    Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

    if (Physics.Raycast(potentialPoint, -transform.up, 2f, _terrainLayer))
    {
      _currentPatrolPosition = potentialPoint;
      hasPatrolPoint = true;
    }**/
  }


  private void OnDrawGizmosSelected()
  {
    if (_PlayerTransform == null) return;
    
      //Vision Cone 
      Gizmos.color = Color.yellow;
      Vector3 leftBoundary = Quaternion.Euler(0, -visionAngle / 3, 0) * transform.forward;
      Vector3 rightBoundary = Quaternion.Euler(0, visionAngle / 3, 0) * transform.forward;
      Gizmos.DrawRay(transform.position, leftBoundary * visionRange);
      Gizmos.DrawRay(transform.position, rightBoundary * visionRange);

      //Engagement Cone
      Gizmos.color = Color.red;
      Vector3 leftCorner = Quaternion.Euler(0, -visionAngle / 1, 0) * transform.forward;
      Vector3 rightCorner = Quaternion.Euler(0, visionAngle / 1, 0) * transform.forward;
      Gizmos.DrawRay(transform.position, leftBoundary * engagementRange);
      Gizmos.DrawRay(transform.position, rightBoundary * engagementRange);
      
      
    //Gizmos.DrawWireSphere(transform.position, engagementRange); //Engagement Range
    //Gizmos.DrawWireSphere(transform.position, visionRange);// Vision Range
  }

  //Check spheres to detect player
  private void PlayerDecteted()
  {
    if(_PlayerTransform == null) return;
    
    //Checking if player is in range
    float _distanceToPlayer = Vector3.Distance(transform.position, _PlayerTransform.position);
    bool _inRange = _distanceToPlayer <= visionRange;
    
    //Adding angle check
    Vector3 _directionToPlayer = (_PlayerTransform.position - transform.position).normalized;
    float _angleToPlayer = Vector3.Angle(transform.forward, _directionToPlayer);
    bool _inVisionCone = _angleToPlayer < visionAngle / 2f;

    bool hasLineOfSight = false;
    if (_inRange && _inVisionCone)
    {
      if (Physics.Raycast(transform.position + Vector3.up * 1.5f, _directionToPlayer, out RaycastHit hit, visionRange,
            ~_obstructionleMask))
      {
        if (hit.transform == _PlayerTransform)
        {
          hasLineOfSight = true;
        }
      }
    }
  
    isPlayerVisible = _inRange && _inVisionCone && hasLineOfSight;
   isPlayerInRange = isPlayerVisible && _distanceToPlayer <= engagementRange;




  }
  
}

