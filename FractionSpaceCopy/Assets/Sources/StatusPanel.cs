using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanel : MonoBehaviour
{
    public TMP_Text nameLabel;
  

    public Slider healthSlider;
    public Image healthSliderBar;
    public TMP_Text healthLabel;

    public void SetStats(string name, Stats stats)
    {
        this.nameLabel.text = name;

        
        this.SetHealth(stats.health, stats.maxHealth);
    }

    public void SetHealth(float health, float maxHealth)
    {
        this.healthLabel.text = $"{Mathf.RoundToInt(health)} / {Mathf.RoundToInt(maxHealth)}";
        float percentage = health / maxHealth;

        this.healthSlider.value = percentage;

        if (percentage < 0.33f)
        {
            this.healthSliderBar.color = Color.red;
        }
    }
}