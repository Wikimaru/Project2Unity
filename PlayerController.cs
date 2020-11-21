using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using SpriteTrail;

public class PlayerController : MonoBehaviour
{

    [Header("Playerstat")]
    public float moveSpeed;
    public Rigidbody2D theRB;
    public float currentDef;
    private Vector2 moveInput;
    private float trueDef;

    [Header("Dashstat")]
    private float activeMoveSpeed;
    private float timedash,activedashSpeed = 0;
    public float dashSpeed = 5, dashlengeth = .5f, dashCooldown = 1f, dashinvinibility = .5f,StaminaUseToDash;


    public float RunSpeed = 2,RunuseStamina;
    private bool Runactive;
    private float runstaminaCounter=0.5f,runSpeedActive;


    [HideInInspector]
    public bool lockmove = false,dashOn =false;
    private Vector2 lockDirection;


    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;

    [Header("PlayerInfo")]
    public int PlayerNo;
    public Transform Hand,body;
    public Animator anim;
    public SpriteRenderer bodySR;
    public GameObject TrailDash,TrailRun;
    public Camera thecam;
    public Collider2D enemy;
    public float AutoAimRange;
    public LayerMask whatIsEnemies;
    public GameObject weapon;
    public WeaponScripts weaponSC;


    void Start()
    {
        activeMoveSpeed = moveSpeed;
        currentDef = PlayerstateController.instance.BaseDef;
        reloadDefense();

    }

    void Update()
    {

        //-----------Player move controller-----------//
        if (!lockmove)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
        }
        else
        {
            moveInput = lockDirection;
        }

        theRB.velocity = moveInput * (activeMoveSpeed+activedashSpeed+runSpeedActive);
        //======MoveAnimation//
        if (moveInput != Vector2.zero)
        {
            anim.SetBool("move", true);
        }
        else
        {
            anim.SetBool("move", false);
        }
        //------------Player Dash controller----------//
        if (Input.GetKeyDown(KeyCode.Space)&& moveInput != Vector2.zero && dashCoolCounter <= 0 && PlayerstateController.instance.currentStamina >= StaminaUseToDash)
        {
            lockDirection.x = Input.GetAxisRaw("Horizontal");
            lockDirection.y = Input.GetAxisRaw("Vertical");
            lockmove = true;
            activedashSpeed = dashSpeed;
            anim.SetTrigger("Dash");
            timedash = dashlengeth;
            dashOn = true;
            dashCoolCounter = dashCooldown;
            PlayerstateController.instance.useStamina(StaminaUseToDash);
            useTrailDash(true);
            Run();

        }
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;

            if (timedash > 0)
            {
                timedash -= Time.deltaTime;
            }
            else if (dashOn)
            {
                lockmove = false;
                dashOn = false;
                activedashSpeed = 0;
                useTrailDash(false);
            }
        }
        //------------Run Controller-------------------//
        if (Runactive)
        {
            runstaminaCounter -= Time.deltaTime;
            if(runstaminaCounter <= 0)
            {
                runstaminaCounter = 0.5f;
                PlayerstateController.instance.useStamina(RunuseStamina);
            }


            if(moveInput == Vector2.zero|| PlayerstateController.instance.currentStamina <=0)
            {
                Runactive = false;
                runSpeedActive = 0;
                useTrailRun(false);
            }
            
        }
        //------------Hand Controller------------------//
        Collider2D[] enemiesToAim = Physics2D.OverlapCircleAll(body.position, AutoAimRange, whatIsEnemies);
        float distanceToClosestEnemy = Mathf.Infinity ;
        Collider2D closetEnemy = null;
        for (int i = 0; i < enemiesToAim.Length; i++)
        {
            float distanceToEnemy = (enemiesToAim[i].transform.position - this.transform.position).sqrMagnitude;
            if(distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closetEnemy = enemiesToAim[i];
            }
        }
        enemy = closetEnemy;

        if (enemiesToAim.Length > 0)
        {
            Vector3 direction = enemy.transform.position - Hand.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Hand.rotation = rotation;
            //Hand.rotation = Quaternion.RotateTowards(Hand.rotation,rotation, Time.deltaTime * 100);
            if (Hand.eulerAngles.z > 90 && Hand.eulerAngles.z < 270)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                Hand.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                Hand.localScale = Vector3.one;
            }
        }
        /*Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = thecam.WorldToScreenPoint(transform.localPosition);
        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            Hand.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            Hand.localScale = Vector3.one;
        }
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        Hand.rotation = Quaternion.Euler(0, 0, angle);*/

        //------------Attack Controller----------------//
        if (Input.GetMouseButtonDown(0))
        {
            if(PlayerstateController.instance.currentStamina >= weapon.GetComponent<WeaponScripts>().usestamina && weapon.GetComponent<WeaponScripts>().AttackCoolDownCounter <= 0)
            {
            weapon.GetComponent<WeaponScripts>().Attack();
            PlayerstateController.instance.useStamina(weapon.GetComponent<WeaponScripts>().usestamina);
            }
            
        }

        //------------Test Systemzone------------------//
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayerstateController.instance.DamageToPlayer(50);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerstateController.instance.HealToPlayer(10);
        }



    }
    public void useTrailDash(bool active)
    {
        TrailDash.SetActive(active);
    }
    public void Run()
    {
        Runactive = true;
        runSpeedActive = RunSpeed;
        useTrailRun(true);
    }
    public void DamageToPlayer(float Damage)
    {
        if(timedash <= 0)
        {
        float trueDamage = Damage * ((100 - trueDef) / 100);
        PlayerstateController.instance.DamageToPlayer(trueDamage);
        }

    }
    public void useTrailRun(bool active)
    {
        TrailRun.SetActive(active);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(body.position, AutoAimRange);
    }
    public void reloadDefense()
    {
        if(currentDef <= 10)
        {
            trueDef = currentDef;
        }
        else if(currentDef <= 70)
        {
            trueDef = 10 + ((currentDef - 10) / 2);
        }
        else
        {
            trueDef = 40 + ((currentDef - 70) / 4);
            if(trueDef > 95)
            {
                trueDef = 95;
            }
        }
    }
}
