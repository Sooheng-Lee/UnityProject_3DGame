using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    private enum MonsterType
    {
        Slime,
        Shell
    }

    [SerializeField] private float patrolSpeed;
    [SerializeField] private float trackSpeed;
    [SerializeField] MonsterType m_MonsterType;
    private NavMeshAgent m_Nav;
    private Animator m_Animator;
    void Awake()
    {
        m_Audio = GetComponent<AudioSource>();
        m_Controller = GetComponent<CharacterController>();
        m_Nav = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        switch (m_MonsterType)
        {
            case MonsterType.Slime:
                SetCharacterInfo("Slime", 50, 5, 5, 1);
                break;
            case MonsterType.Shell:
                SetCharacterInfo("Shell", 80, 10, 10, 5);
                break;

        }
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PlayerAttack")
        {
            
        }
    }

}
