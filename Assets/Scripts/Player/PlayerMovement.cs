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

    // Character Move Components
    private Vector3 moveVec;
    private float moveVelocityY = 0f;
    [SerializeField] private float moveSpeed = 5.7f;

    // Character Attack Components
    [SerializeField]private TrailRenderer attackLine;
    [SerializeField]private GameObject attackArea;
    private bool isSpecialAttack = false;

    void Awake()
    {
        m_Controller = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
        m_Input = GetComponent<PlayerInput>();
        m_Character = GetComponent<PlayerCharacter>();
        attackLine.enabled = false;
        attackArea.SetActive(false);
    }

    // Update is called once per frame
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
            case PlayerCharacter.PlayerState.Idle:
                moveVec = new Vector3(m_Input.h_Axis, 0f, m_Input.v_Axis).normalized * moveSpeed;
                break;
            case PlayerCharacter.PlayerState.Attack:
                moveVec = new Vector3(m_Input.h_Axis, 0f, m_Input.v_Axis).normalized * 1.2f;
                break;
        }
        m_Animator.SetFloat("Movement", moveVec.magnitude);
        transform.LookAt(transform.position + moveVec);
        moveVec.y = moveVelocityY;
        m_Controller.Move(moveVec * Time.deltaTime);
        if (m_Controller.isGrounded)
            moveVelocityY = 0f;
    }

    private void AttackFunc()
    {
        if (m_Character.m_State == PlayerCharacter.PlayerState.Attack)
            return;
        if (m_Input.NormalAttack)
        {
            m_Animator.SetTrigger("DoAttack");
            m_Character.m_State = PlayerCharacter.PlayerState.Attack;
        }
    }

    private void SpecialAttackFunc()
    {
        if (m_Character.m_State == PlayerCharacter.PlayerState.Attack)
            return;
        if (m_Input.SpecialAttack)
        {
            m_Animator.SetTrigger("DoSpecialAttack");
            m_Character.m_State = PlayerCharacter.PlayerState.Attack;
        }
    }

    public void AttackEffect()
    {
        attackLine.enabled = true;
        if(!isSpecialAttack)
            attackArea.SetActive(true);
        else
        {

        }
    }

    public void AttackEnd()
    {
        attackLine.enabled = false;
        attackArea.SetActive(false);
        m_Character.m_State = PlayerCharacter.PlayerState.Idle;
    }
}
