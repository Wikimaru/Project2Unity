using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerstateController : MonoBehaviour
{
    public static PlayerstateController instance;

    public float maxHP,currentHP;

    public float maxMP, currentMP;

    public float maxStamina, currentStamina,reStaminaDelay,reStaminaSpeed;

    public float BaseDef;
    private float reStaminaCounter,StaminaCounter;

    public GameObject Body,Trail;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UImanager.instance.staminaBar.maxValue = maxStamina;
        UImanager.instance.staminaBar.value = currentStamina;
        UImanager.instance.HPBar.SetValue(currentHP,maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if(reStaminaCounter <= 0&& currentStamina != maxStamina && currentStamina < maxStamina)
        {
            
            StaminaCounter -= Time.deltaTime;
            if(StaminaCounter <= 0)
            {
                currentStamina += reStaminaSpeed;
                StaminaCounter = 0.25f;
                UImanager.instance.currentStamina = currentStamina;
            }
            if(currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }
        else if(reStaminaCounter >0)
        {
            reStaminaCounter -= Time.deltaTime;
        }
    }

    public void useStamina(float StaminaUse)
    {
        currentStamina -= StaminaUse;
        reStaminaCounter = reStaminaDelay;
        UImanager.instance.currentStamina = currentStamina;
    }
    public void DamageToPlayer(float Damage)
    {
        currentHP -= Damage;
        UImanager.instance.HPBar.SetValue(currentHP, maxHP);
    }
    public void HealToPlayer(float Heal)
    {
        currentHP += Heal;
        if(currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
        UImanager.instance.HPBar.SetValue(currentHP, maxHP);
    }

}
