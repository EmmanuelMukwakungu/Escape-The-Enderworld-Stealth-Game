using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public float healAmount = 25f;
        public Vector3 rotationSpeed = new Vector3(0f, 50f, 0f);
        private void OnTriggerEnter(Collider other)
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RestoreHealth(healAmount);
                Destroy(gameObject);
            }
        }
        // Update is called once per frame
        void Update()
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
            //Mathf.PingPong(rotationSpeed.magnitude * Time.deltaTime, 1f);
        }
}
