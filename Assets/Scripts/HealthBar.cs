using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthSlider; 
    public Transform enemy;
    void Update()
    {
        if(enemy != null)
        {
            transform.position = enemy.position + new Vector3(0,2.0f,0); 
        }
    }
    public void SetHealth(float health , float maxHealth)
    {
        if(healthSlider !=null)
        {
            healthSlider.value = health / maxHealth ; 
        }
    }
}
