using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject DetectZone,AwakeZone,Hand,AttackIcon;
    public Transform DashPoint, BackDashPoint ,AttackPos;
    public int patterns = 2;  
    public EnemyState State;
    public Rigidbody2D TheRB;
    public Animator weaponAnim;
    public float AttackCoolDown,DashSpeed,Attack2Range;
    public ParticleSystem AttackSFX;
    public LayerMask whatIsPlayer;

    private bool hasTaget = false,CanAttack = false ,Awake = false,UpdateAttack1 = false,UpdateAttack2 = false,lockmove = true;
    private bool Attack2Phase1 = false,Attack2Phase2 = false;
    private Vector3 Direction, lockDirection;
    private float WillATK,AttackCoolDownCounter,Speed,DashCouter,wonderPhase,WonderPhaseCounter = 0;
    private Vector3 DashDirection;
 
    public Collider2D Player;
    public bool KnockBackNow = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Speed = State.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(WonderPhaseCounter <= 0)
        {
            WonderPhaseCounter = Random.Range(0.5f, 1.5f);
            Direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1, 1f),0f);
        }

        if (hasTaget)
        {
            Vector3 handDirection = Player.transform.position - Hand.transform.position;
            float angle = Mathf.Atan2(handDirection.y, handDirection.x) * Mathf.Rad2Deg;
            Quaternion HandRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Hand.transform.rotation = HandRotation;
            if (Hand.transform.eulerAngles.z > 90 && Hand.transform.eulerAngles.z < 270)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                Hand.transform.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = Vector3.one;
                Hand.transform.localScale = Vector3.one;
            }
        }

        if (hasTaget && CanAttack && Awake && AttackCoolDownCounter <= 0)
        {
            CanAttack = false;
            int AttackPT = Random.Range(1,patterns+1);
            Debug.Log(AttackPT);
            switch (AttackPT)
            {
                case 1:
                    Attack1();
                    break;
                case 2:
                    Attack2();
                    break;

            }
        }


        //------ Attack Update ------//
        if (UpdateAttack1)
        {
            if(Vector2.Distance(transform.position,Player.transform.position) > 1 && !State.isKnockBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, (Speed+3) * Time.deltaTime);
            }
            if((Vector2.Distance(transform.position,Player.transform.position) <= 1.5) && !Attack2Phase1)
            {
                Attack2Phase1 = true;
                WillATK = 0.5f;
                AttackIcon.SetActive(true);
            }
            if(Attack2Phase1 && WillATK <= 0 && !Attack2Phase2)
            {
                Attack2Phase2 = true;
                AttackIcon.SetActive(false);
                UpdateAttack1 = false;
                weaponAnim.SetTrigger("Attack");
                AttackSFX.Play();
                Collider2D[] PlayerToDamage = Physics2D.OverlapCircleAll(AttackPos.position, Attack2Range, whatIsPlayer);
                for (int i = 0; i < PlayerToDamage.Length; i++)
                {
                    PlayerToDamage[i].GetComponent<PlayerController>().DamageToPlayer(State.EnemyDamage);
                }
                AttackComplete();
            }
        }
        if (UpdateAttack2)
        {
            if (Vector2.Distance(transform.position, Player.transform.position) > 4.5 && !Attack2Phase1 && !State.isKnockBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
            }
            else if(!Attack2Phase1)
            {
                
                WillATK = 0.3f;
                AttackIcon.SetActive(true);
                Attack2Phase1 = true;
                DashDirection = DashPoint.position;
            }
            if(Attack2Phase1 && WillATK <= 0 && !Attack2Phase2)
            {
                DashCouter = 0.3f;
                AttackIcon.SetActive(false);
                Attack2Phase2 = true;
            }
            if (Attack2Phase2 && DashCouter > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, DashDirection, DashSpeed * Time.deltaTime);
            }
            else if (Attack2Phase2)
            {
                UpdateAttack2 = false;
                weaponAnim.SetTrigger("Attack");
                AttackSFX.Play();
                Collider2D[] PlayerToDamage = Physics2D.OverlapCircleAll(AttackPos.position, Attack2Range, whatIsPlayer);
                for (int i = 0; i < PlayerToDamage.Length; i++)
                {
                    PlayerToDamage[i].GetComponent<PlayerController>().DamageToPlayer(State.EnemyDamage);
                }
                AttackComplete();
            }
        }
        //------- Move Update -------//
        if (!lockmove && !State.isKnockBack)
        {
            Direction.Normalize();
            TheRB.velocity = Direction * Speed;
        }
        else if(!State.isKnockBack)
        {
            Direction = Vector3.zero;
            TheRB.velocity = Direction * Speed;
        }



    }
    private void FixedUpdate()
    {
       //------- Counter Zone -------// 
       if(DashCouter > 0)
        {
            DashCouter -= Time.deltaTime;
        }
       if(WillATK > 0)
        {
            WillATK -= Time.deltaTime;
        }
       if (AttackCoolDownCounter > 0)
       {
            AttackCoolDownCounter -= Time.deltaTime;
       }
       if(WonderPhaseCounter > 0)
        {
            WonderPhaseCounter -= Time.deltaTime;
        }
    }

    public void GetTaget(Collider2D Taget)
    {
        Player = Taget;
        hasTaget = true;
        CanAttack = true;
        DetectZone.SetActive(false);
    }
    public void AwakeEnemy()
    {
        Awake = true;
        AwakeZone.SetActive(false);
        DetectZone.SetActive(true);
    }

    private void Attack1()
    {
        lockmove = true;
        UpdateAttack1 = true;
    }
    private void Attack2()
    {
        lockmove = true;
        UpdateAttack2 = true;
    }
    private void AttackSlash()
    {

    }
    private void AttackComplete()
    {
        //------Attack Complete-------//
        hasTaget = false;
        AttackCoolDownCounter = AttackCoolDown;
        DetectZone.SetActive(true);
        Attack2Phase1 = false;
        Attack2Phase2 = false;
        lockmove = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, Attack2Range);
    }
}
