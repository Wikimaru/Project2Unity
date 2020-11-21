using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{

    public float HP, MaxHP, physicalDef, magicDef, Speed, currentHP,cuurentEnergy,MaxEnergy,EnemyDamage;
    private float currentPhysicalDef, currentMagicDef;
    public Transform HPbar;
    public Transform EnergyBar;
    public SpriteRenderer Body;
    private Vector3 HPBarvalue,EnergyBarValue;
    public bool isKnockBack = false;
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    private float effectcounter;
    private bool iseffect = false;


    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        currentPhysicalDef = physicalDef;
        currentMagicDef = magicDef;
        HPBarvalue = HPbar.localScale;
        EnergyBarValue = EnergyBar.localScale;
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Sprites/Default");

    }

    // Update is called once per frame
    void Update()
    {
        if(effectcounter > 0)
        {
            effectcounter -= Time.deltaTime;
        }
        else if (iseffect)
        {
            iseffect = false;
            normalSprite();
        }
    }

    public void DamagePhysical(float Damage)
    {
        whiteSprite();
        if (Damage <= physicalDef)
        {
            currentHP -= 1;
        }
        else
        {
            currentHP -= (Damage - physicalDef);
        }
        HPBarvalue.x = (currentHP / MaxHP);
        HPbar.transform.localScale = HPBarvalue;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }


    }
    public void DamageMagic(float Damage)
    {

    }
    private void whiteSprite()
    {
        Body.material.shader = shaderGUItext;
        Body.color = Color.white;
        effectcounter = 0.1f;
        iseffect = true;
    }
    private void normalSprite()
    {
        Body.material.shader = shaderSpritesDefault;
        Body.color = Color.white;
    }
}

