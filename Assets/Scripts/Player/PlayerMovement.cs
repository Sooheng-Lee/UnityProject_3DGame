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
        specialAttackArea.SetActive(false);
        specialAttackEffect.SetActive(false);
    }

    // Update.Equal(called once per frame
    void Update()
    {
        AttackFunc();
        SpecialAttackFunc();
        OnDamagedFunc();
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
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
        RaycastHit hit;
        bool onHit = Physics.SphereCast(transform.position, 0.5f, transform.forward, out hit, 1f, LayerMask.GetMask("Enemy"));
        if (onHit)
        {
            Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
            m_Controller.Move(-moveVec * Time.deltaTime);
        }
        m_Controller.Move(moveVec * Time.deltaTime);
        if (m_Controller.isGrounded)
            moveVelocityY = 0f;
    }
    private void OnDamagedFunc()
    {
        if(m_Character.m_State==Character.CharacterState.Damaged)
        {
            attackArea.SetActive(false);
            attackLine.enabled = false;
            specialAttackArea.SetActive(false);
            specialAttackEffect.SetActive(false);
        }
    }
    private void AttackFunc()
    {
        if (m_Character.m_State != Character.CharacterState.Idle)
            return;
        if (m_Input.NormalAttack)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f, LayerMask.GetMask("NPC")))
            {
                NPC npc = hit.collider.GetComponent<NPC>();
                npc.TalkToNPC(transform);
                return;
            }
            m_Animator.SetTrigger("DoAttack");
            m_Character.m_State = Character.CharacterState.Attack;
        }
    }

    private void SpecialAttackFunc()
    {
        if (m_Character.m_State != Character.CharacterState.Idle)
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
