using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public enum NavState
    {
        Stop,
        Patrol,
        Tracking
    }
    private enum MonsterType
    {
        Slime,
        Shell
    }

    // Enemy Base Components
    [SerializeField] MonsterType m_MonsterType;
    private NavMeshAgent m_Nav;
    private Animator m_Animator;
    private NavState navState;

    // Patrol Mode Components
    [SerializeField] private float patrolSpeed;
    public GameObject patrolPackage;
    private PatrolPoints[] patrolSites;
    private int patrolIndex;

    // Track Mode Components
    [SerializeField] private float trackSpeed;
    private Transform targetTransform;

    // Attack Mode Components
    [SerializeField] private GameObject AttackArea;
    [SerializeField] private int attackMax;
    public AudioClip AttackClip;
    private float attackEnableTime = 1.5f;
    private float attackTimer = 0f;

    // Enemy Death Drop Coin Components
    [SerializeField] private Item[] item;

    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_Controller = GetComponent<CharacterController>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        AttackArea.SetActive(false);
        navState = NavState.Patrol;
        patrolSites = patrolPackage.GetComponentsInChildren<PatrolPoints>();
        patrolIndex = 0;
        m_Nav.SetDestination(patrolSites[patrolIndex].transform.position);
        switch (m_MonsterType)
        {
            case MonsterType.Slime:
                SetCharacterInfo("Slime", 50, 5, 5, 1);
                break;
            case MonsterType.Shell:
                SetCharacterInfo("Shell", 80, 10, 10, 5);
                break;
        }
        StartCoroutine(EnemyActionRoutine());
    }
    void Update()
    {
        attackTimer += Time.deltaTime;
        CheckDeath();
        OnDamaged();
    }

    private void OnDisable()
    {
        if (item.Length > 0)
        {
            int index = Random.Range(0, item.Length);
            Vector3 pos = transform.position + new Vector3(0, 1.5f, 0f);
            GameObject dropItem = Instantiate(this.item[index].gameObject, pos, Quaternion.identity);
            dropItem.transform.parent = null;
        }
    }

    public void AttackStart()
    {
        AttackArea.SetActive(true);
        m_Audio.PlayOneShot(AttackClip);
    }

    public void AttackEnd()
    {
        AttackArea.SetActive(false);
        m_State = CharacterState.Idle;
        navState = NavState.Patrol;
    }

    protected override void CheckDeath()
    {
        base.CheckDeath();
        if (m_State == CharacterState.Death)
        {
            m_Animator.SetTrigger("Die");
            Destroy(gameObject, 3f);
            this.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerAttack" && m_State!=CharacterState.Damaged)
        {
            PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
            navState = NavState.Stop;
            if (player.m_State == Character.CharacterState.Attack) {
                TakeDamage(player.transform, player.GetInfo().AttackMin, player.GetInfo().AttackMax);
                targetTransform = player.transform;
            }
            else if(player.m_State == Character.CharacterState.Skill)
            {
                TakeDamage(other.transform, player.GetInfo().AttackMin * 2, player.GetInfo().AttackMax * 2);
                targetTransform = player.transform;
            }

        }
    }

    public void DamageEnd()
    {
        m_State = CharacterState.Idle;
        if (targetTransform != null)
            navState = NavState.Tracking;
        else
            navState = NavState.Patrol;
    }

    private void OnDamaged()
    {
        if(m_State == CharacterState.Damaged)
            AttackArea.SetActive(false);
    }

    protected override void TakeDamage(Transform damageLoc, int damageMin, int damageMax)
    {
        base.TakeDamage(damageLoc, damageMin, damageMax);
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        if(cameraController!=null)
        {
            cameraController.ShakeEffect();
        }
    }
    private IEnumerator EnemyActionRoutine()
    {
        while (m_State != CharacterState.Death)
        {
            switch (navState)
            {
                case NavState.Stop:
                    m_Nav.enabled = false;
                    if(m_State==CharacterState.Attack)
                    {
                        if (attackTimer >= attackEnableTime)
                        {
                            int index = Random.Range(0, attackMax);
                            transform.LookAt(targetTransform);
                            m_Animator.SetInteger("AttackIndex", index);
                            m_Animator.SetTrigger("DoAttack");
                            attackTimer = 0;
                        }
                    }
                    if (Vector3.Distance(transform.position, targetTransform.position) > 1.5f)
                    {
                        navState = NavState.Tracking;
                        m_State = CharacterState.Idle;
                    }
                    else if (Vector3.Distance(transform.position, targetTransform.position) > 5f)
                    {
                        navState = NavState.Patrol;
                        m_State = CharacterState.Idle;
                    }
                    break;

                case NavState.Patrol:
                    m_Nav.enabled = true;
                    m_Nav.speed = patrolSpeed;
                    if (Vector3.Distance(transform.position, patrolSites[patrolIndex].transform.position) <= 1f)
                    {
                        patrolIndex++;
                        if (patrolIndex >= patrolSites.Length)
                            patrolIndex = 0;
                    }
                    m_Nav.SetDestination(patrolSites[patrolIndex].transform.position);
                    m_Animator.SetFloat("speed", 0.5f);
                    
                    // Check Player Access
                    Collider[] targetDetect = Physics.OverlapSphere(transform.position, 5f, LayerMask.GetMask("Player"));
                    foreach (Collider hit in targetDetect)
                    {
                        targetTransform = hit.transform;
                        navState = NavState.Tracking;
                        break;
                    }
                    break;
                case NavState.Tracking:
                    m_Nav.enabled = true;
                    m_State = CharacterState.Idle;
                    m_Nav.speed = trackSpeed;
                    m_Animator.SetFloat("speed", 1f);
                    m_Nav.SetDestination(targetTransform.position);
                    
                    if (Vector3.Distance(transform.position, targetTransform.position) <= 1.5f)
                    {
                        navState = NavState.Stop;
                        m_State = CharacterState.Attack;
                    }
                    else if(Vector3.Distance(transform.position, targetTransform.position) > 5f)
                    {
                        navState = NavState.Patrol;
                        m_State = CharacterState.Idle;
                    }
                    break;
            }
            yield return null;
        }
    }

}
