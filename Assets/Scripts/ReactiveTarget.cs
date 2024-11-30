using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReactiveTarget : MonoBehaviour
{
    public GameObject healthBarPrefab; // Assign HealthBarCanvas prefab in the Inspector
    private GameObject healthBarInstance;
    private HealthBar healthBar;
    public float maxHealth = 100f;
    private float currentHealth;
    public void ReactToHit()
    {
        WanderingAI behavior = GetComponent<WanderingAI>();
        if (behavior != null)
        {
            behavior.SetAlive(false);
        }
        StartCoroutine(Die());
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //Instantiate health bar
        healthBarInstance = Instantiate(healthBarPrefab);

        // Assign HealthBar component
        healthBar = healthBarInstance.GetComponent<HealthBar>();

        // Parent and position
        healthBarInstance.transform.SetParent(transform);
        healthBarInstance.transform.localPosition = new Vector3(0, 2.0f, 0);
        healthBar.enemy = transform;
    }


    // Update is called once per frame
    void Update()
    {
        
        healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;  // Reduce health by the damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);  // resrticiton that the currenthealth goes from 0 to maxHealth

        if (currentHealth <= 0)
        {
            StartCoroutine(Die());  // Call Die coroutine if health reaches 0
        }
    }

    private IEnumerator Die()
    {
        Destroy(healthBarInstance);
        this.transform.Rotate(-75, 0, 0); // i could use some physics to have a downward force but i kind of like it that it flies away
        WanderingAI behavior = GetComponent<WanderingAI>();
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
