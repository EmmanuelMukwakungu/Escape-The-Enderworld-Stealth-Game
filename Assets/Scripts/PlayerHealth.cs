using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    public float maxHealth = 100f;
    private float lerpTimer;
    public float chipSpeed = 2f;
    public TextMeshProUGUI healthText;
    private float durationTimer;
    
    public Image FrontHealthBar;
    public Image BackHealthBar;

    private bool isDead = false;

    public GameObject losePanel;
    public GameObject winPanel;
    
    private Animator animator;

    void Start()
    {
        health = maxHealth;
        animator = GetComponent<Animator>();
        
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamge(Random.Range(5,10));
        }
        
        
    }

    public void UpdateHealthUI()
    {
        float fillFront = FrontHealthBar.fillAmount;
        float fillBack = BackHealthBar.fillAmount;
        float HealthFraction = health / maxHealth;
        healthText.text = health + " HP";
        
        
        if (fillBack > HealthFraction)
        {
            FrontHealthBar.fillAmount = HealthFraction;
            BackHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentage = lerpTimer / chipSpeed;
            BackHealthBar.fillAmount = Mathf.Lerp(fillBack, HealthFraction, percentage);
        }
        else if (fillFront < HealthFraction)
        {
            BackHealthBar.fillAmount = HealthFraction;
            BackHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentage = lerpTimer / chipSpeed;
            FrontHealthBar.fillAmount = Mathf.Lerp(fillFront, HealthFraction, percentage);
        }
    }

    public void TakeDamge(float damage)
    {
        if (isDead) return;
        
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        
        if (health <= 0)
        {
            health = 0;
            Die();
            losePanel.SetActive(true);
        }

    }
    
    public void RestoreHealth(float healAmount)
    {
        if (health < maxHealth)
        {
            health += healAmount;
            health = Mathf.Clamp(health, 0, maxHealth);
            lerpTimer = 0f;
        }

    }

    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        
        GetComponent<PlayerLocomotion>().enabled = false;
        GetComponent<InputManager>().enabled = false;
        
    }


}
