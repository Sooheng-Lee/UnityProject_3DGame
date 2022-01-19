using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Character State Component
    private PlayerCharacter m_Character;

    // Unity Components
    private CharacterController m_Controller;
    private Animator m_Animator;
    private PlayerInput m_Input;
    private AudioSource m_Audio;

    // Character Move Components
    private Vector3 moveVec;
    private float moveVelocityY = 0f;
    [SerializeField] private float moveSpeed = 5.7f;

    // Character Attack Components
    [SerializeField] private AudioClip SwingClip;
    [SerializeField] private AudioClip WalkingClip;
    [SerializeField] private TrailRenderer attackLine;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject specialAttackArea;
    [SerializeField] private GameObject specialAttackEffect;

    void Awake()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_Input = GetComponent<PlayerInput>();
        m_Character = GetComponent<PlayerCharacter>();
        m_Audio = GetComponent<AudioSource>();

    }

    private void Start()
    {
        m_Character.m_State = Character.CharacterState.Idle;
        attackLine.enabled = false;
        attackArea.SetActive(false);
        specialAttackEffect.SetActive(false);
    }

    // Update.Equal(called once per frame
    void Update()
    {
        AttackFunc();
        SpecialAttackFunc();
    }

    private void FixedUpdate()
    {
        // Update Character Move Direction and Velocity
        moveVelocityY += Physics.gravity.y * Time.deltaTime;
        switch (m_Character.m_State) 
        {
            case Character.CharacterState.Idle:
                moveVec = new Vector3(m_Input.h_Axis, 0f, m_Input.v_Axis).normalized * moveSpeed;
                m_Animator.SetFloat("Movement", moveVec.magnitude);
                transform.LookAt(transform.position + moveVec);
                break;
            case Character.CharacterState.Attack:
                moveVec = new Vector3(m_Input.h_Axis, 0f, m_Input.v_Axis).normalized * 1.2f;
                break;
            case Character.CharacterState.Skill:
                moveVec = new Vector3(m_Input.h_Axis, 0f, m_Input.v_Axis).normalized * 1.2f;
                break;
        }
        moveVec.y = moveVelocityY;
        m_Controller.Move(moveVec * Time.deltaTime);
        if (m_Controller.isGrounded)
            moveVelocityY = 0f;
    }

    private void AttackFunc()
    {
        if (m_Character.m_State == Character.CharacterState.Attack || m_Character.m_State == Character.CharacterState.Skill)
            return;
        if (m_Input.NormalAttack)
        {
            m_Animator.SetTrigger("DoAttack");
            PlaySwingSound();
            m_Character.m_State = Character.CharacterState.Attack;
        }
    }

    private void SpecialAttackFunc()
    {
        if (m_Character.m_State == Character.CharacterState.Attack || m_Character.m_State == Character.CharacterState.Skill)
            return;

        if (m_Input.SpecialAttack && m_Character.UseMP(10) >= 0)
        {
            m_Animator.SetTrigger("DoSpecialAttack");
            m_Character.m_State = Character.CharacterState.Skill;
        }
    }

    public void AttackEffect()
    {
        attackLine.enabled = true;
        if (m_Character.m_State == Character.CharacterState.Attack)
            attackArea.SetActive(true);
        else if(m_Character.m_State == Character.CharacterState.Skill)
        {
            specialAttackArea.SetActive(true);
            specialAttackEffect.SetActive(true);
        }
    }

    public void AttackEnd()
    {
        attackLine.enabled = false;
        attackArea.SetActive(false);
        specialAttackArea.SetActive(false);
        specialAttackEffect.SetActive(false);
        m_Character.m_State = Character.CharacterState.Idle;
    }

    public void PlaySwingSound()
    {
        float volume = 0.2f;
        if (m_Character.m_State == Character.CharacterState.Attack)
            volume = 0.2f;
        else if(m_Character.m_State == Character.CharacterState.Skill)
            volume = 0.8f;
        m_Audio.PlayOneShot(SwingClip, volume);
    }

    public void PlayWalkingSound()
    {
        m_Audio.PlayOneShot(WalkingClip, 0.2f);
    }
}
