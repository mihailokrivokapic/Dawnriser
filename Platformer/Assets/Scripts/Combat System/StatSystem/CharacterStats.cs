using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    // This class is responsible for handling character stats
    
    public float maxHealth;
    public float maxMana;

    public float currentHealth { get; private set; }
    public float currentMana { get; private set; }
    public float healthRegen { get; private set; }
    public float manaRegen { get; private set; }

    public Stat armor { get; private set; }
    private Stat magicResist;
    private Stat level;
    private Stat damageReduction;
    private Stat attackDamage;
    private Stat abilityPower;

    // Reference values
    public int coins { get; set; }
    public int armorText;
    public int attackDamageText;
    public int abilityPowerText;



    private void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        armor = new Stat();
        armor.baseValue = 8;
        coins = 0;
        Debug.Log(currentHealth);

        // Set the values
        // armorText = armor.baseValue;
        // attackDamageText = attackDamage.baseValue;
        // abilityPowerText = abilityPower.baseValue;

    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            Damage(10);
            Debug.Log(currentHealth);
        }
    }

    public virtual void Damage(float amount)
    {
        // Invoke the OnDamageTaken function
        EventManager.instance.OnDamageTaken(amount);

        amount -= armor.baseValue;
        currentHealth -= amount;
        if(currentHealth < 0)
        {
            // Invoke the OnDeath function
            Destroy(this);
            EventManager.instance.OnDeath();
        }
    }
    
    public void Heal(float amount)
    {
        // Invoke event
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }


}
