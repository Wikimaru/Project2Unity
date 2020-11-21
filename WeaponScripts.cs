using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScripts : MonoBehaviour
{
    public float PhysicalDamage,MagicDamage;
    public Animator anim;
    public float resetToIdle,AttackCoolDown,canDamageTime;
    public bool idle,CanDamage;
    public int numOfAttack = 2;
    private int attackcount;
    private float resetToIdleCounter,canDamageCounter;
    public float usestamina, AttackCoolDownCounter;
    public ParticleSystem Attack1, Attack2;
    public Rigidbody2D theRB;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public float attackRange;

    public float knockbackPower,knockbackTime;


    // Start is called before the first frame update
    void Start()
    {
        idle = true;
        anim.SetBool("idle", true);
        attackcount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(AttackCoolDownCounter > 0)
        {
            AttackCoolDownCounter -= Time.deltaTime;
        }
        if(resetToIdleCounter > 0)
        {
            resetToIdleCounter -= Time.deltaTime;
        }
        if (!idle && resetToIdleCounter <=0)
        {
            idle = true;
            anim.SetBool("idle", true);
            attackcount = 1;
        }
        if(canDamageCounter > 0)
        {
            canDamageCounter -= Time.deltaTime;
            if(canDamageCounter <= 0)
            {
                CanDamage = false;
            }
        }
    }

    public void Attack()
    {
        if(AttackCoolDownCounter <= 0)
        {
            if (attackcount == 1)
            {
                anim.SetTrigger("Attack1");
                if (numOfAttack == 1)
                {
                    attackcount = 1;
                }
                else
                {
                    attackcount = 2;
                }
                idle = false;
                CanDamage = true;
                canDamageCounter = canDamageTime;
                anim.SetBool("idle", false);
                AttackCoolDownCounter = AttackCoolDown;
                resetToIdleCounter = resetToIdle;
                Attack1.Play();
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange , whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {

                    enemiesToDamage[i].GetComponent<EnemyState>().DamagePhysical(PhysicalDamage);
                    if (enemiesToDamage[i].GetComponent<EnemyState>().currentHP > 0)
                    {
                        KnockBack(enemiesToDamage[i]);
                    }

                }
            }
            else if (attackcount == 2)
            {
                anim.SetTrigger("Attack2");
                if (numOfAttack == 2)
                {
                    attackcount = 1;
                }
                else
                {
                    attackcount = 3;
                }
                idle = false;
                CanDamage = true;
                canDamageCounter = canDamageTime;
                anim.SetBool("idle", false);
                AttackCoolDownCounter = AttackCoolDown;
                resetToIdleCounter = resetToIdle;
                Attack2.Play();
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {

                    enemiesToDamage[i].GetComponent<EnemyState>().DamagePhysical(PhysicalDamage);
                    if (enemiesToDamage[i].GetComponent<EnemyState>().currentHP > 0)
                    {
                        KnockBack(enemiesToDamage[i]);
                    }
                }
            }
            else if (attackcount == 3)
            {
                anim.SetTrigger("Attack3");
            }
        }
        
    }
    //========Other Functions=========//
    private void KnockBack(Collider2D enemiesToDamage)
    {
        Rigidbody2D enemyTheRB = enemiesToDamage.GetComponent<Rigidbody2D>();
        if(enemyTheRB != null)
        {
        enemiesToDamage.GetComponent<EnemyState>().isKnockBack = true;
        enemyTheRB.isKinematic = false;
        Vector2 difference = enemyTheRB.transform.position - transform.position;
        difference = difference.normalized * knockbackPower;
        enemyTheRB.AddForce(difference, ForceMode2D.Impulse);
        StartCoroutine(KnockBackCounter(enemyTheRB));
        
        }
    }
    //========Counter system==========//
    private IEnumerator KnockBackCounter(Rigidbody2D enemy)
    {
        if(enemy != null)
        {
            yield return new WaitForSeconds(knockbackTime);
            enemy.velocity = Vector2.zero;
            enemy.isKinematic = true;
            enemy.GetComponent<EnemyState>().isKnockBack = false;
        }
    }

    //========Editer Mode=============//
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
