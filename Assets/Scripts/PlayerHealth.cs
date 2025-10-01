using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    public float maxHealth = 100f;
    private float lerpTimer;
    public float chipSpeed = 2f;
    public TextMeshProUGUI healthText;

    public Image FrontHealthBar;
    public Image BackHealthBar;

    private float durationTimer;

    void Start()
    {
        health = maxHealth;
        
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        float fillFront = FrontHealthBar.fillAmount;
        float fillBack = BackHealthBar.fillAmount;
        float HealthFraction = health / maxHealth;
        healthText.text = health + "HP";
        
        
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
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;
        
    }
}
