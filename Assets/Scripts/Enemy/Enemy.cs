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

    [SerializeField] private float trackSpeed;
    [SerializeField] MonsterType m_MonsterType;
    private NavMeshAgent m_Nav;
    private Animator m_Animator;
    private NavState navState;

    // Patrol Mode Components
    [SerializeField] private GameObject patrolPackages;
    private Transform[] patrolSites;
    private int patrolIndex;
    [SerializeField] private float patrolSpeed;
    private Vector3 patrolVec;
    private float moveVelocityY;

    // Track Mode Components
    private Transform targetTransform;
    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_Controller = GetComponent<CharacterController>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        navState = NavState.Patrol;
        patrolSites = patrolPackages.GetComponentsInChildren<Transform>();
        patrolIndex = 1;
        m_Nav.SetDestination(patrolSites[patrolIndex].position);
        switch (m_MonsterType)
        {
            case MonsterType.Slime:
                SetCharacterInfo("Slime", 50, 5, 5, 1);
                break;
            case MonsterType.Shell:
                SetCharacterInfo("Shell", 80, 10, 10, 5);
                break;
        }
        patrolVec = transform.position;
        StartCoroutine(EnemyActionRoutine());
    }
    void Update()
    {
        
       
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerAttack")
        {
            PlayerCharacter player = other.GetComponentInParent<PlayerCharacter>();
            if (player.m_State == Character.CharacterState.Attack) {
                TakeDamage(player.transform, player.GetPlayerInfo().AttackMin, player.GetPlayerInfo().AttackMax);
            }
            else if(player.m_State == Character.CharacterState.Skill)
            {
                TakeDamage(other.transform, player.GetPlayerInfo().AttackMin * 2, player.GetPlayerInfo().AttackMax * 2);
            }
            navState = NavState.Stop;
        }
    }

    public void DamageEnd()
    {
        m_State = CharacterState.Idle;
        navState = NavState.Patrol;
    }

    private IEnumerator EnemyActionRoutine()
    {
        while (m_State != CharacterState.Death)
        {
            switch (navState)
            {
                case NavState.Stop:
                    m_Nav.isStopped = true;
                    break;

                case NavState.Patrol:
                    m_Nav.isStopped = false;
                    if(Vector3.Distance(transform.position, patrolSites[patrolIndex].position) <= 1.5f)
                    {
                        patrolIndex++;
                        if (patrolIndex >= patrolSites.Length)
                            patrolIndex = 1;
                    }
                    m_Nav.SetDestination(patrolSites[patrolIndex].position);
                    Debug.Log(patrolIndex);
                    // Check Player Access
                    Collider[] targetDetect = Physics.OverlapSphere(transform.position, 3f, LayerMask.GetMask("Player"));
                    foreach(Collider hit in targetDetect)
                    {
                        targetTransform = hit.transform;
                        navState = NavState.Tracking;
                        break;
                    }
                    break;
                case NavState.Tracking:
                    m_Nav.isStopped = false;
                    m_Nav.SetDestination(targetTransform.position);
                    Debug.Log(targetTransform.position);
                    Debug.Log(transform.position);
                    break;
            }
            yield return null;
        }
    }

}
