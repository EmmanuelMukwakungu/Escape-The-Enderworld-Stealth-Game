using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private PlayerHealth playerHP;
    private EnemyAi enemy;
    public void OnCollisionEnter(Collision other)
    {
       // _Projectile = other.gameObject;
       if (other.gameObject.CompareTag("Player"))
       {
           playerHP = other.gameObject.GetComponent<PlayerHealth>();
           playerHP.TakeDamge(25);
       }

      
    }
}
